using HotelsMinWebAPI.Data;

namespace HotelsWebAPI.Data;

public sealed class HotelRepository(HotelDb context) : IHotelRepository
{
    private bool _disposed;

    public async Task<List<Hotel>> GetHotelsAsync()
    {
        return await context.Hotels.ToListAsync();
    }

    public async Task<Hotel> GetHotelAsync(int id)
    {
        return await context.Hotels.FindAsync(id);
    }

    public async Task<List<Hotel>> GetHotelsAsync(Coordinate coordinate)
    {
        return await context.Hotels.Where(hotel =>
            hotel.Latitude > coordinate.Latitude - 1 &&
            hotel.Latitude < coordinate.Latitude + 1 &&
            hotel.Longitude > coordinate.Longitude - 1 &&
            hotel.Longitude < coordinate.Longitude + 1
        ).ToListAsync();
    }

    public async Task InsertHotelAsync(Hotel hotel)
    {
        await context.Hotels.AddAsync(hotel);
    }

    public async Task UpdateHotelAsync(Hotel hotel)
    {
        var hotelFromDb = await context.Hotels.FindAsync(hotel.Id);
        if (hotelFromDb == null) return;
        hotelFromDb.Name = hotel.Name;
        hotelFromDb.Longitude = hotel.Longitude;
        hotelFromDb.Latitude = hotel.Latitude;
    }

    public async Task DeleteHotelAsync(int id)
    {
        var hotelFromDb = await context.Hotels.FindAsync(id);
        if (hotelFromDb == null) return;
        context.Hotels.Remove(hotelFromDb);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) context.Dispose();
        _disposed = true;
    }
}