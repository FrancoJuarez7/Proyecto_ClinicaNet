using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace ClinicaNet.ViewModel
{
    public class TicketsDetailsViewModel
    {
        public int TicketId { get; set; }
        public string? Observacion { get; set; }

        public string? EstadoDescripcion { get; set; }

        [Display(Name = "Fecha solicitud")]
        public DateTime FechaSolicitud { get; set; }

        [Display(Name = "Nombre afiliado")]
        public string? AfiliadoNombreCompleto { get; set; }

        [Display(Name = "Foto afiliado")]
        public string? NombreFoto { get; set; }

    }
}
