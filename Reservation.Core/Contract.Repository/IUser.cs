using Reservation.Core.Entities;

public interface IUser
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Phone { get; set; }
	public string Email { get; set; }
	public int NumberOfReservation { get; set; }
	public ICollection<ParentsSeat> ParentsSeats { get; set; }
	public ICollection<KidsSeat> KidsSeat { get; set; }
}
