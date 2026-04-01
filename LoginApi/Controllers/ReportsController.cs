using Microsoft.AspNetCore.Mvc;
using LoginApi.Services;
using LoginApi.DTOs.Login;
using System.Text;

namespace LoginApi.ReportsController
{
    [ApiController]
    [Route("reports")]
    public class ReportsController : ControllerBase
    {
        private readonly LoginService _service;

        public ReportsController(LoginService service)
        {
            _service = service;
        }

        [HttpGet("csv")]
        public async Task<IActionResult> GetCsv()
        {
            var data = await _service.GetCsvReportData();

            var sb = new StringBuilder();
            sb.AppendLine("Usuario,Nombre Completo,Area,Total Horas Trabajadas");

            foreach (var row in data)
            {
                var login = EscapeCsvField(row.UserLogin);
                var nombre = EscapeCsvField(row.NombreCompleto);
                var area = EscapeCsvField(row.Area);
                sb.AppendLine($"{login},{nombre},{area},{row.TotalHoras}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "reporte_horas.csv");
        }

        private static string EscapeCsvField(string field)
        {
            if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
                return $"\"{field.Replace("\"", "\"\"")}\"";
            return field;
        }
    }
}