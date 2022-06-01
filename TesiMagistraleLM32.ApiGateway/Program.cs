using Ocelot.DependencyInjection;
using Ocelot.Middleware;

//IConfiguration configuration = new ConfigurationBuilder()
//                            .AddJsonFile("ocelot.json")
//                            .Build();

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOcelot(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//API

app.MapGet("/getapibreedslist", async () =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5002/getapibreedslist/");
    return jsonResponse;
})
.WithName("GetApiBreedsList");

app.MapGet("/getapibreedslistbyname/{value}", async (string value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5002/getapibreedslistbyname/" + value);
    return jsonResponse;
})
.WithName("GetApiBreedsListByBreedName");

app.MapGet("/getbreedimagebyname/{value}", async (string value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5002/getbreedimagebyname/" + value);
    return jsonResponse;
})
.WithName("GetBreedImageByBreedName");

app.MapGet("/getapicatbreedslist", async () =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5002/getapicatbreedslist/");
    return jsonResponse;
})
.WithName("GetApiCatBreedsList");

app.MapGet("/getcatapibreedslistbyname/{value}", async (string value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5002/getcatapibreedslistbyname/" + value);
    return jsonResponse;
})
.WithName("GetCatApiBreedsListByBreedName");

app.MapGet("/getcatbreedimagebyname/{value}", async (string value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5002/getcatbreedimagebyname/" + value);
    return jsonResponse;
})
.WithName("GetCatBreedImageByBreedName");

app.MapGet("/getcomuniitaliani", async () =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5002/getcomuniitaliani/");
    return jsonResponse;
})
.WithName("GetComuniItaliani");


//API SQL

app.MapGet("/addestratoricinofili", async () =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/addestratoricinofili/");
    return jsonResponse;
})
.WithName("GetAddestratoriCinofili");

app.MapGet("/addestratorecinofilo/{value}", async (string value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/addestratorecinofilo/" + value);
    return jsonResponse;
})
.WithName("GetAddestratoreCinofilo");

app.MapPut("/addestratorecinofilo", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PutAsJsonAsync("http://host.docker.internal:5003/addestratorecinofilo/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PutAddestratoreCinofilo");

app.MapPost("/addestratorecinofilo", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PostAsJsonAsync("http://host.docker.internal:5003/addestratorecinofilo/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PostAddestratoreCinofilo");


app.MapGet("/eventi", async () =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/eventi/");
    return jsonResponse;
})
.WithName("GetEventi");

app.MapPost("/evento", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PostAsJsonAsync("http://host.docker.internal:5003/evento/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PostEvento");

app.MapPost("/recensioneaddestratore", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PostAsJsonAsync("http://host.docker.internal:5003/recensioneaddestratore/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PostRecensioneAddestratore");

app.MapPost("/recensionepetsitter", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PostAsJsonAsync("http://host.docker.internal:5003/recensionepetsitter/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PostRecensionePetsitter");

app.MapGet("/recensioniaddestratore/{value}", async (string value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/recensioniaddestratore/" + value);
    return jsonResponse;
})
.WithName("GetRecensioniAddestratore");

app.MapGet("/recensionipetsitter/{value}", async (string value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/recensionipetsitter/" + value);
    return jsonResponse;
})
.WithName("GetRecensioniPetsitter");

app.MapDelete("/recensione/{value}", async (long value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.DeleteAsync("http://host.docker.internal:5003/recensione/" + value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("DeleteRecensione");

app.MapGet("/petsitter", async () =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/petsitter/");
    return jsonResponse;
})
.WithName("GetPetsitter");

app.MapPost("/postpetsitter", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PostAsJsonAsync("http://host.docker.internal:5003/postpetsitter/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PostPetsitter");

app.MapPut("/putpetsitter", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PutAsJsonAsync("http://host.docker.internal:5003/putpetsitter/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PutPetsitter");

app.MapGet("/petsitter/{value}", async (string value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/petsitter/" + value);
    return jsonResponse;
})
.WithName("GetSPetSitter");

app.MapPost("/postveterinario", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PostAsJsonAsync("http://host.docker.internal:5003/postveterinario/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PostVeterinario");

app.MapGet("/veterinari", async () =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/veterinari/");
    return jsonResponse;
})
.WithName("GetVeterinari");

app.MapGet("/veterinario/{value}", async (string value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/veterinario/" + value);
    return jsonResponse;
})
.WithName("GetVeterinario");

app.MapPut("/putveterinario", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PutAsJsonAsync("http://host.docker.internal:5003/putveterinario/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PutVeterinario");

app.MapPost("/recensionevet", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PostAsJsonAsync("http://host.docker.internal:5003/recensionevet/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PostRecensioneVet");

app.MapGet("/recensionivet/{value}", async (string value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/recensionivet/" + value);
    return jsonResponse;
})
.WithName("GetRecensioniVet");

app.MapPost("/domanda", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PostAsJsonAsync("http://host.docker.internal:5003/domanda/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PostDomanda");

app.MapGet("/domande/{value}", async (string value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/domande/" + value);
    return jsonResponse;
})
.WithName("GetDomande");

app.MapDelete("/forum/{value}", async (long value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.DeleteAsync("http://host.docker.internal:5003/forum/" + value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("DeleteForum");

app.MapPost("/commento", async (Object value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.PostAsJsonAsync("http://host.docker.internal:5003/commento/", value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("PostCommento");

app.MapGet("/commenti/{value}", async (long value) =>
{
    var client = new HttpClient();
    var jsonResponse = await client.GetStringAsync("http://host.docker.internal:5003/commenti/" + value);
    return jsonResponse;
})
.WithName("GetCommenti");

app.MapDelete("/commento/{value}", async (long value) =>
{
    var client = new HttpClient();
    var jsonResponse = (await client.DeleteAsync("http://host.docker.internal:5003/commento/" + value)).EnsureSuccessStatusCode();
    return jsonResponse;
})
.WithName("DeleteCommento");

app.Run();