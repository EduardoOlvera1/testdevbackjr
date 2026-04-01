using System.ComponentModel.DataAnnotations;

namespace LoginApi.DTOs.Login
{
    public class UpdateLoginDto
    {
        // only the extension is editable after a record is created
        [Required]
        public int Extension { get; set; }
    }
}