using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Hospitalapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PacientesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Paciente> pacientes = new List<Paciente>();

            string connectionString = _configuration.GetConnectionString("MySqlConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Paciente";

                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();
                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Paciente paciente = new Paciente()
                    {
                        IdPaciente = Convert.ToInt32(dataReader["IdPaciente"]),
                        NombrePac = dataReader["NombrePac"].ToString(),
                        ApellidoPac = dataReader["ApellidoPac"].ToString(),
                        RutPac = dataReader["RutPac"].ToString(),
                        Nacionalidad = dataReader["Nacionalidad"].ToString(),
                        Visa = dataReader["Visa"].ToString(),
                        Genero = dataReader["Genero"].ToString(),
                        SintomasPac = dataReader["SintomasPac"].ToString(),
                        IdMedico = Convert.ToInt32(dataReader["IdMedico"])
                    };

                    pacientes.Add(paciente);
                }

                dataReader.Close();
                connection.Close();
            }

            return Ok(pacientes);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Paciente paciente)
        {
            if (paciente == null)
            {
                return BadRequest("El objeto Paciente es nulo");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "INSERT INTO Paciente (NombrePac, ApellidoPac, RutPac, Nacionalidad, Visa, Genero, SintomasPac, IdMedico) VALUES (@NombrePac, @ApellidoPac, @RutPac, @Nacionalidad, @Visa, @Genero, @SintomasPac, @IdMedico)";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NombrePac", paciente.NombrePac);
                    command.Parameters.AddWithValue("@ApellidoPac", paciente.ApellidoPac);
                    command.Parameters.AddWithValue("@RutPac", paciente.RutPac);
                    command.Parameters.AddWithValue("@Nacionalidad", paciente.Nacionalidad);
                    command.Parameters.AddWithValue("@Visa", paciente.Visa);
                    command.Parameters.AddWithValue("@Genero", paciente.Genero);
                    command.Parameters.AddWithValue("@SintomasPac", paciente.SintomasPac);
                    command.Parameters.AddWithValue("@IdMedico", paciente.IdMedico);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Ok("Paciente creado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Paciente paciente)
        {
            if (paciente == null)
            {
                return BadRequest("El objeto Paciente es nulo");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "UPDATE Paciente SET NombrePac = @NombrePac, ApellidoPac = @ApellidoPac, RutPac = @RutPac, Nacionalidad = @Nacionalidad, Visa = @Visa, Genero = @Genero, SintomasPac = @SintomasPac, IdMedico = @IdMedico WHERE IdPaciente = @id";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@NombrePac", paciente.NombrePac);
                    command.Parameters.AddWithValue("@ApellidoPac", paciente.ApellidoPac);
                    command.Parameters.AddWithValue("@RutPac", paciente.RutPac);
                    command.Parameters.AddWithValue("@Nacionalidad", paciente.Nacionalidad);
                    command.Parameters.AddWithValue("@Visa", paciente.Visa);
                    command.Parameters.AddWithValue("@Genero", paciente.Genero);
                    command.Parameters.AddWithValue("@SintomasPac", paciente.SintomasPac);
                    command.Parameters.AddWithValue("@IdMedico", paciente.IdMedico);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Ok("Paciente actualizado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "DELETE FROM Paciente WHERE IdPaciente = @id";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Ok("Paciente eliminado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
