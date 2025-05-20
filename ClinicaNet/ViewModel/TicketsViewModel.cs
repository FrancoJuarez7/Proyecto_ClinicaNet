using ClinicaNet.Models;

namespace ClinicaNet.ViewModel
{
    public class TicketsViewModel
    {
        public string? buscarAfiliado { get; set; }
        public List<Ticket>? Tickets { get; set; }

        public PaginadorViewModel Paginador { get; set; } = new PaginadorViewModel();
    }
}
