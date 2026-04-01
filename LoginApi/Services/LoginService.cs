using LoginApi.Data;
using LoginApi.DTOs.Login;
using LoginApi.DTOs.Report;
using LoginApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginApi.Services
{
    public class LoginService
    {
        private readonly AppDbContext _context;

        public LoginService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LoginResponseDto>> GetAll()
        {
            return await _context.Logins
                .Select(x => new LoginResponseDto
                {
                    Id = x.Id,
                    User_id = x.User_id,
                    Extension = x.Extension,
                    TipoMov = x.TipoMov,
                    Fecha = x.Fecha
                })
                .ToListAsync();
        }

        public async Task<(bool ok, string error, LoginResponseDto? data)> Create(CreateLoginDto dto)
        {
            if (dto.Fecha > DateTime.Now)
                return (false, "La fecha no puede ser futura", null);

            var userExists = await _context.Users
                .AnyAsync(u => u.User_id == dto.User_id);

            if (!userExists)
                return (false, "El usuario no existe", null);

            // check the last movement to enforce login/logout alternation
            var ultimo = await _context.Logins
                .Where(x => x.User_id == dto.User_id)
                .OrderByDescending(x => x.Fecha)
                .FirstOrDefaultAsync();

            if (dto.TipoMov == 1 && ultimo?.TipoMov == 1)
                return (false, "Ya hay un login sin logout", null);

            if (dto.TipoMov == 0 && (ultimo == null || ultimo.TipoMov == 0))
                return (false, "No hay login previo para logout", null);

            var entity = new Login
            {
                User_id = dto.User_id,
                Extension = dto.Extension,
                TipoMov = dto.TipoMov,
                Fecha = dto.Fecha
            };

            _context.Logins.Add(entity);
            await _context.SaveChangesAsync();

            return (true, "", new LoginResponseDto
            {
                Id = entity.Id,
                User_id = entity.User_id,
                Extension = entity.Extension,
                TipoMov = entity.TipoMov,
                Fecha = entity.Fecha
            });
        }

        public async Task<bool> Update(int id, UpdateLoginDto dto)
        {
            var entity = await _context.Logins.FindAsync(id);
            if (entity == null) return false;

            entity.Extension = dto.Extension;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.Logins.FindAsync(id);
            if (entity == null) return false;

            _context.Logins.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserCsvDto>> GetCsvReportData()
        {
            var users = await _context.Users
                .Include(u => u.Area)
                .ToListAsync();

            // load sorted so the pairing loop below works without extra ordering
            var logins = await _context.Logins
                .OrderBy(l => l.User_id)
                .ThenBy(l => l.Fecha)
                .ToListAsync();

            var loginsByUser = logins
                .GroupBy(l => l.User_id)
                .ToDictionary(g => g.Key, g => g.ToList());

            return users.Select(u =>
            {
                double totalHoras = 0;

                if (loginsByUser.TryGetValue(u.User_id, out var userLogins))
                {
                    // pair each login (1) with the immediate following logout (0)
                    Login? lastLogin = null;
                    foreach (var record in userLogins)
                    {
                        if (record.TipoMov == 1)
                        {
                            lastLogin = record;
                        }
                        else if (record.TipoMov == 0 && lastLogin != null)
                        {
                            totalHoras += (record.Fecha - lastLogin.Fecha).TotalHours;
                            lastLogin = null;
                        }
                    }
                }

                return new UserCsvDto
                {
                    UserLogin = u.Login ?? "",
                    NombreCompleto = $"{u.Nombres} {u.ApellidoPaterno} {u.ApellidoMaterno}".Trim(),
                    Area = u.Area?.Nombre ?? "",
                    TotalHoras = Math.Round(totalHoras, 2)
                };
            }).ToList();
        }
    }
}