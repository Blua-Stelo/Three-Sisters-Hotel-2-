using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Three_Sisters_Hotel.Models
{
    public class SearchRooms
    {
       [Required]
       public int Beds { get; set; }
       [Required]
       public Booking Booking { get; set; }
    }
}
