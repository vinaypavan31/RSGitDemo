namespace ECart.Models
{
    internal class LoginDto
    {
        public string Username {get; set;}
        public int Id {get; set;}
        public Role Role {get;set;}
        public string Token {get; set;}

    }

}