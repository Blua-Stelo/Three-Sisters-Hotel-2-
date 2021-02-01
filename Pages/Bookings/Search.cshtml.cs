using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Three_Sisters_Hotel.Models;

namespace Three_Sisters_Hotel.Pages.Bookings
{
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
        public IList<Booking> DiffBooking { get; set; }

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
            var bookingA = new SqliteParameter("bookingA", RoomsInput.Booking);

            // Construct the query to get the movies watched by Moviegoer A but not Moviegoer B
            // Use placeholders as the parameters
            
            
            var diffBooking = _context.Booking.FromSqlRaw("select [Room].* from [Room] inner join [Booking] on "
                             + "[Room].ID = [Booking].RoomID where [Room].BedCount = @bedcount and "
                             +""
                             + "[Room].ID not in (select [Room].ID from [Room] inner join [Booking] on "
                             + "[Room].ID = [Booking].RoomID where [Booking].MovieGoerEmail = @personB)",bedcount, bookingA);

            bookingA.Checkin < bookingB.Checkout AND bookingB.Checkin < bookingA.Checkout

            //.Select(mo => new Movie { ID = mo.ID, Genre = mo.Genre, Price = mo.Price, ReleaseDate = mo.ReleaseDate, Title = mo.Title });

            // Run the query and save the results in DiffMovies for passing to content file
            //DiffBooking = await diffBooking.ToListAsync();
            return Page();
        }
    }
}