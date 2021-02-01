using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Three_Sisters_Hotel.Data;
using Three_Sisters_Hotel.Models;
using Microsoft.AspNetCore.Authorization;

namespace Three_Sisters_Hotel.Pages.Bookings
{
    [Authorize(Roles = "Customers")]
    public class IndexModel : PageModel
    {
        private readonly Three_Sisters_Hotel.Data.ApplicationDbContext _context;

        public IndexModel(Three_Sisters_Hotel.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; }

        public async Task OnGetAsync()
        {
            var booking = (IQueryable<Booking>)_context.Booking.Include(b => b.TheRoom).Include(b => b.TheCustomer);

            Booking = await booking.AsNoTracking().ToListAsync();


             /*
            Booking = await _context.Booking
                .Include(b => b.TheCustomer)
                .Include(b => b.TheRoom).ToListAsync();
             */
        }
    }
}
