using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClinicaNet.Models
{
    public class TicketDetalle
    {
        public int Id { get; set; }

        [Display(Name = "Numero de ticket")]
        public int TicketId { get; set; } // Un Ticket puede tener muchos TicketDetalles.

        public Ticket? Ticket { get; set; } /* Propiedad de navegación (Cada TicketDetalle pertenece a un Ticket, pero un Ticket puede tener varios 
                                         TicketDetalles).*/
        [Display(Name="Descripcion del pedido")]
        public string? DescripcionPedido { get; set; }

        [Display(Name = "Estado de la solicitud")]
        public int EstadoId { get; set; } // Un Estado puede estar asociado a muchos TicketDetalles.
        public Estado? Estado { set; get; } /* Propiedad de navegación (Cada TicketDetalle tiene un único Estado, pero un Estado puede estar 
                                             asociado a múltiples TicketDetalles). */
        [Display(Name = "Fecha del estado asignado")]
        public DateTime FechaEstado { get; set; }

    }
}

// El ejemplo de que un Ticket puede tener muchos TicketDetalles esta en la clase de Ticket.
// El ejemplo de que un Estado puede estar asociado a muchos TicketDetalles esta en la clase de Estado.
