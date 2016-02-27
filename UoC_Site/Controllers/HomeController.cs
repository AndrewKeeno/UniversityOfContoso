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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async System.Threading.Tasks.Task<ActionResult> Overview(int id)
        {
            StudentFull model = new StudentFull();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://uoc-api-akee516.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                var response = await client.GetAsync("api/StudentFulls/" + id);
                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<StudentFull>(
                        new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() });
                }

            }

            return View(model);
        }
    }
}