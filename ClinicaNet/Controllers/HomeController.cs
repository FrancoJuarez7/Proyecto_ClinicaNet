using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClinicaNet.Models;
using ClinicaNet.ViewModel;
using ClinicaNet.Data;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaNet.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public async Task<IActionResult> Index(int pagina = 1)
    {

        var ticketsDetallesQuery = _context.TicketsDetalles.Include(a => a.Estado).Include(t => t.Ticket).ThenInclude(r => r.Afiliado).
            Where(c => c.Estado.Descripcion.ToLower() == "pendiente de resolucion").OrderBy(t => t.Ticket.FechaSolicitud);

        int RegistrosPorPagina = 5;
        var registrosMostrar = ticketsDetallesQuery.Skip((pagina - 1) * RegistrosPorPagina).Take(RegistrosPorPagina);

        TicketsDetailsListViewModel ticketsDetallesViewModel = new TicketsDetailsListViewModel()
        {
            TicketsDetalles = await registrosMostrar.ToListAsync(),
        };

        ticketsDetallesViewModel.Paginador.PaginaActual = pagina;
        ticketsDetallesViewModel.Paginador.RegistrosPorPagina = RegistrosPorPagina;
        ticketsDetallesViewModel.Paginador.TotalRegistros = await ticketsDetallesQuery.CountAsync();

        return View(ticketsDetallesViewModel);
    }

    public async Task<IActionResult> TicketsDetails(int id)
    {
        var ticketDetalle = await _context.TicketsDetalles
           .Include(td => td.Ticket) 
           .ThenInclude(t => t.Afiliado) 
           .FirstOrDefaultAsync(td => td.TicketId == id);

        if(ticketDetalle == null)
        {
            return NotFound();
        }

        TicketsDetailsViewModel ticketsDetailsViewModel = new TicketsDetailsViewModel()
        {
            TicketId = ticketDetalle.Id,
            AfiliadoNombreCompleto = ticketDetalle.Ticket.Afiliado.NombreCompleto,
            FechaSolicitud = ticketDetalle.Ticket.FechaSolicitud,
            Observacion = ticketDetalle.Ticket.Observacion,
            NombreFoto = ticketDetalle.Ticket.Afiliado.NombreFoto
        };

        return View(ticketsDetailsViewModel); 
    }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
