using System.ComponentModel.DataAnnotations;

namespace LoginApi.DTOs.Login
{
    public class CreateLoginDto
    {
        [Required]
        public int User_id { get; set; }

        public int Extension { get; set; }

        [Required]
        public int TipoMov { get; set; } // 1 login, 0 logout

        [Required]
        public DateTime Fecha { get; set; }
    }
}