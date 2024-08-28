using Microsoft.EntityFrameworkCore;
using Reservation.Core.Contract.Repository;
using Reservation.Core.Entities;
using Reservation.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ReservationDbContext _dbContext;

        public GenericRepository(ReservationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddedAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public void DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(User))
            {
                return (IEnumerable<T>)_dbContext.Set<User>().Include(U => U.ParentsSeats).Include(U => U.KidsSeat).ToList();
            }
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<ISeat>> GetSeatsByUserId(int userId)
        {
            if (typeof(T) == typeof(ParentsSeat))
            {
                return await _dbContext.Set<ParentsSeat>().Where(seat => seat.UserId == userId).ToListAsync();

            }
            return await _dbContext.Set<KidsSeat>().Where(seat => seat.UserId == userId).ToListAsync();
        }


        public void UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            _dbContext.SaveChanges();
        }

        public void UpdateAll(IEnumerable<T> entity)
        {
            _dbContext.Set<T>().UpdateRange(entity);
            _dbContext.SaveChanges();
        }
    }
}
