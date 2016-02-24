using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Mvc;
using UoC_Site.Models;

namespace UoC_Site.Controllers
{
    public class CourseController : Controller
    {
        private HttpClient client;
        private List<MediaTypeFormatter> mediaFormatter = new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() };

        public CourseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:58102/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Course/Details/5
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Details(int id)
        {
            return View("CourseDetails", await getModel(id));
        }

        // GET: Course/Create
        [HttpGet]
        public ActionResult Create(int studentId)
        {
            return View("CourseCreate", new Course() { Student = new Student() { ID = studentId } } );
        }

        // POST: Course/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(FormCollection collection)
        {
            try
            {
                Course model = new Course();

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

                var response = await client.PostAsJsonAsync("api/Courses?studentId=" + collection["StudentId"], model);
                response.EnsureSuccessStatusCode();

                Uri CourseUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Course/Edit/5
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id)
        {
            return View("CourseEdit", await getModel(id));
        }

        // PUT: Course/Edit/5
        [HttpPut]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, FormCollection collection)
        {
            try
            {
                Course model = new Course();

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

                var response = await client.PutAsJsonAsync("api/Courses/" + id, model);
                response.EnsureSuccessStatusCode();

                Uri CourseUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Course/Delete/5
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            return View("CourseDelete", await getModel(id));
        }

        // POST: Course/Delete/5
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                var response = await client.DeleteAsync("api/Courses/" + id);
                response.EnsureSuccessStatusCode();
                Uri CourseUrl = response.Headers.Location;

                return RedirectToAction("Overview", "Home");
            }
            catch
            {
                return View("Error");
            }
        }

        private async System.Threading.Tasks.Task<Course> getModel(int id)
        {
            Course model = null;

            var response = await client.GetAsync("api/Courses/" + id);
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
