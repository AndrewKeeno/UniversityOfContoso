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
    public class TestsController : ApiController
    {
        private UoC_APIContext db = new UoC_APIContext();

        // GET: api/Tests
        public IQueryable<Test> GetTests(int studentID, int courseID, string sortBy = "", string thenBy = "", bool sortByAsc = true, bool thenByAsc = true)
        {
            var list = db.Tests.Where(a => a.Course.Student.ID == studentID && a.Course.ID == courseID);

            if (!sortBy.Equals("Marks") || !sortBy.Equals("Name") || !sortBy.Equals("TestDateTime") || !sortBy.Equals(""))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                { ReasonPhrase = "Sort By parameter \"" + sortBy + "\" is invalid" });
            }
            if (!thenBy.Equals("Marks") || !thenBy.Equals("Name") || !thenBy.Equals("TestDateTime") || !thenBy.Equals(""))
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

        // GET: api/Tests/5
        [ResponseType(typeof(Test))]
        public IHttpActionResult GetTest(int id)
        {
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return NotFound();
            }

            return Ok(test);
        }

        // PUT: api/Tests/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTest(int id, Test test)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != test.ID)
            {
                return BadRequest();
            }

            db.Entry(test).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(id))
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

        // POST: api/Tests&&courseID
        [ResponseType(typeof(Test))]
        public IHttpActionResult PostTest(Test test, int courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            test.Course = db.Courses.FirstOrDefault(c => c.ID == courseId);

            db.Tests.Add(test);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TestExists(test.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = test.ID }, test);
        }

        // DELETE: api/Tests/5
        [ResponseType(typeof(Test))]
        public IHttpActionResult DeleteTest(int id)
        {
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return NotFound();
            }

            db.Tests.Remove(test);
            db.SaveChanges();

            return Ok(test);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TestExists(int id)
        {
            return db.Tests.Count(e => e.ID == id) > 0;
        }
    }
}