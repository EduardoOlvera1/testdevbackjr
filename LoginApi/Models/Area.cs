using System.ComponentModel.DataAnnotations;

namespace LoginApi.Models
{
    public class Area
    {
        [Key]
        public int IDArea { get; set; }

        public string? Nombre { get; set; }
    }
}
