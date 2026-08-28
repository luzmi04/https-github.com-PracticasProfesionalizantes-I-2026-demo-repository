using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SanSaludAPI.DataAccess
{
    public class DatabaseInitializer
    {
        private readonly SanSaludDbContext _context;

        public DatabaseInitializer(SanSaludDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            // Crea la base de datos si no existe (aplica el modelo actual)
            _context.Database.EnsureCreated();

            // Semilla de Pacientes
            if (!await _context.Pacientes.AnyAsync())
            {
                var pacientes = new[]
                {
                    new Paciente { Id = Guid.NewGuid(), Nombre = "Juan Pérez", DNI = "12345678", Email = "juan.perez@example.com" },
                    new Paciente { Id = Guid.NewGuid(), Nombre = "María Gómez", DNI = "87654321", Email = "maria.gomez@example.com" },
                    new Paciente { Id = Guid.NewGuid(), Nombre = "Luis Fernández", DNI = "11223344", Email = "luis.fernandez@example.com" }
                };

                await _context.Pacientes.AddRangeAsync(pacientes);
                await _context.SaveChangesAsync();
            }

            // Semilla de Medicos
            if (!await _context.Medicos.AnyAsync())
            {
                var medicos = new[]
                {
                    new Medico { Id = Guid.NewGuid(), Nombre = "Dra. Ana Ruiz", Especialidad = "Pediatría", Matricula = "M-1001" },
                    new Medico { Id = Guid.NewGuid(), Nombre = "Dr. Carlos López", Especialidad = "Cardiología", Matricula = "M-1002" },
                    new Medico { Id = Guid.NewGuid(), Nombre = "Dra. Elena Soto", Especialidad = "Dermatología", Matricula = "M-1003" }
                };

                await _context.Medicos.AddRangeAsync(medicos);
                await _context.SaveChangesAsync();
            }

            // Semilla de Turnos (asegurarse de que haya pacientes y medicos)
            if (!await _context.Turnos.AnyAsync())
            {
                var pacientes = await _context.Pacientes.Take(3).ToListAsync();
                var medicos = await _context.Medicos.Take(3).ToListAsync();

                if (pacientes.Count >= 1 && medicos.Count >= 1)
                {
                    var turnos = new[]
                    {
                        new Turno { Id = Guid.NewGuid(), PacienteId = pacientes[0].Id, MedicoId = medicos[0].Id, FechaHora = DateTime.UtcNow.AddDays(1), DuracionHoras = 1 },
                        new Turno { Id = Guid.NewGuid(), PacienteId = pacientes[Math.Min(1, pacientes.Count - 1)].Id, MedicoId = medicos[Math.Min(1, medicos.Count - 1)].Id, FechaHora = DateTime.UtcNow.AddDays(2), DuracionHoras = 2 },
                        new Turno { Id = Guid.NewGuid(), PacienteId = pacientes[Math.Min(2, pacientes.Count - 1)].Id, MedicoId = medicos[Math.Min(2, medicos.Count - 1)].Id, FechaHora = DateTime.UtcNow.AddDays(3), DuracionHoras = 2 }
                    };

                    await _context.Turnos.AddRangeAsync(turnos);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
