using System.ComponentModel.DataAnnotations;

namespace ClinicaNet.Models
{
    public class Estado
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Estado de la solicitud")]
        public string? Descripcion { get; set; }

        public List<TicketDetalle>? TicketsDetalles { get; set; } // Muchos TicketDetalles pueden estar asociado a un mismo Estado.
    }
}

/*
Estado: Pendiente de resolución

    - TicketDetalle 101: "Esperando respuesta del usuario para obtener más detalles sobre el error."

    - TicketDetalle 102: "El ticket está pendiente de validación por parte del supervisor antes de proceder."

Estado: En Proceso

    - TicketDetalle 102: "Investigando la causa del error reportado en el sistema."

    - TicketDetalle 103: "El equipo de soporte ha comenzado a reproducir el problema en un entorno controlado."

Estado: Resuelto

    - TicketDetalle 103: "Problema solucionado y ticket cerrado. Usuario confirmó la resolución."

Estado: Rechazado

    - TicketDetalle 104: "Ticket rechazado por no cumplir con los requisitos del sistema."
 */