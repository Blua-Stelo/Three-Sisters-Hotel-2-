using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Three_Sisters_Hotel.Models;

namespace Three_Sisters_Hotel.Pages.Customers
{
    [Authorize(Roles = "Customers")]
    public class MyDetailsModel : PageModel
    {
        private readonly Three_Sisters_Hotel.Data.ApplicationDbContext _context;

        public MyDetailsModel(Three_Sisters_Hotel.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CustomerViewModel Myself { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Customer customer = await _context.Customer.FirstOrDefaultAsync(m => m.Email == _email);

            if (customer != null)
            {
                ViewData["ExistInDB"] = "true";
                Myself = new CustomerViewModel
                {
                    LastName = customer.LastName,
                    FirstName = customer.FirstName,
                    Postcode = customer.Postcode,
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
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Customer customer = await _context.Customer.FirstOrDefaultAsync(m => m.Email == _email);

            if (customer != null)
            {
                ViewData["ExisInDB"] = "true";
            }
            else
            {
                ViewData["ExitInDB"] = "false";
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (customer == null)
            {
                customer = new Customer();
            }

            customer.Email = _email;
            customer.LastName = Myself.LastName;
            customer.FirstName = Myself.FirstName;
            customer.Postcode = Myself.Postcode;

            if ((string)ViewData["ExistInDB"] == "true")
            {
                _context.Attach(customer).State = EntityState.Modified;
            }
            else
            {
                _context.Customer.Add(customer);
            }
            try
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