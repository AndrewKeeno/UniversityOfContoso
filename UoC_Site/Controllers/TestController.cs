using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Mvc;
using UoC_Site.Models;

namespace UoC_Site.Controllers
{
    public class TestController : Controller
    {
        private HttpClient client;
        private List<MediaTypeFormatter> mediaFormatter = new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() };

        public TestController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://uoc-api-akee516.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Test/Details/5
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Details(int id)
        {
            return View("TestDetails", await getModel(id));
        }

        // GET: Test/Create
        [HttpGet]
        public ActionResult Create(int courseId)
        {
            return View("TestCreate", new Test() { Course = new Course() { ID = courseId } });
        }

        // POST: Test/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(FormCollection collection)
        {
            try
            {
                Test model = new Test();

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

                var response = await client.PostAsJsonAsync("api/Tests?courseId=" + collection["CourseId"], model);
                response.EnsureSuccessStatusCode();

                Uri testUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Test/Edit/5
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id)
        {
            return View("TestEdit", await getModel(id));
        }

        // PUT: Test/Edit/5
        [HttpPut]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, FormCollection collection)
        {
            try
            {
                Test model = new Test();

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

                var response = await client.PutAsJsonAsync("api/Tests/" + id, model);
                response.EnsureSuccessStatusCode();

                Uri testUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Test/Delete/5
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            return View("TestDelete", await getModel(id));
        }

        // DELETE: Test/Delete/5
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                var response = await client.DeleteAsync("api/Tests/" + id);
                response.EnsureSuccessStatusCode();
                Uri testUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home");
            }
            catch
            {
                return View("Error");
            }
        }

        private async System.Threading.Tasks.Task<Test> getModel(int id)
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