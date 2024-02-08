using HotelsMinWebAPI.Data;
using HotelsWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HotelDb>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});
builder.Services.AddScoped<IHotelRepository, HotelRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<HotelDb>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();

#region Test CRUD code

app.MapGet("/hotels", async (IHotelRepository repository) =>
        Results.Ok(await repository.GetHotelsAsync()))
    .Produces<List<Hotel>>()
    .WithName("GetAllHotels")
    .WithTags("GET");

app.MapGet("/hotels/{id:int}", async (int id, IHotelRepository repository) =>
        await repository.GetHotelAsync(id) is { } hotel
            ? Results.Ok(hotel)
            : Results.NotFound())
    .Produces<Hotel>()
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetHotel")
    .WithTags("GET");
;

app.MapPost("/hotels", async ([FromBody] Hotel hotel, IHotelRepository repository) =>
    {
        await repository.InsertHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.Created($"/hotels/{hotel.Id}", hotel);
    })
    .Accepts<Hotel>("application/json")
    .Produces<Hotel>(StatusCodes.Status201Created)
    .WithName("CreateHotel")
    .WithTags("POST");

app.MapPut("/hotels", async ([FromBody] Hotel hotel, IHotelRepository repository) =>
    {
        await repository.UpdateHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.NoContent();
    })
    .Accepts<Hotel>("application/json")
    .WithName("UpdateHotel")
    .WithTags("PUT");

app.MapDelete("/hotels/{id:int}", async (int id, IHotelRepository repository) =>
    {
        await repository.DeleteHotelAsync(id);
        await repository.SaveAsync();
        return Results.NoContent();
    })
    .WithName("DeleteHotel")
    .WithTags("DELETE");

#endregion

app.Run();