namespace LoginApi.DTOs.Login
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Extension { get; set; }
        public int TipoMov { get; set; }
        public DateTime Fecha { get; set; }
    }
}