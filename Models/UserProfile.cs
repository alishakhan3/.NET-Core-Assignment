using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserProfile : IdentityUser
    {
        [Key]
        public string accId { get; set; }

        public string profileType { get; set; }

        public string name { get; set; }

        public string url { get; set; }
    }
}
