using Reservation.Core.Entities;

public interface ISeat
{
	public int Id { get; set; }
	public string Name { get; set; }
	public SeatStatus Status { get; set; }
	public int? UserId { get; set; }
	public User User { get; set; }
}
