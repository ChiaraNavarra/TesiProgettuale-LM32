var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/getapibreedslist", async () =>
{
    var client = new HttpClient();
    var request = new HttpRequestMessage
    {
        Method = HttpMethod.Get,
        RequestUri = new Uri("https://api.thedogapi.com/v1/breeds")
    };
    using (var response = await client.SendAsync(request))
    {
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }
})
    .WithName("GetApiBreedsList");

app.MapGet("/getapibreedslistbyname/{value}", async (string value) =>
{
    var client = new HttpClient();
    var request = new HttpRequestMessage
    {
        Method = HttpMethod.Get,
        RequestUri = new Uri("https://api.thedogapi.com/v1/breeds/search?q=" + value),
    };
    using (var response = await client.SendAsync(request))
    {
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }
})
    .WithName("GetApiBreedsListByBreedName");

app.MapGet("/getbreedimagebyname/{value}", async (string value) =>
{
    var client = new HttpClient();
    var request = new HttpRequestMessage
    {
        Method = HttpMethod.Get,
        RequestUri = new Uri("https://api.thedogapi.com/v1/images/" + value),
    };
    using (var response = await client.SendAsync(request))
    {
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }
})
    .WithName("GetBreedImageByBreedName");

app.MapGet("/getapicatbreedslist", async () =>
{
    var client = new HttpClient();
    var request = new HttpRequestMessage
    {
        Method = HttpMethod.Get,
        RequestUri = new Uri("https://api.thecatapi.com/v1/breeds")
    };
    using (var response = await client.SendAsync(request))
    {
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }
})
    .WithName("GetApiCatBreedsList");

app.MapGet("/getcatapibreedslistbyname/{value}", async (string value) =>
{
    var client = new HttpClient();
    var request = new HttpRequestMessage
    {
        Method = HttpMethod.Get,
        RequestUri = new Uri("https://api.thecatapi.com/v1/breeds/search?q=" + value),
    };
    using (var response = await client.SendAsync(request))
    {
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }
})
    .WithName("GetCatApiBreedsListByBreedName");

app.MapGet("/getcatbreedimagebyname/{value}", async (string value) =>
{
    var client = new HttpClient();
    var request = new HttpRequestMessage
    {
        Method = HttpMethod.Get,
        RequestUri = new Uri("https://api.thecatapi.com/v1/images/" + value),
    };
    using (var response = await client.SendAsync(request))
    {
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }
})
    .WithName("GetCatBreedImageByBreedName");

app.MapGet("/getcomuniitaliani", async () =>
{
    var client = new HttpClient();
    var request = new HttpRequestMessage
    {
        Method = HttpMethod.Get,
        RequestUri = new Uri("https://comuni-ita.herokuapp.com/api/comuni")
    };
    using (var response = await client.SendAsync(request))
    {
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }
})
    .WithName("GetComuniItaliani");

app.Run();