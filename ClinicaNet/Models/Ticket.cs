using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClinicaNet.Models
{
    public class Ticket
    {
        [Display(Name = "Numero de ticket")]
        public int Id { get; set; }

        [Display(Name = "Afiliado")]
        public int AfiliadoId { get; set; } // Un afilidado puede tener muchos Tickets.
        public Afiliado? Afiliado { get; set; } /* Propiedad de navegación (Un Ticket está asociado a un Afiliado, y un Afiliado puede tener varios 
                                                 Tickets).*/
        [Display(Name="Fecha de solicitud")]
        public DateTime FechaSolicitud { get; set; }

        public string? Observacion { get; set; }

        public List<TicketDetalle>? TicketsDetalles { get; set; } // Muchos TicketDetalles pueden tener un mismo Ticket.

    }
}

/* 
Un Afiliado puede tener varios Tickets:

Afiliado 34 
 - Ticket 101: Resonancia magnetica.
 - Ticket 102: Revision dentista.
 - Ticket 103: Fisioterapia.

Un Ticket puede tener muchos TicketsDetalles porque cada TicketsDetalles muestra las etapas del Ticket:
 
📄 Ticket 101: "Juan no puede ingresar a su cuenta"

🧩 TicketsDetalles (etapas del proceso), esto se mostrara en el Index de TicketsDetalles:

 - TicketDetalle del Ticket 101 → Estado: Abierto, Fecha: 01/04
 - TicketDetalle del Ticket 101  → Estado: En Proceso, Fecha: 02/04
 - TicketDetalle del Ticket 101  → Estado: Resuelto, Fecha: 03/04 
*/