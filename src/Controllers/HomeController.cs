using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FaPicClient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FaPicClient.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);       
              

                var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);                
                    StringContent httpConent = new StringContent(JsonConvert.SerializeObject(Convert.ToBase64String(memoryStream.ToArray())), Encoding.UTF8, "application/json");
                    var content = await client.PostAsync("http://localhost:5001/image", httpConent);               
            }

            return Redirect("Index");
        }
        public IActionResult Logout() => SignOut("Cookies", "oidc");

        public async Task<IActionResult> Index()
        {
            var model = new HomeModel();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("http://localhost:5001/image");
           model.Images= JsonConvert.DeserializeObject<IList<ImageApiModel>> (content).Select(c=> Convert.FromBase64String(c.Image)).ToList();          

            return View(model);
        }
    }

    public class ImageApiModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
    }
}