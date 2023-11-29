using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Hospitalapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // Constructor que recibe la configuración de la aplicación
        public ReservasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para obtener todas las reservas
        [HttpGet]
        public IActionResult Get()
        {
            List<Reserva> reservas = new List<Reserva>();

            // Obtener la cadena de conexión desde la configuración
            string connectionString = _configuration.GetConnectionString("MySqlConnection");

            // Utilizar la conexión a la base de datos MySQL
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Reserva"; // Consulta SQL para seleccionar todas las reservas

                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();
                MySqlDataReader dataReader = command.ExecuteReader();

                // Leer los resultados y crear objetos Reserva
                while (dataReader.Read())
                {
                    Reserva reserva = new Reserva()
                    {
                        IdReserva = Convert.ToInt32(dataReader["IdReserva"]),
                        Especialidad = dataReader["Especialidad"].ToString(),
                        DiaReserva = Convert.ToDateTime(dataReader["DiaReserva"]),
                        IdPaciente = Convert.ToInt32(dataReader["IdPaciente"])
                    };

                    reservas.Add(reserva);
                }

                dataReader.Close();
                connection.Close();
            }

            return Ok(reservas); // Devolver todas las reservas obtenidas
        }

        // Método para crear una nueva reserva
        [HttpPost]
        public IActionResult Post([FromBody] Reserva reserva)
        {
            if (reserva == null)
            {
                return BadRequest("El objeto Reserva es nulo");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "INSERT INTO Reserva (Especialidad, DiaReserva, IdPaciente) VALUES (@Especialidad, @DiaReserva, @IdPaciente)";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Especialidad", reserva.Especialidad);
                    command.Parameters.AddWithValue("@DiaReserva", reserva.DiaReserva);
                    command.Parameters.AddWithValue("@IdPaciente", reserva.IdPaciente);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Ok("Reserva creada correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // Método para actualizar una reserva existente
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Reserva reserva)
        {
            if (reserva == null)
            {
                return BadRequest("El objeto Reserva es nulo");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "UPDATE Reserva SET Especialidad = @Especialidad, DiaReserva = @DiaReserva, IdPaciente = @IdPaciente WHERE IdReserva = @id";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@Especialidad", reserva.Especialidad);
                    command.Parameters.AddWithValue("@DiaReserva", reserva.DiaReserva);
                    command.Parameters.AddWithValue("@IdPaciente", reserva.IdPaciente);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Ok("Reserva actualizada correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // Método para eliminar una reserva existente por su ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "DELETE FROM Reserva WHERE IdReserva = @id";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Ok("Reserva eliminada correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}