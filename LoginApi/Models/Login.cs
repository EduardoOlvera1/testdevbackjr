using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginApi.Models
{
public class Login
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int User_id { get; set; }

    [ForeignKey("User_id")]
    public User? User { get; set; }

    public int Extension { get; set; }

    [Required]
    public int TipoMov { get; set; }

    [Required]
    public DateTime Fecha { get; set; }
}
}