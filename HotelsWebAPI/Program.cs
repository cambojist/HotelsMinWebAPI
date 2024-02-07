using HotelsMinWebAPI.Data;
using HotelsWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HotelDb>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<HotelDb>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();

#region Test CRUD code
app.MapGet("/hotels", async (HotelDb db) => 
    await db.Hotels.ToListAsync());

app.MapGet("/hotels/{id:int}", async (int id, HotelDb db) =>
    await db.Hotels.FirstOrDefaultAsync(h => h.Id == id) is { } hotel 
    ? Results.Ok(hotel)
    : Results.NotFound());

app.MapPost("/hotels", async ([FromBody] Hotel hotel, HotelDb db) =>
{
    db.Hotels.Add(hotel);
    await db.SaveChangesAsync();
    return Results.Created($"/hotels/{hotel.Id}", hotel);
});

app.MapPut("/hotels", async ([FromBody] Hotel hotel, HotelDb db) =>
{
    var hotelFromDb = await db.Hotels.FindAsync(hotel.Id);
    if (hotelFromDb == null) return Results.NotFound();
    hotelFromDb.Longitude = hotel.Longitude;
    hotelFromDb.Latitude = hotel.Latitude;
    hotelFromDb.Name = hotel.Name;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/hotels/{id:int}", async (int id, HotelDb db) =>
{
    var hotelFromDb = await db.Hotels.FindAsync(id);
    if (hotelFromDb == null) return Results.NotFound();
    db.Hotels.Remove(hotelFromDb);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
#endregion

app.Run();
