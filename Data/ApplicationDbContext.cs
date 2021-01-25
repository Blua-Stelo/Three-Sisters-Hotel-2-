using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Three_Sisters_Hotel.Models;

namespace Three_Sisters_Hotel.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Three_Sisters_Hotel.Models.Booking> Booking { get; set; }
        public DbSet<Three_Sisters_Hotel.Models.Customer> Customer { get; set; }
        public DbSet<Three_Sisters_Hotel.Models.Room> Room { get; set; }
    }
}
