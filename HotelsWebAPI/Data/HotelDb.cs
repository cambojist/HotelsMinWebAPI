using HotelsMinWebAPI.Data;

namespace HotelsWebAPI.Data;

public class HotelDb(DbContextOptions<HotelDb> options) : DbContext(options)
{
    public DbSet<Hotel> Hotels => Set<Hotel>();
} 