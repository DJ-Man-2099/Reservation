using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Core.Entities
{
    public class User : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NumberOfReservation { get; set; }
        public ICollection<ParentsSeat> ParentsSeats { get; set; } = new HashSet<ParentsSeat>();
        public ICollection<KidsSeat> KidsSeat { get; set; } = new HashSet<KidsSeat>();
    }
}
