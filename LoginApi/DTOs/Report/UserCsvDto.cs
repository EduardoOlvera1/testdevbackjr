namespace LoginApi.DTOs.Report
{
    public class UserCsvDto
    {
        public string UserLogin { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public double TotalHoras { get; set; }
    }
}
