using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;

namespace MoviesAdmin.Services
{
    public class UserSnapshotService : IUserSnapshotService
    {
        private readonly MoviesAdminContext _context;

        public UserSnapshotService(MoviesAdminContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(string userId, string email, string name)
        {
            var userSnapshot = await _context.UserSnapshot.FirstOrDefaultAsync(u => u.UserId == userId);

            if (userSnapshot == null)
            {
                _context.UserSnapshot.Add(new UserSnapshot
                {
                    UserId = userId,
                    Email = email,
                    Name = name
                });
            }
            else
            {
                userSnapshot.Email = email;
                userSnapshot.Name = name;
                _context.UserSnapshot.Update(userSnapshot);
            }

            await _context.SaveChangesAsync();
        }
    }
}
