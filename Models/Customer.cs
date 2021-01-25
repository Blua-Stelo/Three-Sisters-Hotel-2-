using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Three_Sisters_Hotel.Models
{
    public class Customer
    {
        [Key, Required]
        [DataType(DataType.EmailAddress)]
        // [EmailAddress], the one above implies this one
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"[A-Z][a-z'-]{1,19}")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        [RegularExpression(@"[A-Z][a-z'-]{1,19}")]
        public string LastName { get; set; }

        [NotMapped] // not mapping this property to database, but exist in memory
        public string FullName => $"{FirstName} {LastName}";

        [Required, Display(Name = "Post Code")]
        [RegularExpression(@"[0-9]{4}")]
        public string Postcode { get; set; }

        public ICollection<Booking> TheBookings { get; set; }

        
    }
}
