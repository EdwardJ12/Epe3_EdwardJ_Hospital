public class Reserva
{
    public int IdReserva { get; set; }
    public string? Especialidad { get; set; }
    public DateTime DiaReserva { get; set; }
    public int IdPaciente { get; set; }

    public Paciente? Paciente { get; set; }
}