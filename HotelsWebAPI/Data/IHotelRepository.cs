﻿using HotelsMinWebAPI.Data;

namespace HotelsWebAPI.Data;

public interface IHotelRepository : IDisposable
{
    Task<List<Hotel>> GetHotelsAsync();
    Task<List<Hotel>> GetHotelsAsync(Coordinate coordinate);
    Task<Hotel> GetHotelAsync(int id);
    Task InsertHotelAsync(Hotel hotel);
    Task UpdateHotelAsync(Hotel hotel);
    Task DeleteHotelAsync(int id);
    Task SaveAsync();
}