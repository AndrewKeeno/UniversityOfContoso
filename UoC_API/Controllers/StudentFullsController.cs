using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using UoC_API.Models;

namespace UoC_API.Controllers
{
    public class StudentFullsController : ApiController
    {
        private UoC_APIContext db = new UoC_APIContext();

        // GET: api/StudentFulls/5
        [ResponseType(typeof(StudentFull))]
        public IHttpActionResult GetStudentFull(int id)
        {
            StudentFull studentFull = new StudentFull(db.Students.Find(id));

            if (studentFull == null)
            {
                return NotFound();
            }

            studentFull.Courses = 
                db
                .Courses
                .Where(c => c.Student.ID == studentFull.ID)
                .Select(c => new CourseEager()
                    {
                        ID = c.ID,
                        Title = c.Title,
                        Description = c.Description,
                        Credits = c.Credits,
                        CompletionPercentage = c.CompletionPercentage,
                        Grade = c.Grade
                    })
                .ToList();
            studentFull.ToDoItems =
                db
                .ToDoItems
                .Where(t => t.Student.ID == studentFull.ID)
                .Select(t => new ToDoItemEager()
                    {
                        ID = t.ID,
                        Message = t.Message,
                        SubmittedOn = t.SubmittedOn
                    })
                .ToList();
            
            foreach(var c in studentFull.Courses)
            {
                c.Tests = 
                    db
                    .Tests
                    .Where(t => t.Course.Student.ID == studentFull.ID && t.Course.ID == c.ID)
                    .Select(t => new TestEager()
                        {
                            ID = t.ID,
                            Name = t.Name,
                            TestDateTime = t.TestDateTime,
                            Weighting = t.Weighting,
                            Marks = t.Marks,
                            Score = t.Score
                        })
                    .ToList();
                c.Assignments =
                    db
                    .Assignments
                    .Where(a => a.Course.Student.ID == studentFull.ID && a.Course.ID == c.ID)
                    .Select(a => new AssignmentEager()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            DueDateTime = a.DueDateTime,
                            Weighting = a.Weighting,
                            Marks = a.Marks,
                            Score = a.Score,
                            LinkToWork = a.LinkToWork
                        })
                    .ToList();
            }

            return Ok(studentFull);
        }
    }
}