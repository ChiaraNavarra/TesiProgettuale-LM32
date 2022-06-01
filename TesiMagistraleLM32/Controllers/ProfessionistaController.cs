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
using System.Net;

namespace TesiMagistraleLM32.Controllers
{
    public class ProfessionistaController : Controller
    {
        private readonly ILogger<ProfessionistaController> _logger;

        private static readonly HttpClient client = new HttpClient();

        private readonly UserManager<IdentityUser> _userManager;



        public ProfessionistaController(ILogger<ProfessionistaController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }


        [Route("Professionista/AddestratoriCinofili")]
        public IActionResult AddestratoriCinofili()
        {
            return View();
        }

        [Route("Professionista/Veterinari")]
        public IActionResult Veterinari()
        {
            return View();
        }

        [Route("Professionista/Petsitter")]
        public IActionResult Petsitter()
        {
            return View();
        }
        

        [HttpGet]
        public async Task<IActionResult> _ListAddestratori()
        {
            var isOk = true;
            var listvmodel = new List<AddestratoreCinofiloViewModel>();
            try
            {
                var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/addestratoricinofili/");
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;
                var list = root.EnumerateArray().ToList();
                foreach (var item in list)
                {
                    var model = new AddestratoreCinofiloViewModel();
                    model.Id = item.GetProperty("id").ToString();
                    model.Nome = item.GetProperty("nome").ToString();
                    model.Cognome = item.GetProperty("cognome").ToString();
                    model.NumeroTelefono = item.GetProperty("numeroTelefono").ToString();
                    model.Note = item.GetProperty("note").ToString();
                    model.Comune = item.GetProperty("comune").ToString();
                    model.Email = item.GetProperty("email").ToString();
                    model.Indirizzo = item.GetProperty("indirizzo").ToString();

                    listvmodel.Add(model);
                }

                return PartialView("_ListAddestratori", listvmodel.AsReadOnly());
            }
            catch (Exception ex)
            {
                isOk = false;
            }

            return PartialView("_ListAddestratori", new AddestratoreCinofiloViewModel());

        }

        [HttpGet]
        public async Task<IActionResult> _ListPetsitter()
        {
            var isOk = true;
            var listvmodel = new List<PetsitterViewModel>();
            try
            {
                var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/petsitter/");
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;
                var list = root.EnumerateArray().ToList();
                foreach (var item in list)
                {
                    var model = new PetsitterViewModel();
                    model.Id = item.GetProperty("id").ToString();
                    model.Nome = item.GetProperty("nome").ToString();
                    model.Cognome = item.GetProperty("cognome").ToString();
                    model.NumeroTelefono = item.GetProperty("numeroTelefono").ToString();
                    model.Note = item.GetProperty("note").ToString();
                    model.Comune = item.GetProperty("comune").ToString();
                    model.Email = item.GetProperty("email").ToString();

                    listvmodel.Add(model);
                }

                return PartialView("_ListPetsitter", listvmodel.AsReadOnly());
            }
            catch (Exception ex)
            {
                isOk = false;
            }

            return PartialView("_ListPetsitter", new PetsitterViewModel());

        }

        [HttpGet]
        public async Task<IActionResult> _ListVeterinari()
        {
            var isOk = true;
            var listvmodel = new List<VeterinarioViewModel>();
            try
            {
                var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/veterinari/");
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;
                var list = root.EnumerateArray().ToList();
                foreach (var item in list)
                {
                    var model = new VeterinarioViewModel();
                    model.Id = item.GetProperty("id").ToString();
                    model.Nome = item.GetProperty("nome").ToString();
                    model.Cognome = item.GetProperty("cognome").ToString();
                    model.NumeroTelefono = item.GetProperty("numeroTelefono").ToString();
                    model.Note = item.GetProperty("note").ToString();
                    model.Comune = item.GetProperty("comune").ToString();
                    model.Email = item.GetProperty("email").ToString();
                    model.TipoVeterinario = item.GetProperty("tipoVeterinario").ToString();
                    model.Indirizzo = item.GetProperty("indirizzo").ToString();

                    listvmodel.Add(model);
                }

                return PartialView("_ListVeterinari", listvmodel.AsReadOnly());
            }
            catch (Exception ex)
            {
                isOk = false;
            }

            return PartialView("_ListVeterinari", new VeterinarioViewModel());

        }

        [HttpGet]
        public async Task<IActionResult> _ModalInserimentoVet(string id)
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

            var jsonResponseAddestratore = await client.GetStringAsync("http://host.docker.internal:5001/veterinario/" + id);
            using JsonDocument docAddestratore = JsonDocument.Parse(jsonResponseAddestratore);
            JsonElement rootAddestratore = docAddestratore.RootElement;
            var listAddestratore = rootAddestratore.EnumerateArray().ToList();
            var listvmodelAddestratore = new List<VeterinarioViewModel>();
            foreach (var item in listAddestratore)
            {
                var model = new VeterinarioViewModel();
                model.Nome = item.GetProperty("nome").ToString();
                model.Cognome = item.GetProperty("cognome").ToString();
                model.NumeroTelefono = item.GetProperty("numeroTelefono").ToString();
                model.Note = item.GetProperty("note").ToString();
                model.Comune = item.GetProperty("comune").ToString();
                model.TipoVeterinario = item.GetProperty("tipoVeterinario").ToString();
                model.Indirizzo = item.GetProperty("indirizzo").ToString();
                //model.Email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                listvmodelAddestratore.Add(model);
            }

            ViewBag.Comuni = listvmodel;
            return PartialView(listvmodelAddestratore.First());
        }


        [HttpGet]
        public async Task<IActionResult> _ModalInserimentoRecensione(string idAddestratore, string idPetsitter, string idVeterinario)
        {           
            
            ViewBag.IdAddestratore = idAddestratore;
            ViewBag.IdPetsitter = idPetsitter;
            ViewBag.IdVeterinario = idVeterinario;

            return await Task.FromResult(PartialView("_ModalInserimentoRecensione"));
        }

        [HttpGet]
        public async Task<IActionResult> _ModalVisualizzaRecensioniAddestratore(string id)
        {

            var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/recensioniaddestratore/" + id);
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;
            var list = root.EnumerateArray().ToList();
            var listvmodel = new List<RecensioneViewModel>();
            foreach (var item in list)
            {
                var model = new RecensioneViewModel();
                model.Id = Int64.Parse(item.GetProperty("id").ToString());
                model.Testo = item.GetProperty("testo").ToString();
                model.UsernameUtente = item.GetProperty("usernameUtente").ToString();
                listvmodel.Add(model);
            }

            return PartialView("_ModalVisualizzaRecensioni", listvmodel);
        }

        [HttpGet]
        public async Task<IActionResult> _ModalVisualizzaRecensioniPetsitter(string id)
        {

            var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/recensionipetsitter/" + id);
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;
            var list = root.EnumerateArray().ToList();
            var listvmodel = new List<RecensioneViewModel>();
            foreach (var item in list)
            {
                var model = new RecensioneViewModel();
                model.Id = Int64.Parse(item.GetProperty("id").ToString());
                model.Testo = item.GetProperty("testo").ToString();
                model.UsernameUtente = item.GetProperty("usernameUtente").ToString();
                listvmodel.Add(model);
            }

            return PartialView("_ModalVisualizzaRecensioni", listvmodel);
        }

        [HttpGet]
        public async Task<IActionResult> _ModalVisualizzaRecensioniVet(string id)
        {

            var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5001/recensionivet/" + id);
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;
            var list = root.EnumerateArray().ToList();
            var listvmodel = new List<RecensioneViewModel>();
            foreach (var item in list)
            {
                var model = new RecensioneViewModel();
                model.Id = Int64.Parse(item.GetProperty("id").ToString());
                model.Testo = item.GetProperty("testo").ToString();
                model.UsernameUtente = item.GetProperty("usernameUtente").ToString();
                listvmodel.Add(model);
            }

            return PartialView("_ModalVisualizzaRecensioni", listvmodel);
        }

        [HttpGet]
        public async Task<IActionResult> _ModalInserimentoAddestratore(string id)
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

            var jsonResponseAddestratore = await client.GetStringAsync("http://host.docker.internal:5001/addestratorecinofilo/" + id);
            using JsonDocument docAddestratore = JsonDocument.Parse(jsonResponseAddestratore);
            JsonElement rootAddestratore = docAddestratore.RootElement;
            var listAddestratore = rootAddestratore.EnumerateArray().ToList();
            var listvmodelAddestratore = new List<AddestratoreCinofiloViewModel>();
            foreach (var item in listAddestratore)
            {
                var model = new AddestratoreCinofiloViewModel();
                model.Nome = item.GetProperty("nome").ToString();
                model.Cognome = item.GetProperty("cognome").ToString();
                model.NumeroTelefono = item.GetProperty("numeroTelefono").ToString();
                model.Note = item.GetProperty("note").ToString();
                model.Comune = item.GetProperty("comune").ToString();
                model.Indirizzo = item.GetProperty("indirizzo").ToString();
                //model.Email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                listvmodelAddestratore.Add(model);
            }

            ViewBag.Comuni = listvmodel;
            return PartialView(listvmodelAddestratore.First());
        }

        [HttpGet]
        public async Task<IActionResult> _ModalInserimentoPetsitter(string id)
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

            var jsonResponseAddestratore = await client.GetStringAsync("http://host.docker.internal:5001/petsitter/" + id);
            using JsonDocument docAddestratore = JsonDocument.Parse(jsonResponseAddestratore);
            JsonElement rootAddestratore = docAddestratore.RootElement;
            var listAddestratore = rootAddestratore.EnumerateArray().ToList();
            var listvmodelAddestratore = new List<PetsitterViewModel>();
            foreach (var item in listAddestratore)
            {
                var model = new PetsitterViewModel();
                model.Nome = item.GetProperty("nome").ToString();
                model.Cognome = item.GetProperty("cognome").ToString();
                model.NumeroTelefono = item.GetProperty("numeroTelefono").ToString();
                model.Note = item.GetProperty("note").ToString();
                model.Comune = item.GetProperty("comune").ToString();
                //model.Email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                listvmodelAddestratore.Add(model);
            }

            ViewBag.Comuni = listvmodel;
            return PartialView(listvmodelAddestratore.First());
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SalvaNuovoAddestratore(AddestratoreCinofiloViewModel model)
        {
            var isOk = false;
            var errorMsg = "";
            try
            {
                var request = new AddestratoreCinofiloViewModel();
                request.Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                request.Nome = model.Nome;
                request.Cognome = model.Cognome;
                request.NumeroTelefono = model.NumeroTelefono;
                request.Note = model.Note;
                request.Comune = model.Comune;
                request.Indirizzo = model.Indirizzo;

                var stringContent = System.Text.Json.JsonSerializer.Serialize(request);
                var response = await client.PutAsJsonAsync("http://host.docker.internal:5001/addestratorecinofilo/", request);
                                
                isOk = response.StatusCode == HttpStatusCode.OK ? true: false;
            }
            catch (Exception ex)
            {
                isOk = false;
                errorMsg = "Aggiornamento addestratore fallito";
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
        public async Task<IActionResult> SalvaNuovoPetsitter(PetsitterViewModel model)
        {
            var isOk = false;
            var errorMsg = "";
            try
            {
                var request = new PetsitterViewModel();
                request.Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                request.Nome = model.Nome;
                request.Cognome = model.Cognome;
                request.NumeroTelefono = model.NumeroTelefono;
                request.Note = model.Note;
                request.Comune = model.Comune;

                var stringContent = System.Text.Json.JsonSerializer.Serialize(request);
                var response = await client.PutAsJsonAsync("http://host.docker.internal:5001/putpetsitter/", request);

                isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception ex)
            {
                isOk = false;
                errorMsg = "Aggiornamento Pet sitter fallito";
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
        public async Task<IActionResult> SalvaNuovoVet(VeterinarioViewModel model)
        {
            var isOk = false;
            var errorMsg = "";
            try
            {
                var request = new VeterinarioViewModel();
                request.Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                request.Nome = model.Nome;
                request.Cognome = model.Cognome;
                request.NumeroTelefono = model.NumeroTelefono;
                request.Note = model.Note;
                request.Comune = model.Comune;
                request.TipoVeterinario = model.TipoVeterinario;
                request.Indirizzo = model.Indirizzo;

                var stringContent = System.Text.Json.JsonSerializer.Serialize(request);
                var response = await client.PutAsJsonAsync("http://host.docker.internal:5001/putveterinario/", request);

                isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception ex)
            {
                isOk = false;
                errorMsg = "Aggiornamento Veterinario fallito";
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
        public async Task<IActionResult> SalvaNuovaRecensione(RecensioneViewModel model)
        {
            var isOk = false;
            var errorMsg = "";
            try
            {
                var request = new RecensioneViewModel();

                if (!string.IsNullOrWhiteSpace(model.IdAddestratore))
                {
                    request.Testo = model.Testo;
                    request.IdAddestratore = model.IdAddestratore;
                    request.IdUtente = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    var stringContent = System.Text.Json.JsonSerializer.Serialize(request);
                    var response = await client.PostAsJsonAsync("http://host.docker.internal:5001/recensioneaddestratore/", request);
                    isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
                }
                if (!string.IsNullOrWhiteSpace(model.IdVeterinario))
                {
                    request.Testo = model.Testo;
                    request.IdVeterinario = model.IdVeterinario;
                    request.IdUtente = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    var stringContent = System.Text.Json.JsonSerializer.Serialize(request);
                    var response = await client.PostAsJsonAsync("http://host.docker.internal:5001/recensionevet/", request);
                    isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
                }
                else
                {
                    request.Testo = model.Testo;
                    request.IdPetsitter = model.IdPetsitter;
                    request.IdUtente = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    var stringContent = System.Text.Json.JsonSerializer.Serialize(request);
                    var response = await client.PostAsJsonAsync("http://host.docker.internal:5001/recensionepetsitter/", request);
                    isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
                }
                

                
            }
            catch (Exception ex)
            {
                isOk = false;
                errorMsg = "Inserimento recensione fallito";
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
        public async Task<IActionResult> EliminaAddestratore(long id)
        {
            var isOk = false;
            var errorMsg = "";
            try
            {

                var response = await client.DeleteAsync("http://host.docker.internal:5001/addestratorecinofilo/" + id);

                isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception ex)
            {
                isOk = false;
                errorMsg = "Eliminazione addestratore fallito";
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
        public async Task<IActionResult> EliminaRecensione(long id)
        {
            var isOk = false;
            var errorMsg = "";
            try
            {

                var response = await client.DeleteAsync("http://host.docker.internal:5001/recensione/" + id);

                isOk = response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception ex)
            {
                isOk = false;
                errorMsg = "Eliminazione recensione fallito";
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