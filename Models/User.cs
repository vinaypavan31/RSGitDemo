using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECart.Models{

    [Table("tbl_users")]
    public class User{
        
        [Key,Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{get;set;}

        //[Required]
        public string? Name{get;set;}

        //[EmailAddress(ErrorMessage ="ented valid email address")]
        public string? Email{get;set;}

        //[Required]
        public string? Address{get;set;}

        //[Phone]
        public string? Phone{get;set;}
        public Role Role{get;set;}
    }
}