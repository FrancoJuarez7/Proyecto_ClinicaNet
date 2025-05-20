using ClinicaNet.Models;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ClinicaNet.ViewModel
{
    public class TicketsDetailsListViewModel
    {
        public List<TicketDetalle>? TicketsDetalles { get; set; }

        public PaginadorViewModel Paginador { get; set; } = new PaginadorViewModel();
    }
}
