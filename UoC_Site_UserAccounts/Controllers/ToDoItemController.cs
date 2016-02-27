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
    public class ToDoItemController : Controller
    {
        private UserDbContext db;
        private UserManager<Student> manager;

        private HttpClient client;
        private List<MediaTypeFormatter> mediaFormatter = new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() };

        public ToDoItemController()
        {
            db = new UserDbContext();
            manager = new UserManager<Student>(new UserStore<Student>(db));

            client = new HttpClient();
            client.BaseAddress = new Uri("http://uoc-api-akee516.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: ToDoItem
        [HttpGet]
        public async Task<ActionResult> List()
        {
            try
            {
                var currentUserId = manager.FindById(User.Identity.GetUserId()).Id;
                IEnumerable<ToDoItem> toDoList = null;

                var response = await client.GetAsync("api/ToDoItems?studentId=" + currentUserId);
                response.EnsureSuccessStatusCode();
                
                toDoList = await response.Content.ReadAsAsync<IEnumerable<ToDoItem>>(mediaFormatter);

                return View("List", toDoList);
            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: ToDoItem/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var toDo = await getModel(id);
            return
                toDo != null
                ?
                View("Details", toDo)
                :
                View("Error", new { message = "" });
        }

        // GET: ToDoItem/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: ToDoItem/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id,Message,IsDone")] ToDoItem todo)
        {
            try
            {
                var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
                if (ModelState.IsValid)
                {
                    var response = await client.PostAsJsonAsync("api/ToDoItems?studentId=" + currentUser.Id, todo);
                    response.EnsureSuccessStatusCode();

                    Uri ToDoItemUrl = response.Headers.Location;

                    return RedirectToAction("Index");
                }

                return View("Create", todo);

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: ToDoItem/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var toDo = await getModel(id);
            return
                toDo != null
                ?
                View("Details", toDo)
                :
                View("Error");
        }

        // PUT: ToDoItem/Edit/5
        [HttpPut]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Message,IsDone")] ToDoItem todo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await client.PutAsJsonAsync("api/ToDoItems/" + todo.Id, todo);
                    response.EnsureSuccessStatusCode();

                    Uri ToDoItemUrl = response.Headers.Location;
                    return RedirectToAction("Index");
                }

                return View("Edit", todo);

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        // GET: ToDoItem/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var toDo = await getModel(id);
            return
                toDo != null
                ?
                View("Details", toDo)
                :
                View("Error");
        }

        // DELETE: ToDoItem/Delete/5
        [HttpDelete]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                var response = await client.DeleteAsync("api/ToDoItems/" + id);
                response.EnsureSuccessStatusCode();

                Uri ToDoItemUrl = response.Headers.Location;

                return RedirectToAction("Index");

            }
            catch
            {
                return View("Error", new { message = "" });
            }
        }

        private async Task<ToDoItem> getModel(int id)
        {
            ToDoItem model = null;

            var response = await client.GetAsync("api/ToDoItems/" + id);
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsAsync<ToDoItem>(mediaFormatter);
            }

            return model;
        }

        ~ToDoItemController()
        {
            client.Dispose();
        }
    }
}
