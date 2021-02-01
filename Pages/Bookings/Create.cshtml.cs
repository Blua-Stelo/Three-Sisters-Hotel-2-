using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Three_Sisters_Hotel.Data;
using Three_Sisters_Hotel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.Sqlite;

namespace Three_Sisters_Hotel.Pages.Bookings
{
    [Authorize(Roles = "Customers")]
    public class CreateModel : PageModel
    {

        private readonly Three_Sisters_Hotel.Data.ApplicationDbContext _context;

        public CreateModel(Three_Sisters_Hotel.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CustomerEmail"] = new SelectList(_context.Set<Customer>(), "Email", "Email");
        ViewData["RoomID"] = new SelectList(_context.Set<Room>(), "ID", "ID");
            return Page();
        }

        [BindProperty]
        public Booking Bookings { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Booking bookings = await _context.Booking.FirstOrDefaultAsync(m => m.CustomerEmail == _email);

            

            if (!ModelState.IsValid)
            {
                ViewData["CustomerEmail"] = new SelectList(_context.Set<Customer>(), "Email", "Email");
                ViewData["RoomID"] = new SelectList(_context.Set<Room>(), "ID", "ID");
                return Page();
            }

            bookings = new Booking();
            bookings.RoomID = Bookings.RoomID;
            bookings.CustomerEmail = _email;
            bookings.ChecnIn = Bookings.ChecnIn;
            bookings.CheckOut = Bookings.CheckOut;
            var totalday = (bookings.CheckOut - bookings.ChecnIn).Days;
            // retrieve the pizza to get its price
            var theRoom = await _context.Room.FindAsync(Bookings.RoomID);
            // calculate the total price of this order
            bookings.Cost = (int)totalday * theRoom.Price;

            var Roomid = new SqliteParameter("Roomid", Bookings.RoomID);
            var Checkin = new SqliteParameter("Checkin", Bookings.ChecnIn);
            var Checkout = new SqliteParameter("Checkout", Bookings.CheckOut);

            var diffRooms = _context.Room.FromSqlRaw("select [Room].* from [Room] inner join [Booking] on "
                             + "[Booking].RoomID = @Roomid where [Booking].ChecnIn < @Checkin and [Booking].CheckOut <  @Checkout ", Roomid, Checkin, Checkout);
            ViewData["rooms"] = diffRooms;

            _context.Booking.Add(bookings);
            await _context.SaveChangesAsync();

            ViewData["Room"] = theRoom.ID;
            ViewData["total"] = bookings.Cost;
            ViewData["SuccessDB"] = "success";
            /*
            if (diffRooms == null)
            {
                _context.Booking.Add(bookings);
                await _context.SaveChangesAsync();

                ViewData["Room"] = theRoom.ID;
                ViewData["total"] = bookings.Cost;
                ViewData["SuccessDB"] = "success";
            }
            else
            {
                ViewData["SuccessDB"] = "fail";
                return Page();
            }
            */

            return Page();
        }
    }
}
