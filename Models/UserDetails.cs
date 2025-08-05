using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECart.Models
{
   
        [Table("tbl_userdetails")]
        public class UserDetails
        {
        
            [Key,Required]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id {get; set;}
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public virtual required User Users {get; set;}
            public Role Role { get; set; }
        }

    
}