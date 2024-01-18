
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PA.Core.Models
{
    /// <summary>
    /// Custom user data for Asp.net Core Identity
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(20)]
        public string? FirstName { get; set; }
        [Required]
        [StringLength(20)]
        public string? LastName { get; set; }

        [StringLength(6)]
        public string? Initials { get; set; }
        [StringLength(5)]
        public string? Title { get; set; }
        public DateTime? JoinDate { get; set; } = DateTime.Now;
        [Required]
        public PrimaryDepartment PrimaryDepartment { get; set; } = PrimaryDepartment.GENERAL_SERVICE;
        [StringLength(256)]
        [PersonalData]
        public string? Photo { get; set; } = string.Empty;
    }
}
