using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservation.Core.Contract.Repository;
using Reservation.Core.Entities;
using Reservation.DTOs;

namespace Reservation.Controllers
{

    public class UserController : BaseApiController
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ParentsSeat> _parentsSeatRepo;
        private readonly IGenericRepository<KidsSeat> _kidsSeatRepo;

        public UserController(IGenericRepository<User> UserRepo, IMapper mapper,
        IGenericRepository<ParentsSeat> ParentsSeatRepo,
        IGenericRepository<KidsSeat> KidsSeatRepo
        )
        {
            _userRepo = UserRepo;
            _mapper = mapper;
            _parentsSeatRepo = ParentsSeatRepo;
            _kidsSeatRepo = KidsSeatRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userRepo.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userRepo.GetAsync(id);
            var mappedUser = _mapper.Map<User, UserDto>(user);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(mappedUser);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> AddUser(UserDto userDto)
        {
            var mappedUser = _mapper.Map<UserDto, User>(userDto);
            await _userRepo.AddedAsync(mappedUser); // Ensure that the method is awaited
            return Ok(mappedUser);
        }

        [HttpDelete("RemoveUser/{id}")]
        public async Task<ActionResult<String>> DeleteUser(int id)
        {
            var parentsSeats = await _parentsSeatRepo.GetSeatsByUserId(id);
            foreach (var seat in parentsSeats)
            {
                seat.UserId = null;
                seat.Status = SeatStatus.free;
            }
            _parentsSeatRepo.UpdateAll((IEnumerable<ParentsSeat>)parentsSeats);
            var kidsSeats = await _kidsSeatRepo.GetSeatsByUserId(id);
            foreach (var seat in kidsSeats)
            {
                seat.UserId = null;
                seat.Status = SeatStatus.free;
            }
            _kidsSeatRepo.UpdateAll((IEnumerable<KidsSeat>)kidsSeats);

            var user = await _userRepo.GetAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            _userRepo.DeleteAsync(user);
            return Ok("User Deleted Successfully");
        }

        [HttpPut("UpdatUser/{id}")]
        public async Task<ActionResult<UserDto>> UpdatUserData(int id, UserDto userDto)
        {
            var user = await _userRepo.GetAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            user.Name = userDto.Name;
            user.Phone = userDto.Phone;
            user.Email = userDto.Email;
            user.NumberOfReservation = userDto.NumberOfReservation;



            _userRepo.UpdateAsync(user);
            return Ok(user);
        }

        [HttpPost("ReserveSeat/{UserId}")]
        public async Task<ActionResult<string>> ReserveSeat(ReserveSeatDto AllSeatsDto,
                                                            int UserId)
        {
            var user = await _userRepo.GetAsync(UserId);
            if (user is null)
            {
                return NotFound(new { message = "User Not Found" });
            }
            if (AllSeatsDto.ParentsSeatsId != null)
            {

                for (int i = 0; i < AllSeatsDto.ParentsSeatsId.Count(); i++)
                {
                    var seatId = AllSeatsDto.ParentsSeatsId[i];
                    var seat = await _parentsSeatRepo.GetAsync(seatId);
                    if (seat is null)
                    {
                        return NotFound(new { message = "Seat Not Found" });
                    }
                    if (seat.Status == SeatStatus.Reserved)
                    {
                        return BadRequest(new { message = "Seat Is Already Reserved" });
                    }

                    seat.UserId = UserId;
                    seat.Status = SeatStatus.Reserved;

                    _parentsSeatRepo.UpdateAsync(seat);
                }
            }
            for (int i = 0; i < AllSeatsDto.KidsSeatsId.Count(); i++)
            {
                var seatId = AllSeatsDto.KidsSeatsId[i];
                var seat = await _kidsSeatRepo.GetAsync(seatId);
                if (seat is null)
                {
                    return NotFound(new { message = "Seat Not Found" });
                }
                if (seat.Status == SeatStatus.Reserved)
                {
                    return BadRequest(new { message = "Seat Is Already Reserved" });
                }

                seat.UserId = UserId;
                seat.Status = SeatStatus.Reserved;

                _kidsSeatRepo.UpdateAsync(seat);
            }

            return Ok("Seat Is Reserved Successfully");

        }

        [HttpGet("GetAllSeatsByUserId/{userId}")]
        public async Task<ActionResult<Dictionary<string, IEnumerable<ISeat>>>> GetAllSeatsByUserId(int userId)
        {
            var kidsSeats = await _kidsSeatRepo.GetSeatsByUserId(userId);
            var parentsSeats = await _parentsSeatRepo.GetSeatsByUserId(userId);
            return Ok(new Dictionary<string, IEnumerable<ISeat>>
            {
                {"KidsSeats", kidsSeats},
                {"ParentsSeats", parentsSeats}
            });
        }

        [HttpPut("RemoveAllSeatByUserId/{userId}")]
        public async Task<IActionResult> RemoveAllSeatByUserId(int userId)
        {
            // Retrieve all seats with the specified UserId
            var parentsSeats = await _parentsSeatRepo.GetSeatsByUserId(userId);

            // Check if any seats were found
            if (parentsSeats == null || !parentsSeats.Any())
            {
                return NotFound($"No seats found for UserId {userId}");
            }

            // Update each seat's UserId and Status
            foreach (var seat in parentsSeats)
            {
                seat.UserId = null;
                seat.Status = SeatStatus.free;
            }

            // Save the changes
            _parentsSeatRepo.UpdateAll((IEnumerable<ParentsSeat>)parentsSeats);
            // Retrieve all seats with the specified UserId
            var kidsSeats = await _kidsSeatRepo.GetSeatsByUserId(userId);

            // Check if any seats were found
            if (kidsSeats == null || !kidsSeats.Any())
            {
                return NotFound($"No seats found for UserId {userId}");
            }

            // Update each seat's UserId and Status
            foreach (var seat in kidsSeats)
            {
                seat.UserId = null;
                seat.Status = SeatStatus.free;
            }

            // Save the changes
            _kidsSeatRepo.UpdateAll((IEnumerable<KidsSeat>)kidsSeats);

            // Return an Ok result
            return Ok();
        }


        [HttpGet("GetAllSeat")]
        public async Task<ActionResult<Dictionary<string, IEnumerable<ISeat>>>> GetAllSeats()
        {
            var kidsSeats = await _kidsSeatRepo.GetAllAsync();
            var parentsSeats = await _parentsSeatRepo.GetAllAsync();
            return Ok(new Dictionary<string, IEnumerable<ISeat>>
            {
                {"KidsSeats", kidsSeats},
                {"ParentsSeats", parentsSeats}

            });
        }
    }
}
