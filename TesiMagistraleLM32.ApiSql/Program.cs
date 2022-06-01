//using Newtonsoft.Json;
using TesiMagistraleLM32.ApiSql.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("connectionstring");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapGet("/addestratoricinofili", async () =>
{
    var query = @"SELECT addestr.Id, addestr.Nome, addestr.Cognome, addestr.NumeroTelefono, addestr.Comune, addestr.Note, addestr.Indirizzo, users.Email
                        FROM [dbo].[AddestratoreCinofilo] addestr
                        LEFT JOIN dbo.AspNetUsers users on users.Id = addestr.Id
                       LEFT JOIN dbo.AspNetUserRoles us on us.UserId = addestr.Id
                        LEFT JOIN dbo.AspNetRoles rol on rol.Id = us.RoleId
                        where rol.Name = 'PetTrainer'";

    var lista = new List<AddestratoreCinofiloModel>();
    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<AddestratoreCinofiloModel>(query)).ToList();
    }
    return lista;

}).WithName("GetAddestratoriCinofili");

app.MapPost("/addestratorecinofilo", async (Object content) =>
{
    var person = JsonSerializer.Deserialize<AddestratoreCinofiloModel>(content.ToString(), new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
    var query = $"INSERT INTO [dbo].[AddestratoreCinofilo] (Id,Nome,Cognome,NumeroTelefono,Note,Comune,Indirizzo) VALUES('{person.Id}', '{person.Nome}', '{person.Cognome}', '{person.NumeroTelefono}', '{person.Note}', '{person.Comune}', , '{person.Indirizzo}');";

    using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync(query, null);
    }
    Results.Ok();
})
.WithName("PostAddestratoreCinofilo");


app.MapPut("/addestratorecinofilo", async (Object content) =>
{
    var person = JsonSerializer.Deserialize<AddestratoreCinofiloModel>(content.ToString(), new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
    var query = $"UPDATE [dbo].[AddestratoreCinofilo] SET NumeroTelefono = '{person.NumeroTelefono}', Note = '{person.Note}', Comune = '{person.Comune}', Indirizzo = '{person.Indirizzo}' WHERE Id = '{person.Id}'";

    using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync(query, null);
    }
    Results.Ok();
})
.WithName("PutAddestratoreCinofilo");

app.MapGet("/addestratorecinofilo/{idUtente}", async (string idUtente) =>
{
    var query = $"SELECT * FROM [dbo].[AddestratoreCinofilo] where Id = '{idUtente}'";
    var lista = new List<AddestratoreCinofiloModel>();

    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<AddestratoreCinofiloModel>(query)).ToList();
    }
    return lista;
})
.WithName("GetAddestratoreCinofilo");

app.MapGet("/eventi", async () =>
{

    var query = "SELECT * FROM [dbo].[Evento] ORDER BY DataEvento desc";
    var lista = new List<EventoModel>();
    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<EventoModel>(query)).ToList();
    }
    return lista;
})
.WithName("GetEventi");

app.MapPost("/evento", async (Object content) =>
{
    var evento = JsonSerializer.Deserialize<EventoModel>(content.ToString(), new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });
    var query = $"INSERT INTO [dbo].[Evento] (Titolo,DataEvento,Comune,Descrizione,TipoEvento) VALUES('{evento.Titolo}'," +
    $" '{evento.DataEvento}', '{evento.Comune}', '{evento.Descrizione}', '{evento.TipoEvento}');";

    using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync(query, null);
    }
    Results.Ok();
})
.WithName("PostEvento");

app.MapPost("/recensioneaddestratore", async (Object content) =>
{
    var person = JsonSerializer.Deserialize<RecensioneModel>(content.ToString(), new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
    var query = $"INSERT INTO [dbo].[Recensione] (Testo, IdUtente) VALUES('{person.Testo}', '{person.IdUtente}'); SELECT SCOPE_IDENTITY(); ";

    using (var conn = new SqlConnection(connectionString))
    {
        var idRecensione = await conn.QuerySingleAsync<long>(query, null);

        var query2 = $"INSERT INTO [dbo].[RelAddestratoreRecensione] (IdAddestratore,IdRecensione) VALUES('{person.IdAddestratore}', '{idRecensione}'); ";
        await conn.ExecuteAsync(query2, null);
    }
    Results.Ok();

})
.WithName("PostRecensioneAddestratore");

app.MapPost("/recensionepetsitter", async (Object content) =>
{
    var person = JsonSerializer.Deserialize<RecensioneModel>(content.ToString(), new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
    var query = $"INSERT INTO [dbo].[Recensione] (Testo, IdUtente) VALUES('{person.Testo}', '{person.IdUtente}'); SELECT SCOPE_IDENTITY(); ";

    using (var conn = new SqlConnection(connectionString))
    {

        var idRecensione = await conn.QuerySingleAsync<long>(query, null);
        var query2 = $"INSERT INTO [dbo].[RelPetsitterRecensione] (IdPetsitter,IdRecensione) VALUES('{person.IdPetsitter}', '{idRecensione}'); ";
        await conn.ExecuteAsync(query2, null);
    }
    Results.Ok();
})
.WithName("PostRecensionePetsitter");

app.MapDelete("/recensione/{id}", async (long id) =>
{
    var query = $"DELETE FROM [dbo].[Recensione] WHERE Id = {id}; DELETE FROM [dbo].[RelAddestratoreRecensione] WHERE IdRecensione = {id}; DELETE FROM [dbo].[RelPetsitterRecensione] WHERE IdRecensione = {id};  DELETE FROM [dbo].[RelVeterinarioRecensione] WHERE IdRecensione = {id};";
    using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync(query, null);
    }
    Results.Ok();
})
.WithName("DeleteRecensione");

app.MapGet("/recensioniaddestratore/{value}", async (string value) =>
{
    var query = @$"SELECT r.Id, r.Testo, u.UserName as UsernameUtente
                        FROM [dbo].[Recensione] r
                        LEFT JOIN [dbo].[RelAddestratoreRecensione] rel on rel.IdRecensione = r.Id
                        LEFT JOIN [dbo].AspNetUsers u on r.IdUtente = u.Id
                        WHERE rel.IdAddestratore = '{value}'";
    var lista = new List<RecensioneModel>();
    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<RecensioneModel>(query)).ToList();
    }
    return lista;
})
.WithName("GetRecensioniAddestratore");

app.MapGet("/recensionipetsitter/{value}", async (string value) =>
{
    var query = @$"SELECT r.Id, r.Testo, u.UserName as UsernameUtente
                        FROM [dbo].[Recensione] r
                        LEFT JOIN [dbo].[RelPetsitterRecensione] rel on rel.IdRecensione = r.Id
                        LEFT JOIN [dbo].AspNetUsers u on r.IdUtente = u.Id
                        WHERE rel.IdPetsitter = '{value}'";
    var lista = new List<RecensioneModel>();
    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<RecensioneModel>(query)).ToList();
    }
    return lista;
})
.WithName("GetRecensioniPetsitter");


app.MapGet("/petsitter", async () =>
{
    var query = @"SELECT addestr.Id, addestr.Nome, addestr.Cognome, addestr.NumeroTelefono, addestr.Comune, addestr.Note, users.Email
                        FROM [dbo].[Petsitter] addestr
                        LEFT JOIN dbo.AspNetUsers users on users.Id = addestr.Id
                        LEFT JOIN dbo.AspNetUserRoles us on us.UserId = addestr.Id
                        LEFT JOIN dbo.AspNetRoles rol on rol.Id = us.RoleId
                        where rol.Name = 'PetSitter'";
    var lista = new List<PetsitterModel>();
    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<PetsitterModel>(query)).ToList();
    }
    return lista;
})
.WithName("GetPetsitter");

app.MapPost("/postpetsitter", async (Object content) =>
{
    var person = JsonSerializer.Deserialize<PetsitterModel>(content.ToString(), new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
    var query = $"INSERT INTO [dbo].[Petsitter] (Id,Nome,Cognome,NumeroTelefono,Note,Comune) VALUES('{person.Id}', '{person.Nome}', '{person.Cognome}', '{person.NumeroTelefono}', '{person.Note}', '{person.Comune}');";

    using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync(query, null);
    }
    Results.Ok();
})
.WithName("PostPetsitter");

app.MapPut("/putpetsitter", async (Object content) =>
{
    var person = JsonSerializer.Deserialize<PetsitterModel>(content.ToString(), new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
    var query = $"UPDATE [dbo].[Petsitter] SET NumeroTelefono = '{person.NumeroTelefono}', Note = '{person.Note}', Comune = '{person.Comune}' WHERE Id = '{person.Id}'";
    using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync(query, null);
    }
    Results.Ok();
})
.WithName("PutPetsitter");

app.MapGet("/petsitter/{idUtente}", async (string idUtente) =>
{
    string query = $"SELECT * FROM [dbo].[Petsitter] where Id = '{idUtente}'";
    var lista = new List<PetsitterModel>();
    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<PetsitterModel>(query)).ToList();
    }
    return lista;
})
.WithName("GetSPetSitter");

app.MapPost("/postveterinario", async (Object content) =>
{
    var person = JsonSerializer.Deserialize<VeterinarioModel>(content.ToString(), new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
    var query = $"INSERT INTO [dbo].[Veterinario] (Id,Nome,Cognome,NumeroTelefono,Note,Comune,TipoVeterinario,Indirizzo) VALUES('{person.Id}', '{person.Nome}', '{person.Cognome}', '{person.NumeroTelefono}', '{person.Note}', '{person.Comune}', '{person.TipoVeterinario}', '{person.Indirizzo}');";
    using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync(query, null);
    }
    Results.Ok();
})
.WithName("PostVeterinario");

app.MapGet("/veterinari", async () =>
{
    string query = @"SELECT addestr.Id, addestr.Nome, addestr.Cognome, addestr.NumeroTelefono, addestr.Comune, addestr.Note, addestr.TipoVeterinario, addestr.Indirizzo, users.Email
                        FROM [dbo].[Veterinario] addestr
                        LEFT JOIN dbo.AspNetUsers users on users.Id = addestr.Id
                        LEFT JOIN dbo.AspNetUserRoles us on us.UserId = addestr.Id
                        LEFT JOIN dbo.AspNetRoles rol on rol.Id = us.RoleId
                        where rol.Name = 'Veterinario'";
    var lista = new List<VeterinarioModel>();
    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<VeterinarioModel>(query)).ToList();
    }
    return lista;
})
.WithName("GetVeterinari");

app.MapGet("/veterinario/{idUtente}", async (string idUtente) =>
{
    string query = $"SELECT * FROM [dbo].[Veterinario] where Id = '{idUtente}'";
    var lista = new List<VeterinarioModel>();
    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<VeterinarioModel>(query)).ToList();
    }
    return lista;
})
.WithName("GetVeterinario");

app.MapPut("/putveterinario", async (Object content) =>
{
    var person = JsonSerializer.Deserialize<VeterinarioModel>(content.ToString(), new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
    var query = $"UPDATE [dbo].[Veterinario] SET NumeroTelefono = '{person.NumeroTelefono}', Note = '{person.Note}', Comune = '{person.Comune}', TipoVeterinario = '{person.TipoVeterinario}', Indirizzo = '{person.Indirizzo}' WHERE Id = '{person.Id}'";
    using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync(query, null);
    }
    Results.Ok();
})
.WithName("PutVeterinario");


app.MapPost("/recensionevet", async (Object content) =>
{
    var person = JsonSerializer.Deserialize<RecensioneModel>(content.ToString(), new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
    var query = $"INSERT INTO [dbo].[Recensione] (Testo, IdUtente) VALUES('{person.Testo}', '{person.IdUtente}'); SELECT SCOPE_IDENTITY(); ";

    using (var conn = new SqlConnection(connectionString))
    {

        var idRecensione = await conn.QuerySingleAsync<long>(query, null);

        var query2 = $"INSERT INTO [dbo].[RelVeterinarioRecensione] (IdVeterinario,IdRecensione) VALUES('{person.IdVeterinario}', '{idRecensione}'); ";
        await conn.ExecuteAsync(query2, null);
    }
    Results.Ok();
})
.WithName("PostRecensioneVet");

app.MapGet("/recensionivet/{value}", async (string value) =>
{
    var query = @$"SELECT r.Id, r.Testo, u.UserName as UsernameUtente
                        FROM [dbo].[Recensione] r
                        LEFT JOIN [dbo].[RelVeterinarioRecensione] rel on rel.IdRecensione = r.Id
                        LEFT JOIN [dbo].AspNetUsers u on r.IdUtente = u.Id
                        WHERE rel.IdVeterinario = '{value}'";
    var lista = new List<RecensioneModel>();
    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<RecensioneModel>(query)).ToList();
    }
    return lista;
})
.WithName("GetRecensioniVet");

app.MapPost("/domanda", async (Object content) =>
{
    var person = JsonSerializer.Deserialize<ForumModel>(content.ToString(), new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });
    
    var query = $"INSERT INTO [dbo].[Forum] (TipoForum,Testo,IdUtente,DataInizio,Titolo) VALUES('{person.TipoForum}', '{person.Testo}', '{person.IdUtente}', '{person.DataInizio}', '{person.Titolo}');";
    using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync(query, null);
    }
    Results.Ok();
})
.WithName("PostDomanda");

app.MapGet("/domande/{value}", async (string value) =>
{
    var query = "SELECT f.Id, f.TipoForum, f.Testo, f.DataInizio, f.Titolo, us.UserName as UsernameUtente FROM [dbo].[Forum] f LEFT JOIN [dbo].[AspNetUsers] us on us.Id = f.IdUtente WHERE f.Testo LIKE '%" + value + "%' or f.Titolo LIKE '%" + value + "%'  ORDER BY f.DataInizio desc";
    var lista = new List<ForumModel>();
    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<ForumModel>(query)).ToList();
    }
    return lista;
})
.WithName("GetDomande");

app.MapDelete("/forum/{id}", async (long id) =>
{
    var query = $"DELETE FROM [dbo].[Forum] WHERE Id = {id}; DELETE FROM [dbo].[RelForumCommento] WHERE IdForum = {id};";
    using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync(query, null);
    }
    Results.Ok();
})
.WithName("DeleteForum");

app.MapPost("/commento", async (Object content) =>
{
    var person = JsonSerializer.Deserialize<CommentoModel>(content.ToString(), new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
    var query = $"INSERT INTO [dbo].[Commento] (IdUtente, Testo, DataInizio) VALUES('{person.IdUtente}', '{person.Testo}', '{person.DataInizio}'); SELECT SCOPE_IDENTITY(); ";

    using (var conn = new SqlConnection(connectionString))
    {
        var idCommento = await conn.QuerySingleAsync<long>(query, null);
        string query2 = $"INSERT INTO [dbo].[RelForumCommento] (IdForum,IdCommento) VALUES('{person.IdForum}', '{idCommento}'); ";
        await conn.ExecuteAsync(query2, null);
    }
    Results.Ok();

})
.WithName("PostCommento");

app.MapGet("/commenti/{value}", async (long value) =>
{
    var query = @$"SELECT r.Id, r.Testo, r.DataInizio, u.UserName as UsernameUtente
                        FROM [dbo].[Commento] r
                        LEFT JOIN [dbo].RelForumCommento rel on rel.IdCommento = r.Id
                        LEFT JOIN [dbo].AspNetUsers u on r.IdUtente = u.Id
                        WHERE rel.IdForum = '{value}'";
    var lista = new List<CommentoModel>();
    using (var conn = new SqlConnection(connectionString))
    {
        lista = (await conn.QueryAsync<CommentoModel>(query)).ToList();
    }
    return lista;
})
.WithName("GetCommenti");

app.MapDelete("/commento/{id}", async (long id) =>
{
    var query = $"DELETE FROM [dbo].[Commento] WHERE Id = {id}; DELETE FROM [dbo].[RelForumCommento] WHERE IdCommento = {id};";
    using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync(query, null);
    }
    Results.Ok();
})
.WithName("DeleteCommento");

app.Run();

