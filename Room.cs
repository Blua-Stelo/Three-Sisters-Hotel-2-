using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Three_Sisters_Hotel.Models
{
    public class Room
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[G123]{1}", ErrorMessage = "Levels should be G, 1, 2 or 3!")]
        public string Level { get; set; }

        [Display(Name = "Bed Count")]
        [Range(1, 3, ErrorMessage = "Number of bed should between 1 and 3!")]
        public int BedCount { get; set; }

        [Display(Name = "Price per night")]
        [DataType(DataType.Currency)]
        [Range(50, 300, ErrorMessage = "Price should between $50 and $300 per night!")]
        public decimal Price { get; set; }

        public ICollection<Booking> TheBookings { get; set; }
    }
}
