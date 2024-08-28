using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Core.Entities
{
    public class ParentsSeat : ISeat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SeatStatus Status { get; set; } = SeatStatus.free;
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
