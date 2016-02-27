using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Mvc;
using UoC_Site.Models;

namespace UoC_Site.Controllers
{
    public class AssignmentController : Controller
    {
        private HttpClient client;
        private List<MediaTypeFormatter> mediaFormatter = new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() };

        public AssignmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://uoc-api-akee516.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Assignment/Details/5
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Details(int id)
        {
            return View("AsssignmentDetails", await getModel(id));
        }

        // GET: Assignment/Create
        [HttpGet]
        public ActionResult Create(int courseId)
        {
            return View("AssignmentCreate", new Assignment() { Course = new Course() { ID = courseId } });
        }

        // POST: Assignment/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(FormCollection collection)
        {
            try
            {
                Assignment model = new Assignment();

                var propertyInfo = model.GetType();

                foreach (var o in collection.AllKeys)
                {
                    try
                    { 
                        propertyInfo.GetProperty(o).SetValue(model, collection[o]);
                    }
                    catch (NullReferenceException)
                    {
                        continue;
                    }
                }

                var response = await client.PostAsJsonAsync("api/Assignments?courseId=" + collection["CourseId"], model);
                response.EnsureSuccessStatusCode();

                Uri AssignmentUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Assignment/Edit/5
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id)
        {
            return View("AssignmentEdit", await getModel(id));
        }

        // POST: Assignment/Edit/5
        [HttpPut]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, FormCollection collection)
        {
            try
            {
                Assignment model = new Assignment();

                var propertyInfo = model.GetType();

                foreach (var o in collection.AllKeys)
                {
                    try
                    {
                        propertyInfo.GetProperty(o).SetValue(model, collection[o]);
                    }
                    catch (NullReferenceException)
                    {
                        continue;
                    }
                }

                var response = await client.PutAsJsonAsync("api/Assignments/" + id, model);
                response.EnsureSuccessStatusCode();

                Uri AssignmentUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Assignment/Delete/5
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            return View("AssignmentDelete", await getModel(id));
        }

        // POST: Assignment/Delete/5
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                var response = await client.DeleteAsync("api/Assignments/" + id);
                response.EnsureSuccessStatusCode();
                Uri AssignmentUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home");
            }
            catch
            {
                return View("Error");
            }
        }

        private async System.Threading.Tasks.Task<Assignment> getModel(int id)
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
