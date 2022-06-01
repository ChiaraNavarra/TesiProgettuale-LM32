using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TesiMagistraleLM32.Models;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Security.Claims;

namespace TesiMagistraleLM32.Controllers
{
    public class AnimaleController : Controller
    {
        private readonly ILogger<AnimaleController> _logger;

        private static readonly HttpClient client = new HttpClient();

        private readonly UserManager<IdentityUser> _userManager;



        public AnimaleController(ILogger<AnimaleController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [Route("Animale/RazzeCanine")]
        public IActionResult RazzeCanine()
        {
            return View();
        }

        

        [HttpGet]
        public async Task<IActionResult> _ListRazzeCanine(RazzaCaninaViewModel vm)
        {
            var isOk = true;
            var listvmodel = new List<RazzaCaninaViewModel>();
            try
            {
                var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/getapibreedslist/");
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;
                var list = root.EnumerateArray().ToList();
                foreach(var item in list)
                {
                    var model = new RazzaCaninaViewModel();
                    model.id = item.GetProperty("id").ToString();
                    model.name = item.GetProperty("name").ToString();
                    var image = item.GetProperty("image");
                    JObject jobject = JObject.Parse(item.ToString());
                    JObject jobject2 = JObject.Parse(image.ToString());
                    JToken breed_group = jobject.SelectToken("breed_group");
                    JToken origin = jobject.SelectToken("origin");
                    JToken bred_for = jobject.SelectToken("bred_for");
                    JToken life_span = jobject.SelectToken("life_span");
                    JToken temperament = jobject.SelectToken("temperament");
                    JToken url = jobject2.SelectToken("id");
                    if (breed_group != null)
                    {
                        model.breed_group = item.GetProperty("breed_group").ToString();
                    }
                    if (bred_for != null)
                    {
                        model.bred_for = item.GetProperty("bred_for").ToString();
                    }
                    if (origin != null)
                    {
                        model.origin = item.GetProperty("origin").ToString();
                    }
                    if (life_span != null)
                    {
                        model.life_span = item.GetProperty("life_span").ToString();
                    }
                    if (temperament != null)
                    {
                        model.temperament = item.GetProperty("temperament").ToString();
                    }
                    if (url != null)
                    {
                        model.img = image.GetProperty("id").ToString();
                    }

                    listvmodel.Add(model);
                }

                
                return PartialView("_ListRazzeCanine", listvmodel.AsReadOnly());
            }
            catch (Exception ex)
            {
                isOk = false;
            }

            return PartialView("_ListRazzeCanine", new RazzaCaninaViewModel());

        }

       
        [HttpGet]
        public async Task<IActionResult> _ListRazzeCanineFiltrata(RazzaCaninaViewModel vm)
        {
            var isOk = true;
            var listvmodel = new List<RazzaCaninaViewModel>();
            try
            {
                var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/getapibreedslistbyname/" + vm.name);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;
                var list = root.EnumerateArray().ToList();
                foreach (var item in list)
                {
                    var model = new RazzaCaninaViewModel();
                    model.id = item.GetProperty("id").ToString();
                    model.name = item.GetProperty("name").ToString();
                    JObject jobject = JObject.Parse(item.ToString());
                    JToken breed_group = jobject.SelectToken("breed_group");
                    JToken origin = jobject.SelectToken("origin");
                    JToken bred_for = jobject.SelectToken("bred_for");
                    JToken life_span = jobject.SelectToken("life_span");
                    JToken temperament = jobject.SelectToken("temperament");
                    JToken url = jobject.SelectToken("reference_image_id");
                    if (breed_group != null)
                    {
                        model.breed_group = item.GetProperty("breed_group").ToString();
                    }
                    if (bred_for != null)
                    {
                        model.bred_for = item.GetProperty("bred_for").ToString();
                    }
                    if (origin != null)
                    {
                        model.origin = item.GetProperty("origin").ToString();
                    }
                    if (life_span != null)
                    {
                        model.life_span = item.GetProperty("life_span").ToString();
                    }
                    if (temperament != null)
                    {
                        //var jsonResponseTraslate = await client.PostAsJsonAsync("http://host.docker.internal:5001/translate/", item.GetProperty("temperament").ToString());
                        //using JsonDocument docTraslate = JsonDocument.Parse(jsonResponseTraslate);
                        //JsonElement rootTraslate = docTraslate.RootElement;
                        model.temperament = item.GetProperty("temperament").ToString();
                    }
                    if (url != null)
                    {
                        var imageId = item.GetProperty("reference_image_id").ToString();
                        var jsonResponseImage = await client.GetStringAsync("http://host.docker.internal:5001/getbreedimagebyname/" + imageId);
                        using JsonDocument docImage = JsonDocument.Parse(jsonResponseImage);
                        JsonElement rootImage = docImage.RootElement;
                        model.img = rootImage.GetProperty("url").ToString();
                    }

                    listvmodel.Add(model);
                }


                return PartialView("_ListRazzeCanine", listvmodel.AsReadOnly());
            }
            catch (Exception ex)
            {
                isOk = false;
            }

            return PartialView("_ListRazzeCanine", new RazzaCaninaViewModel());

        }

        [HttpGet]
        public async Task<IActionResult> _ModalDettaglioRazza(string name, string temperament, string img)
        {
            var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/getbreedimagebyname/" + img);
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;
            ViewBag.Temperament = temperament;
            ViewBag.Img = root.GetProperty("url").ToString();
            ViewBag.Name = name;

            return await Task.FromResult(PartialView("_ModalDettaglioRazza"));
        }

        [Route("Animale/RazzeFeline")]
        public IActionResult RazzeFeline()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> _ListRazzeFeline(RazzaFelinaViewModel vm)
        {
            var isOk = true;
            var listvmodel = new List<RazzaFelinaViewModel>();
            try
            {
                var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/getapicatbreedslist/");
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;
                var list = root.EnumerateArray().ToList();
                foreach (var item in list)
                {
                    var model = new RazzaFelinaViewModel();
                    model.id = item.GetProperty("id").ToString();
                    model.name = item.GetProperty("name").ToString();
                    var isPresentImage = item.TryGetProperty("image", out JsonElement returnElement);
                    JObject jobject = JObject.Parse(item.ToString());
                    JToken origin = jobject.SelectToken("origin");
                    JToken life_span = jobject.SelectToken("life_span");
                    JToken temperament = jobject.SelectToken("temperament");
                    JToken wikipedia_url = jobject.SelectToken("wikipedia_url");
                    if (isPresentImage)
                    {
                        var image = item.GetProperty("image");
                        JObject jobject2 = JObject.Parse(image.ToString());
                        JToken url = jobject2.SelectToken("id");
                        if (url != null)
                        {
                            model.img = image.GetProperty("id").ToString();
                        }
                    }

                    if (origin != null)
                    {
                        model.origin = item.GetProperty("origin").ToString();
                    }
                    if (life_span != null)
                    {
                        model.life_span = item.GetProperty("life_span").ToString();
                    }
                    if (temperament != null)
                    {
                        model.temperament = item.GetProperty("temperament").ToString();
                    }
                    if (wikipedia_url != null)
                    {
                        model.wikipedia_url = item.GetProperty("wikipedia_url").ToString();
                    }


                    listvmodel.Add(model);
                }


                return PartialView("_ListRazzeFeline", listvmodel.AsReadOnly());
            }
            catch (Exception ex)
            {
                isOk = false;
            }

            return PartialView("_ListRazzeFeline", new RazzaFelinaViewModel());

        }

        [HttpGet]
        public async Task<IActionResult> _ListRazzeFelineFiltrata(RazzaFelinaViewModel vm)
        {
            var isOk = true;
            var listvmodel = new List<RazzaFelinaViewModel>();
            try
            {
                var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/getcatapibreedslistbyname/" + vm.name);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;
                var list = root.EnumerateArray().ToList();
                foreach (var item in list)
                {
                    var model = new RazzaFelinaViewModel();
                    model.id = item.GetProperty("id").ToString();
                    model.name = item.GetProperty("name").ToString();
                    JObject jobject = JObject.Parse(item.ToString());
                    JToken origin = jobject.SelectToken("origin");
                    JToken life_span = jobject.SelectToken("life_span");
                    JToken temperament = jobject.SelectToken("temperament");
                    JToken wikipedia_url = jobject.SelectToken("wikipedia_url");
                    JToken url = jobject.SelectToken("reference_image_id");
                    if (origin != null)
                    {
                        model.origin = item.GetProperty("origin").ToString();
                    }
                    if (life_span != null)
                    {
                        model.life_span = item.GetProperty("life_span").ToString();
                    }
                    if (temperament != null)
                    {
                        model.temperament = item.GetProperty("temperament").ToString();
                    }
                    if (wikipedia_url != null)
                    {
                        model.wikipedia_url = item.GetProperty("wikipedia_url").ToString();
                    }
                    if (url != null)
                    {
                        var imageId = item.GetProperty("reference_image_id").ToString();
                        var jsonResponseImage = await client.GetStringAsync("http://host.docker.internal:5001/getcatbreedimagebyname/" + imageId);
                        using JsonDocument docImage = JsonDocument.Parse(jsonResponseImage);
                        JsonElement rootImage = docImage.RootElement;
                        model.img = rootImage.GetProperty("url").ToString();
                    }

                    listvmodel.Add(model);
                }


                return PartialView("_ListRazzeFeline", listvmodel.AsReadOnly());
            }
            catch (Exception ex)
            {
                isOk = false;
            }

            return PartialView("_ListRazzeFeline", new RazzaFelinaViewModel());

        }

    }
}