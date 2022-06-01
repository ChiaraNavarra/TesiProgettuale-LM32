using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TesiMagistraleLM32.Models;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Security.Claims;
using System.Net;

namespace TesiMagistraleLM32.Controllers
{
    public class ForumController : Controller
    {
        private readonly ILogger<ForumController> _logger;

        private static readonly HttpClient client = new HttpClient();

        public ForumController(ILogger<ForumController> logger)
        {
            _logger = logger;
        }

        [Route("Forum/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> _ModalInserimentoDomanda()
        {
            
            return await Task.FromResult(PartialView("_ModalInserimentoDomanda"));
        }

        [HttpGet]
        public async Task<IActionResult> _ModalInserimentoCommento(long id)
        {
            ViewBag.IdForum = id;
            return await Task.FromResult(PartialView("_ModalInserimentoCommento"));
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SalvaNuovaDomanda(ForumViewModel model)
        {
            var isOk = false;
            var errorMsg = "";
            try
            {
                var request = new ForumViewModel();
                request.IdUtente = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                request.TipoForum = model.TipoForum;
                request.Testo = model.Testo;
                request.DataInizio = DateTime.Now;
                request.Titolo = model.Titolo;

                var stringContent = System.Text.Json.JsonSerializer.Serialize(request);
                var response = await client.PostAsJsonAsync("http://host.docker.internal:5001/domanda/", request);

                isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception ex)
            {
                isOk = false;
                errorMsg = "Inserimento domanda fallito";
                _logger.LogError(ex, errorMsg, null);
            }

            var jsonResponse = new
            {
                success = isOk,
                errorMsg = errorMsg
            };

            return Json(jsonResponse);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SalvaNuovoCommento(CommentoViewModel model)
        {
            var isOk = false;
            var errorMsg = "";
            try
            {
                var request = new CommentoViewModel();
                request.IdUtente = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                request.Testo = model.Testo;
                request.IdForum = model.IdForum;
                request.DataInizio = DateTime.Now;

                var stringContent = System.Text.Json.JsonSerializer.Serialize(request);
                var response = await client.PostAsJsonAsync("http://host.docker.internal:5001/commento/", request);

                isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception ex)
            {
                isOk = false;
                errorMsg = "Inserimento commento fallito";
                _logger.LogError(ex, errorMsg, null);
            }
            var jsonResponse = new
            {
                success = isOk,
                errorMsg = errorMsg
            };

            return Json(jsonResponse);
        }

        [HttpGet]
        public async Task<IActionResult> _ListDomande(string testo)
        {
            var isOk = true;
            var listvmodel = new List<ForumViewModel>();
            try
            {
                var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/domande/" + testo);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;
                var list = root.EnumerateArray().ToList();
                foreach (var item in list)
                {
                    var model = new ForumViewModel();
                    model.Id = Int64.Parse(item.GetProperty("id").ToString());
                    model.Titolo = item.GetProperty("titolo").ToString();
                    model.TipoForum = item.GetProperty("tipoForum").ToString();
                    model.DataInizio = DateTimeOffset.Parse(item.GetProperty("dataInizio").ToString());
                    model.Testo = item.GetProperty("testo").ToString();
                    model.UsernameUtente = item.GetProperty("usernameUtente").ToString();

                    listvmodel.Add(model);
                }

                foreach(var item in listvmodel)
                {
                    item.Commenti = new List<CommentoViewModel>();
                    var jsonResponseCommenti = await client.GetStringAsync("http://host.docker.internal:5001/commenti/" + item.Id);
                    using JsonDocument docCommenti = JsonDocument.Parse(jsonResponseCommenti);
                    JsonElement rootCommenti = docCommenti.RootElement;
                    var listCommenti = rootCommenti.EnumerateArray().ToList();

                    foreach(var c in listCommenti)
                    {
                        var commento = new CommentoViewModel();
                        commento.Id = Int64.Parse(c.GetProperty("id").ToString());
                        commento.DataInizio = DateTimeOffset.Parse(c.GetProperty("dataInizio").ToString());
                        commento.Testo = c.GetProperty("testo").ToString();
                        commento.UsernameUtente = c.GetProperty("usernameUtente").ToString();
                        item.Commenti.Add(commento);
                    }
                }


                return PartialView("_ListDomande", listvmodel.AsReadOnly());
            }
            catch (Exception ex)
            {
                isOk = false;
            }

            return PartialView("_ListDomande", new ForumViewModel());

        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> EliminaForum(long id)
        {
            var isOk = false;
            var errorMsg = "";
            try
            {

                var response = await client.DeleteAsync("http://host.docker.internal:5001/forum/" + id);

                isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception ex)
            {
                isOk = false;
                errorMsg = "Eliminazione forum fallito";
                _logger.LogError(ex, errorMsg, null);
            }
            var jsonResponse = new
            {
                success = isOk,
                errorMsg = errorMsg
            };

            return Json(jsonResponse);
        }


        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> EliminaCommento(long id)
        {
            var isOk = false;
            var errorMsg = "";
            try
            {

                var response = await client.DeleteAsync("http://host.docker.internal:5001/commento/" + id);

                isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception ex)
            {
                isOk = false;
                errorMsg = "Eliminazione commento fallito";
                _logger.LogError(ex, errorMsg, null);
            }
            var jsonResponse = new
            {
                success = isOk,
                errorMsg = errorMsg
            };

            return Json(jsonResponse);
        }

    }
}