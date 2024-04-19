using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Security.Policy;
using System.Text;
using System.Diagnostics;

namespace WebApplication1.Service
{
    public class UserService
    {


        private readonly WebDbContext _context;

        public UserService(WebDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserProfile>> GetUserProfile(string userId)
        {
            var userProfiles = await _context.UserProfiles.Where(up => up.Id == userId).ToListAsync();

            if (userProfiles == null || !userProfiles.Any())
            {
                return new List<UserProfile>();
            }

            return userProfiles.Select(up => new UserProfile
            {
                accId = up.accId,
                profileType = up.profileType,
                name = up.name,
                url = up.url
            }).ToList();
        }


        public async Task AddUserProfileAsync(string userId, string accId, string profileType, string name, string url)
        {
                var userProfile = new UserProfile
                {
                    Id = userId,
                    accId = accId,
                    profileType = profileType,
                    name = name,
                    url = url
                };

                Debug.WriteLine($"UserService- AddUserProfileAsync- {userProfile?.accId}, {userProfile?.profileType}, {userProfile?.name}, {userProfile?.url}");

                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();
            
        }

        public async Task UpdateUserProfileAsync(string userId, string accId, string profileType, string name, string url)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.accId == accId);

            if (user != null)
            {
                user.name = name;

                user.url = url;

                await _context.SaveChangesAsync();
            }
        }

    }

}
