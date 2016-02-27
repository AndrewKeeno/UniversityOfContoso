using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using UoC_Site_UserAccounts.Models;

namespace UoC_Site_UserAccounts.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private UserDbContext db;
        private UserManager<Student> manager;

        private HttpClient client;
        private List<MediaTypeFormatter> mediaFormatter = new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() };

        public CourseController()
        {
            db = new UserDbContext();
            manager = new UserManager<Student>(new UserStore<Student>(db));

            client = new HttpClient();
            client.BaseAddress = new Uri("http://uoc-api-akee516.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Course
        public async Task<ActionResult> List()
        {
            try
            {
                var currentUserId = manager.FindById(User.Identity.GetUserId()).Id;
                IEnumerable<Course> courseList = null;

                var response = await client.GetAsync("api/Courses?studentId=" + currentUserId);
                response.EnsureSuccessStatusCode();

                courseList = await response.Content.ReadAsAsync<IEnumerable<Course>>(mediaFormatter);

                return View("List", courseList);
            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: Course/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var course = await getModel(id);
            return
                course != null
                ?
                View("Details", course)
                :
                View("Error", new { message = "" });
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Course/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,Credits,Grade")] Course course)
        {
            try
            {
                var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
                if (ModelState.IsValid)
                {
                    var response = await client.PostAsJsonAsync("api/Courses?studentId=" + currentUser.Id, course);
                    response.EnsureSuccessStatusCode();

                    Uri ToDoItemUrl = response.Headers.Location;

                    return RedirectToAction("Index");
                }

                return View("Create", course);

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: Course/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var course = await getModel(id);
            return
                course != null
                ?
                View("Details", course)
                :
                View("Error");
        }

        // PUT: Course/Edit/5
        [HttpPut]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Credits,Grade")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await client.PutAsJsonAsync("api/Courses/" + course.Id, course);
                    response.EnsureSuccessStatusCode();

                    Uri ToDoItemUrl = response.Headers.Location;
                    return RedirectToAction("Index");
                }

                return View("Edit", course);

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: Course/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var course = await getModel(id);
            return
                course != null
                ?
                View("Details", course)
                :
                View("Error");
        }

        // DELETE: Course/Delete/5
        [HttpDelete]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                var response = await client.DeleteAsync("api/Courses/" + id);
                response.EnsureSuccessStatusCode();

                Uri ToDoItemUrl = response.Headers.Location;

                return RedirectToAction("Index");

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        private async Task<Course> getModel(int id)
        {
            Course model = null;

            var response = await client.GetAsync("api/ToDoItems/" + id);
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsAsync<Course>(mediaFormatter);
            }

            return model;
        }

        ~CourseController()
        {
            client.Dispose();
        }
    }
}
