using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using UoC_Site.Models;

namespace UoC_Site.Controllers
{
    public class ToDoItemController : Controller
    {
        private HttpClient client;
        private List<MediaTypeFormatter> mediaFormatter = new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() };

        public ToDoItemController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:58102/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: ToDoItem/List
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> List(int studentId)
        {
            IEnumerable<ToDoItem> toDoList = null;

            var response = await client.GetAsync("api/ToDoItems?studentId=" + studentId);
            if (response.IsSuccessStatusCode)
            {
                toDoList = await response.Content.ReadAsAsync<IEnumerable<ToDoItem>>(mediaFormatter);
            }

            return View("ToDoItemList", toDoList);
        }

        // GET: ToDoItem/Details/5
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Details(int id)
        {
            return View("ToDoItemDetails", await getModel(id));
        }

        // GET: ToDoItem/Create
        [HttpGet]
        public ActionResult Create(int studentId)
        {
            return View("ToDoItemCreate", new ToDoItem() { Student = new Student() { ID = studentId } });
        }

        // POST: ToDoItem/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(FormCollection collection)
        {
            try
            {
                ToDoItem model = new ToDoItem();

                var propertyInfo = model.GetType();

                foreach (var o in collection.AllKeys)
                {
                    try
                    { 
                        propertyInfo.GetProperty(o).SetValue(model, collection[o]);
                    }
                    catch
                    {
                        continue;
                    }
                }

                var response = await client.PostAsJsonAsync("api/ToDoItems?studentId=" + collection["StudentId"], model);
                response.EnsureSuccessStatusCode();

                Uri ToDoItemUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home", new { id = collection["StudentId"] });
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: ToDoItem/Edit/5
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id)
        {
            var model = await getModel(id);
            model.Student.ID = id;
            return View("ToDoItemEdit", model);
        }

        // PUT: ToDoItem/Edit/5
        [HttpPut]
        public async System.Threading.Tasks.Task<ActionResult> Edit(FormCollection collection)
        {
            try
            {
                ToDoItem model = new ToDoItem();

                var propertyInfo = model.GetType();

                foreach (var o in collection.AllKeys)
                {
                    try
                    {
                        propertyInfo.GetProperty(o).SetValue(model, collection[o]);
                    }
                    catch
                    {
                        continue;
                    }
                }

                var response = await client.PutAsJsonAsync("api/ToDoItems/" + collection["StudentId"], model);
                response.EnsureSuccessStatusCode();

                Uri ToDoItemUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: ToDoItem/Delete/5
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            return View("ToDoItemDelete", await getModel(id));
        }

        // DELETE: ToDoItem/Delete/5
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                var response = await client.DeleteAsync("api/ToDoItems/" + id);
                response.EnsureSuccessStatusCode();
                Uri ToDoItemUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home");
            }
            catch
            {
                return View("Error");
            }
        }

        private async System.Threading.Tasks.Task<ToDoItem> getModel(int id)
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
