using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Three_Sisters_Hotel.Models;

namespace Three_Sisters_Hotel.Pages.Bookings
{
    [Authorize(Roles = "Customers")]
    public class SearchModel : PageModel
    {
        private readonly Three_Sisters_Hotel.Data.ApplicationDbContext _context;

        public SearchModel(Three_Sisters_Hotel.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SearchRooms RoomsInput { get; set; }

        // List of different movies; for passing to Content file to display
        public IList<Room> DiffRooms { get; set; }

        public IActionResult OnGet()
        {
            // Get the options for the MovieGoer select list from the database
            // and save them in ViewData for passing to Content file
            ViewData["RoomList"] = new SelectList(_context.Room, "ID", "BedCount");
            ViewData["CheckinList"] = new SelectList(_context.Booking, "ID", "ChecnIn");
            ViewData["CheckoutList"] = new SelectList(_context.Booking, "ID", "CheckOut");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // prepare the parameters to be inserted into the query
            var bedcount = new SqliteParameter("bedcounts", RoomsInput.Beds);
            var Checkin = new SqliteParameter("Checkin", RoomsInput.Checkin);
            var Checkout = new SqliteParameter("Checkout", RoomsInput.Checkout);
            //ViewData["bed"] = RoomsInput.Beds;
            //ViewData["in"] = RoomsInput.Checkin;
            //ViewData["out"] = RoomsInput.Checkout;


            // Construct the query to get the movies watched by Moviegoer A but not Moviegoer B
            // Use placeholders as the parameters

            
            var diffRooms = _context.Room.FromSqlRaw("select [Room].* from [Room] inner join [Booking] on "
                             + "[Room].ID = [Booking].RoomID where [Room].BedCount = @bedcounts and "
                             + "[Room].ID not in (select [Room].ID from [Room] inner join [Booking] on "
                             + "[Room].ID = [Booking].RoomID where [Booking].ChecnIn < @Checkin and [Booking].CheckOut <  @Checkout)", bedcount, Checkin, Checkout);

            //var diffRooms = _context.Room.FromSqlRaw("select * from Room inner join Booking on Room.ID = Booking.RoomID where Room.BedCount = @bedcounts and Room.ID not in (select Room.ID from Room inner join Booking on Room.ID = Booking.RoomID where Booking.ChecnIn < @Checkin and Booking.CheckOut < @Checkout)", bedcount, Checkin, Checkout);
           // select* from Room inner join Booking on Room.ID = Booking.RoomID where Room.BedCount = 1
           //and Room.ID not in (select Room.ID from Room inner join Booking on Room.ID = Booking.RoomID where Booking.ChecnIn < 2021 - 02 - 01 and Booking.CheckOut < 2021 - 02 - 02)
            //bookingA.Checkin < bookingB.Checkout AND bookingB.Checkin < bookingA.Checkout

            //.Select(mo => new Movie { ID = mo.ID, Genre = mo.Genre, Price = mo.Price, ReleaseDate = mo.ReleaseDate, Title = mo.Title });

            // Run the query and save the results in DiffMovies for passing to content file
            DiffRooms = await diffRooms.ToListAsync();

            return Page();
        }
    }
}