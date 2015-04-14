using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCS.Model
{

    [Table("Users")]
    public partial class User 
    {
        public User()
        {
            UserId = Guid.NewGuid().ToString();
        }
        [Key]
        public string UserId { get; set; }

        [Required]
        [Display(Name="User Name")]
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public UserRole Role { get; set; }

        public DateTime? LastVisit { get; set; }

    }

    public enum UserRole
    {
        Admin,
        IT,
        Manager
    }
}
