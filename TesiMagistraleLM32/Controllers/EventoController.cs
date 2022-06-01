using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TesiMagistraleLM32.Models;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Net;

namespace TesiMagistraleLM32.Controllers
{
    public class EventoController : Controller
    {
        private readonly ILogger<EventoController> _logger;

        private static readonly HttpClient client = new HttpClient();

        public EventoController(ILogger<EventoController> logger)
        {
            _logger = logger;
        }

        [Route("Evento/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> _ListEventi()
        {
            var isOk = true;
            var listvmodel = new List<EventoViewModel>();
            try
            {
                var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/eventi/");
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;
                var list = root.EnumerateArray().ToList();
                foreach (var item in list)
                {
                    var model = new EventoViewModel();
                    model.Id = Int64.Parse(item.GetProperty("id").ToString());
                    model.Titolo = item.GetProperty("titolo").ToString();
                    model.Comune = item.GetProperty("comune").ToString();
                    model.DataEvento = DateTime.Parse(item.GetProperty("dataEvento").ToString());
                    model.Descrizione = item.GetProperty("descrizione").ToString();
                    model.TipoEvento = item.GetProperty("tipoEvento").ToString();

                    listvmodel.Add(model);
                }


                return PartialView("_ListEventi", listvmodel.AsReadOnly());
            }
            catch (Exception ex)
            {
                isOk = false;
            }

            return PartialView("_ListEventi", new EventoViewModel());

        }

        [HttpGet]
        public async Task<IActionResult> _ModalInserimentoEvento()
        {
            var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/getcomuniitaliani/");
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;
            var list = root.EnumerateArray().ToList();
            var listvmodel = new List<ComuneViewModel>();
            foreach (var item in list)
            {
                var model = new ComuneViewModel();
                model.codice = item.GetProperty("codice").ToString();
                model.nome = item.GetProperty("nome").ToString();
                listvmodel.Add(model);
            }
            ViewBag.Comuni = listvmodel;
            return await Task.FromResult(PartialView("_ModalInserimentoEvento"));
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SalvaNuovoEvento(EventoViewModel model)
        {
            var isOk = false;
            var errorMsg = "";
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    errorMsg = "Presenza di campi non validi"
                });
            }
            try
            {
                var request = new EventoViewModel();
                request.Titolo = model.Titolo;
                request.TipoEvento = model.TipoEvento;
                request.DataEvento = model.DataEvento;
                request.Descrizione = model.Descrizione;
                request.Comune = model.Comune;

                var stringContent = System.Text.Json.JsonSerializer.Serialize(request);
                var response = await client.PostAsJsonAsync("http://host.docker.internal:5001/evento/", request);

                isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception ex)
            {
                isOk = false;
                errorMsg = "Inserimento evento fallito";
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