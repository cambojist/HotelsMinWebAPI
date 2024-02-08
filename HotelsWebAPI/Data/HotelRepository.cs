using HotelsMinWebAPI.Data;

namespace HotelsWebAPI.Data;

public sealed class HotelRepository(HotelDb context) : IHotelRepository
{
    public async Task<List<Hotel>> GetHotelsAsync() => await context.Hotels.ToListAsync();

    public async Task<Hotel> GetHotelAsync(int id) => await context.Hotels.FindAsync(id);

    public async Task InsertHotelAsync(Hotel hotel) => await context.Hotels.AddAsync(hotel);

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

    public async Task SaveAsync() => await context.SaveChangesAsync();

    private bool _disposed;

    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) context.Dispose();
        _disposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}