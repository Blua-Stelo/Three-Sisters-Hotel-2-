using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Three_Sisters_Hotel.Models
{
    public class BookingView
    {
        [Range(1,16)]
        public int RoomID { get; set; }

        [DataType(DataType.Date)]
        public DateTime ChecnIn { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }
    }
}
