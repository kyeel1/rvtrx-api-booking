using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RVTR.Booking.DataContext;
using RVTR.Booking.DataContext.Repositories;
using RVTR.Booking.ObjectModel.Models;
using Xunit;

namespace RVTR.Booking.UnitTesting.Tests
{
  public class BookingRepositoryTest
  {
    private static readonly SqliteConnection _connection = new SqliteConnection("Data Source=:memory:");
    private static readonly DbContextOptions<BookingContext> _options = new DbContextOptionsBuilder<BookingContext>().UseSqlite(_connection).Options;

    public static readonly IEnumerable<object[]> _records = new List<object[]>()
    {
      new object[]
      {
        new BookingModel() {
          Id = 1,
          AccountId = 1,
          LodgingId = 2,
          CheckIn = new DateTime(2020, 8, 16),
          Checkout = new DateTime(2010, 8, 18)
        }

      }
    };


    [Theory]
    [MemberData(nameof(_records))]
    public async void Test_Repository_GetBookingsAsync_ByDate(BookingModel booking)
    {
      //DateTime bookingCheckIn = DateTime.Parse(checkIn);
      //DateTime bookingCheckOut = DateTime.Parse(checkOut);
      await _connection.OpenAsync();

      try
      {
        using (var ctx = new BookingContext(_options))
        {
          await ctx.Database.EnsureCreatedAsync();
          await ctx.Bookings.AddAsync(booking);
        }

        using (var ctx = new BookingContext(_options))
        {
          var bookings = new BookingRepository(ctx);

          var actual = await bookings.SelectAsync();

          Assert.NotNull(actual);
        }

      }
      finally
      {
        _connection.Close();
      }
    }


  }
}
