using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginApi.Models
{
    public class User
    {
        [Key]
        public int User_id { get; set; }

        public string? Login { get; set; }

        public string? Nombres { get; set; }

        public string? ApellidoPaterno { get; set; }

        public string? ApellidoMaterno { get; set; }

        public int IDArea { get; set; }

        public DateTime LastLoginAttempt { get; set; }

        [ForeignKey("IDArea")]
        public Area? Area { get; set; }
    }
}