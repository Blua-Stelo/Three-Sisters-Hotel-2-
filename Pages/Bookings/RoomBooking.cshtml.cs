using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Three_Sisters_Hotel.Models;

namespace Three_Sisters_Hotel.Pages.Bookings
{
    [Authorize(Roles = "Customers")]
    public class RoomBookingModel : PageModel
    {
        private readonly Three_Sisters_Hotel.Data.ApplicationDbContext _context;
        public RoomBookingModel(Three_Sisters_Hotel.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BookingView Myself { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // retrieve the logged-in user's email
            // need to add "using System.Security.Claims;"
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Booking booking = await _context.Booking.FirstOrDefaultAsync(m => m.CustomerEmail == _email);

            if (booking != null)
            {
                // The user has been created in the database
                ViewData["ExistInDB"] = "true";
                Myself = new BookingView
                {
                    // Retrieve his/her details for display in the web form
                    RoomID = booking.RoomID,
                    ChecnIn = booking.ChecnIn,
                    CheckOut = booking.CheckOut,
                };
            }
            else
            {
                ViewData["ExistInDB"] = "false";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // retrieve the logged-in user's email
            // need to add "using System.Security.Claims;"
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Booking booking = await _context.Booking.FirstOrDefaultAsync(m => m.CustomerEmail == _email);

            if (booking != null)
            {
                // This ViewData entry is needed in the content file
                // The user has been created in the database
                ViewData["ExistInDB"] = "true";
            }
            else
            {
                ViewData["ExistInDB"] = "false";
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (booking == null)
            {

                booking = new Booking();
            }

        
            booking.CustomerEmail = _email;
            booking.RoomID = Myself.RoomID;
            booking.ChecnIn = Myself.ChecnIn;
            booking.CheckOut = Myself.CheckOut;

            if ((string)ViewData["ExistInDB"] == "true")
            {
                _context.Attach(booking).State = EntityState.Modified;
            }
            else
            {
                _context.Booking.Add(booking);
            }

            try  // catching the conflict of editing this record concurrently
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            ViewData["SuccessDB"] = "success";
            return Page();
        }
    }
}
