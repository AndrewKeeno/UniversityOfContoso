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
    public class AssignmentsController : ApiController
    {
        private UoC_APIContext db = new UoC_APIContext();

        // GET: api/Assignments
        public IQueryable<Assignment> GetAssignments(int studentID, int courseID, string sortBy = "", string thenBy = "", bool sortByAsc = true, bool thenByAsc = true)
        {
            var list = db.Assignments.Where(a => a.Course.Student.ID == studentID && a.Course.ID == courseID);

            //ID / Name / DueDateTime

            if (!sortBy.Equals("Name") || !sortBy.Equals("Marks") || !sortBy.Equals("DueDateTime") || !sortBy.Equals(""))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                { ReasonPhrase = "Sort By parameter \"" + sortBy + "\" is invalid" });
            }
            if (!thenBy.Equals("Name") || !thenBy.Equals("Marks") || !thenBy.Equals("DueDateTime") || !thenBy.Equals(""))
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

        // GET: api/Assignments/5
        [ResponseType(typeof(Assignment))]
        public IHttpActionResult GetAssignment(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return NotFound();
            }

            return Ok(assignment);
        }

        // PUT: api/Assignments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAssignment(int id, Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assignment.ID)
            {
                return BadRequest();
            }

            db.Entry(assignment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(id))
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

        // POST: api/Assignments&&courseId
        [ResponseType(typeof(Assignment))]
        public IHttpActionResult PostAssignment(Assignment assignment, int courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            assignment.Course = db.Courses.Find(courseId);

            db.Assignments.Add(assignment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AssignmentExists(assignment.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = assignment.ID }, assignment);
        }

        // DELETE: api/Assignments/5
        [ResponseType(typeof(Assignment))]
        public IHttpActionResult DeleteAssignment(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return NotFound();
            }

            db.Assignments.Remove(assignment);
            db.SaveChanges();

            return Ok(assignment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssignmentExists(int id)
        {
            return db.Assignments.Count(e => e.ID == id) > 0;
        }
    }
}