namespace Reservation.DTOs
{
    public class ReserveSeatDto
    {
        public List<int>? ParentsSeatsId { get; set; }
        public List<int> KidsSeatsId { get; set; }
    }

}
