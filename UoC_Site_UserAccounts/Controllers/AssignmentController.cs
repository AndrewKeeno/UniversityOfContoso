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
    public class AssignmentController : Controller
    {
        private UserDbContext db;
        private UserManager<Student> manager;

        private HttpClient client;
        private List<MediaTypeFormatter> mediaFormatter = new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() };

        public AssignmentController()
        {
            db = new UserDbContext();
            manager = new UserManager<Student>(new UserStore<Student>(db));

            client = new HttpClient();
            client.BaseAddress = new Uri("http://uoc-api-akee516.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Assignment
        [HttpGet]
        public async Task<ActionResult> List()
        {
            try
            {
                var currentUserId = manager.FindById(User.Identity.GetUserId()).Id;
                IEnumerable<Assignment> assignmentList = null;

                var response = await client.GetAsync("api/Assignments?studentId=" + currentUserId);
                response.EnsureSuccessStatusCode();

                assignmentList = await response.Content.ReadAsAsync<IEnumerable<Assignment>>(mediaFormatter);

                return View("List", assignmentList);
            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: Assignment/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var assignment = await getModel(id);
            return
                assignment != null
                ?
                View("Details", assignment)
                :
                View("Error", new { message = "" });
        }

        // GET: Assignment/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Assignment/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Weighting,Marks,OutOf,WeightedScore,LinkToWork")] Assignment assignment)
        {
            try
            {
                var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
                if (ModelState.IsValid)
                {
                    var response = await client.PostAsJsonAsync("api/Assignments?studentId=" + currentUser.Id, assignment);
                    response.EnsureSuccessStatusCode();

                    Uri ToDoItemUrl = response.Headers.Location;

                    return RedirectToAction("Index");
                }

                return View("Create", assignment);

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: Assignment/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var assignment = await getModel(id);
            return
                assignment != null
                ?
                View("Edit", assignment)
                :
                View("Error", new { message = "" });
        }

        // PUT: Assignment/Edit/5
        [HttpPut]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Weighting,Marks,OutOf,WeightedScore,LinkToWork")] Assignment assignment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await client.PutAsJsonAsync("api/Assignments/" + assignment.Id, assignment);
                    response.EnsureSuccessStatusCode();

                    Uri ToDoItemUrl = response.Headers.Location;
                    return RedirectToAction("Index");
                }

                return View("Edit", assignment);

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: Assignment/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var assignment = await getModel(id);
            return
                assignment != null
                ?
                View("Delete", assignment)
                :
                View("Error", new { message = "" });
        }

        // DELETE: Assignment/Delete/5
        [HttpDelete]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                var response = await client.DeleteAsync("api/Assignments/" + id);
                response.EnsureSuccessStatusCode();

                Uri ToDoItemUrl = response.Headers.Location;

                return RedirectToAction("Index");

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        private async Task<Assignment> getModel(int id)
        {
            Assignment model = null;

            var response = await client.GetAsync("api/Assignments/" + id);
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsAsync<Assignment>(mediaFormatter);
            }

            return model;
        }

        ~AssignmentController()
        {
            client.Dispose();
        }
    }
}
