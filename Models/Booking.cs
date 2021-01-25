using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Three_Sisters_Hotel.Models
{
    public class Booking
    {
        [Key]
        public int ID { get; set; }

        public int RoomID { get; set; }

        public string CustomerEmail { get; set; }

        [DataType(DataType.Date)]
        public DateTime ChecnIn { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        public Decimal Cost { get; set; }

        public Room TheRoom { get; set; }

        public Customer TheCustomer { get; set; }
    }
}
