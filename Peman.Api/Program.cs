using Peman.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/weather/stations", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WheaterStation()
       {
           Indsinop = index.ToString(),
           Nombre = DateTime.Now.ToString()
       }
       )
       .ToArray();
    return forecast;
})
.WithName("GetWeatherStations");

app.MapGet("/weather/stations/{id}", (string id) =>
{

    return new WheaterStation()
    {
        Indsinop = id.ToString(),
        Nombre = DateTime.Now.ToString()
    };

})
.WithName("GetWeatherStationById");


app.MapPost("/weather/stations", (WheaterStation station) =>
{

    return new WheaterStation()
    {
        Indsinop = Guid.NewGuid().ToString(),
        Nombre = DateTime.Now.ToString()
    };

})
.WithName("NewWeatherStation");

app.MapPut("/weather/stations/{id}", (string id, WheaterStation station) =>
{
    return new WheaterStation()
    {
        Indsinop = id.ToString(),
        Nombre = DateTime.Now.ToString()
    };

})
.WithName("SaveWeatherStationById");

app.MapDelete("/weather/stations/{id}", (string id) =>
{
    Results.Ok();
})
.WithName("DeleteWeatherStationById");

app.Run();