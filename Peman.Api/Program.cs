using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peman.Api;
using Peman.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServer<PemanContexDb>(builder.Configuration.GetConnectionString("SqlConnectionString"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weather/stations", ([FromServices] PemanContexDb contexDb) =>
{
    var stations = contexDb.Stations.ToArray();
    if (stations.Any())
        return Results.Ok(stations);
    else
        return Results.NoContent();
})
.WithName("GetWeatherStations");

app.MapGet("/weather/stations/{id}", (string id, [FromServices] PemanContexDb contexDb) =>
{
    var station = contexDb.Stations.FirstOrDefault(x => x.Indicativo == id);
    if (station is null)
        return Results.NotFound();
    else
        return Results.Ok(station);
})
.WithName("GetWeatherStationById");


app.MapPost("/weather/stations", async (WheaterStation station, [FromServices] PemanContexDb contexDb) =>
{
    var actual = contexDb.Stations.FirstOrDefault(x => x.Indicativo == station.Indicativo);
    if (actual is null)
    {
        contexDb.Stations.Add(station);
        await contexDb.SaveChangesAsync();
        return Results.Created("/weather/stations", station);
    }
    else
        return Results.BadRequest($"An station with id {station.Indicativo} already exists.");
})
.WithName("NewWeatherStation");

app.MapPut("/weather/stations/{id}", async (string id, WheaterStationToSave station, [FromServices] PemanContexDb contexDb) =>
{
    var actual = await contexDb.Stations.FirstOrDefaultAsync(x => x.Indicativo == id);
    if (actual is null)
    {
        return Results.NotFound();
    }
    else
    {
        actual.Provincia = station.Provincia;
        actual.Latitud = station.Latitud;
        actual.Altitud = station.Altitud;
        actual.Longitud = station.Longitud;
        actual.Indsinop = station.Indsinop;
        actual.Nombre = station.Nombre;
        await contexDb.SaveChangesAsync();
        return Results.Ok(actual);
    }
})
.WithName("SaveWeatherStationById");

app.MapDelete("/weather/stations/{id}", async (string id, [FromServices] PemanContexDb contexDb) =>
{
    var station = contexDb.Stations.FirstOrDefault(x => x.Indicativo == id);
    if (station is null)
        return Results.NoContent();
    else
    {
        contexDb.Stations.Remove(station);
        await contexDb.SaveChangesAsync();
        return Results.Ok();
    }
})
.WithName("DeleteWeatherStationById");

app.Run();