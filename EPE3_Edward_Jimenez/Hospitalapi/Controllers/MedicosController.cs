using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Hospitalapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        //inyeccion de depencias, IConfiguration es una interfaz proporcionada por ASP.NET Core que permite acceder a la configuración de la aplicación.
        public MedicosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult ListarMedico()
        {
            List<Medicos> medicos = new List<Medicos>();

            string connectionString = _configuration.GetConnectionString("MySqlConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Medicos";

                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();
                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Medicos medico = new Medicos()
                    {
                        idMedico = Convert.ToInt32(dataReader["idMedico"]),
                        NombreMed = dataReader["NombreMed"].ToString(),
                        ApellidoMed = dataReader["ApellidoMed"].ToString(),
                        RunMed = dataReader["RunMed"].ToString(),
                        EunaCom = dataReader["EunaCom"].ToString(),
                        NacionalidadMed = dataReader["NacionalidadMed"].ToString(),
                        Especialidad = dataReader["Especialidad"].ToString(),
                    };

                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Horarios")))
                    {
                        medico.Horarios = TimeSpan.Parse(dataReader.GetString("Horarios"));
                    }
                    else
                    {
                        medico.Horarios = TimeSpan.Zero; // Valor predeterminado en caso de valor nulo
                    }

                    medico.TarifaHr = Convert.ToInt32(dataReader["TarifaHr"]);

                    medicos.Add(medico);
                }

                dataReader.Close();
                connection.Close();
            }

            return Ok(medicos);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Medicos medico)
        {
            if (medico == null)
            {
                return BadRequest("El objeto Medico es nulo");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "INSERT INTO Medicos (NombreMed, ApellidoMed, RunMed, EunaCom, NacionalidadMed, Especialidad, Horarios, TarifaHr) VALUES (@NombreMed, @ApellidoMed, @RunMed, @EunaCom, @NacionalidadMed, @Especialidad, @Horarios, @TarifaHr)";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NombreMed", medico.NombreMed);
                    command.Parameters.AddWithValue("@ApellidoMed", medico.ApellidoMed);
                    command.Parameters.AddWithValue("@RunMed", medico.RunMed);
                    command.Parameters.AddWithValue("@EunaCom", medico.EunaCom);
                    command.Parameters.AddWithValue("@NacionalidadMed", medico.NacionalidadMed);
                    command.Parameters.AddWithValue("@Especialidad", medico.Especialidad);
                    command.Parameters.AddWithValue("@Horarios", medico.Horarios);
                    command.Parameters.AddWithValue("@TarifaHr", medico.TarifaHr);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Ok("Medico creado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Medicos medico)
        {
            if (medico == null)
            {
                return BadRequest("El Medico es nulo");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "UPDATE Medicos SET NombreMed = @NombreMed, ApellidoMed = @ApellidoMed, RunMed = @RunMed, EunaCom = @EunaCom, NacionalidadMed = @NacionalidadMed, Especialidad = @Especialidad, Horarios = @Horarios, TarifaHr = @TarifaHr WHERE idMedico = @id";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@NombreMed", medico.NombreMed);
                    command.Parameters.AddWithValue("@ApellidoMed", medico.ApellidoMed);
                    command.Parameters.AddWithValue("@RunMed", medico.RunMed);
                    command.Parameters.AddWithValue("@EunaCom", medico.EunaCom);
                    command.Parameters.AddWithValue("@NacionalidadMed", medico.NacionalidadMed);
                    command.Parameters.AddWithValue("@Especialidad", medico.Especialidad);
                    command.Parameters.AddWithValue("@Horarios", medico.Horarios);
                    command.Parameters.AddWithValue("@TarifaHr", medico.TarifaHr);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Ok("Medico actualizado correctamente");
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
                    string query = "DELETE FROM Medicos WHERE idMedico = @id";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Ok("Medico eliminado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}