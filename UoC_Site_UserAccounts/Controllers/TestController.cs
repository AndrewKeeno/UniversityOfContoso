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
    public class TestController : Controller
    {
        private UserDbContext db;
        private UserManager<Student> manager;

        private HttpClient client;
        private List<MediaTypeFormatter> mediaFormatter = new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() };

        public TestController()
        {
            db = new UserDbContext();
            manager = new UserManager<Student>(new UserStore<Student>(db));

            client = new HttpClient();
            client.BaseAddress = new Uri("http://uoc-api-akee516.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Test
        [HttpGet]
        public async Task<ActionResult> List()
        {
            try
            {
                var currentUserId = manager.FindById(User.Identity.GetUserId()).Id;
                IEnumerable<Test> testList = null;

                var response = await client.GetAsync("api/Tests?studentId=" + currentUserId);
                response.EnsureSuccessStatusCode();

                testList = await response.Content.ReadAsAsync<IEnumerable<Test>>(mediaFormatter);

                return View("List", testList);
            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: Test/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var test = await getModel(id);
            return
                test != null
                ?
                View("Details", test)
                :
                View("Error", new { message = "" });
        }

        // GET: Test/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Test/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Weighting,Marks,OutOf,WeightedScore")] Test test)
        {
            try
            {
                var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
                if (ModelState.IsValid)
                {
                    var response = await client.PostAsJsonAsync("api/Tests?studentId=" + currentUser.Id, test);
                    response.EnsureSuccessStatusCode();

                    Uri ToDoItemUrl = response.Headers.Location;

                    return RedirectToAction("Index");
                }

                return View("Create", test);

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: Test/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var test = await getModel(id);
            return
                test != null
                ?
                View("Details", test)
                :
                View("Error");
        }

        // PUT: Test/Edit/5
        [HttpPut]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Weighting,Marks,OutOf,WeightedScore")] Test test)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await client.PutAsJsonAsync("api/Tests/" + test.Id, test);
                    response.EnsureSuccessStatusCode();

                    Uri ToDoItemUrl = response.Headers.Location;
                    return RedirectToAction("Index");
                }

                return View("Edit", test);

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: Test/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var test = await getModel(id);
            return
                test != null
                ?
                View("Details", test)
                :
                View("Error");
        }

        // DELETE: Test/Delete/5
        [HttpDelete]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                var response = await client.DeleteAsync("api/Tests/" + id);
                response.EnsureSuccessStatusCode();

                Uri ToDoItemUrl = response.Headers.Location;

                return RedirectToAction("Index");

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        private async Task<Test> getModel(int id)
        {
            Test model = null;

            var response = await client.GetAsync("api/Tests/" + id);
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsAsync<Test>(mediaFormatter);
            }

            return model;
        }

        ~TestController()
        {
            client.Dispose();
        }
    }
}
