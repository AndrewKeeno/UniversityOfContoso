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
    public class ToDoItemsController : ApiController
    {
        private UoC_APIContext db = new UoC_APIContext();

        // GET: api/ToDoItems&&studentId
        public IQueryable<ToDoItem> GetToDoItems(int studentId)
        {
            var list = db.ToDoItems.Where(c => c.Student.ID == studentId);

            return list;
        }

        // GET: api/ToDoItems/5
        [ResponseType(typeof(ToDoItem))]
        public IHttpActionResult GetToDoItem(int id)
        {
            ToDoItem toDoItem = db.ToDoItems.Find(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return Ok(toDoItem);
        }

        // PUT: api/ToDoItems/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutToDoItem(int id, ToDoItem toDoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != toDoItem.ID)
            {
                return BadRequest();
            }

            db.Entry(toDoItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(id))
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

        // POST: api/ToDoItems
        [ResponseType(typeof(ToDoItem))]
        public IHttpActionResult PostToDoItem(ToDoItem toDoItem, int studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ToDoItems.Add(toDoItem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ToDoItemExists(toDoItem.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = toDoItem.ID }, toDoItem);
        }

        // DELETE: api/ToDoItems/5
        [ResponseType(typeof(ToDoItem))]
        public IHttpActionResult DeleteToDoItem(int id)
        {
            ToDoItem toDoItem = db.ToDoItems.Find(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            db.ToDoItems.Remove(toDoItem);
            db.SaveChanges();

            return Ok(toDoItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ToDoItemExists(int id)
        {
            return db.ToDoItems.Count(e => e.ID == id) > 0;
        }
    }
}