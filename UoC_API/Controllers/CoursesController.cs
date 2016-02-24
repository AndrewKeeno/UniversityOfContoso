using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using UoC_API.Extentions;
using UoC_API.Models;

namespace UoC_API.Controllers
{
    public class CoursesController : ApiController
    {
        private UoC_APIContext db = new UoC_APIContext();

        // GET: api/Courses/?[studentID=#]

        public IQueryable<Course> GetCourses(int studentID, string sortBy = "", string thenBy = "", bool sortByAsc = true, bool thenByAsc = true)
        {
            var list = db.Courses.Where(c => c.Student.ID == studentID);

            if (!sortBy.Equals("Title") || !sortBy.Equals("ID") || !sortBy.Equals("Grade") || !sortBy.Equals(""))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    { ReasonPhrase = "Sort By parameter \"" + sortBy + "\" is invalid" });
            }
            if (!thenBy.Equals("Title") || !thenBy.Equals("ID") || !thenBy.Equals("Grade") || !thenBy.Equals(""))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    { ReasonPhrase = "Sort By parameter \"" + thenBy + "\" is invalid" });
            }
            if (!thenBy.Equals("") && sortBy.Equals(""))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    { ReasonPhrase = "Can not sort Then By without first Sort By" });
            }

            try
            {
                if (!sortBy.Equals("") && !thenBy.Equals(""))
                {
                    list = list.ComprehensiveStringSort(sortBy, sortByAsc, thenBy, thenByAsc);
                }
                else if (!sortBy.Equals(""))
                {
                    list = sortByAsc ? list.OrderByString(sortBy) : list.OrderByStringDescending(sortBy);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return list;
        }

        // GET: api/Courses/5
        [ResponseType(typeof(Course))]
        public IHttpActionResult GetCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // PUT: api/Courses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCourse(int id, Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.ID)
            {
                return BadRequest();
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Courses&&studentId
        [ResponseType(typeof(Course))]
        public IHttpActionResult PostCourse(Course course, int studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            course.Student = db.Students.Find(studentId);

            db.Courses.Add(course);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CourseExists(course.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = course.ID }, course);
        }

        // DELETE: api/Courses/5
        [ResponseType(typeof(Course))]
        public IHttpActionResult DeleteCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            //Remove all tests and assignments that are linked to that course
            db.Tests.RemoveRange(db.Tests.Where(t => t.Course.ID == id));
            db.Assignments.RemoveRange(db.Assignments.Where(a => a.Course.ID == id));

            db.Courses.Remove(course);
            db.SaveChanges();

            return Ok(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(int id)
        {
            return db.Courses.Count(e => e.ID == id) > 0;
        }
    }
}