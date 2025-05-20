using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaNet.Data;
using ClinicaNet.Models;
using ClinicaNet.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaNet.Controllers
{
    [Authorize]
    public class TicketsDetallesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsDetallesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketsDetalles
        public async Task<IActionResult> Index(int pagina = 1)
        {
            var applicationDbContext = _context.TicketsDetalles.Include(t => t.Estado).Include(t => t.Ticket);

            int RegistrosPorPagina = 5;

            var registrosMostrar = applicationDbContext.Skip((pagina - 1) * RegistrosPorPagina).Take(RegistrosPorPagina);

            TicketsDetailsListViewModel ticketsDetallesViewModel = new TicketsDetailsListViewModel()
            {
                TicketsDetalles = await registrosMostrar.ToListAsync()
            };

            ticketsDetallesViewModel.Paginador.PaginaActual = pagina;
            ticketsDetallesViewModel.Paginador.RegistrosPorPagina = RegistrosPorPagina;
            ticketsDetallesViewModel.Paginador.TotalRegistros = await applicationDbContext.CountAsync();

            return View(ticketsDetallesViewModel);
        }

        // GET: TicketsDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketDetalle = await _context.TicketsDetalles
                .Include(t => t.Estado)
                .Include(t => t.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketDetalle == null)
            {
                return NotFound();
            }

            return View(ticketDetalle);
        }

        // GET: TicketsDetalles/Create
        [Authorize(Roles = "Doctor")]
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion");
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Id");
            return View();
        }

        // POST: TicketsDetalles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketId,DescripcionPedido,EstadoId,FechaEstado")] TicketDetalle ticketDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", ticketDetalle.EstadoId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Id", ticketDetalle.TicketId);
            return View(ticketDetalle);
        }

        // GET: TicketsDetalles/Edit/5
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketDetalle = await _context.TicketsDetalles.FindAsync(id);
            if (ticketDetalle == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", ticketDetalle.EstadoId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Id", ticketDetalle.TicketId);
            return View(ticketDetalle);
        }

        // POST: TicketsDetalles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketId,DescripcionPedido,EstadoId,FechaEstado")] TicketDetalle ticketDetalle)
        {
            if (id != ticketDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketDetalleExists(ticketDetalle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", ticketDetalle.EstadoId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Id", ticketDetalle.TicketId);
            return View(ticketDetalle);
        }

        // GET: TicketsDetalles/Delete/5
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketDetalle = await _context.TicketsDetalles
                .Include(t => t.Estado)
                .Include(t => t.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketDetalle == null)
            {
                return NotFound();
            }

            return View(ticketDetalle);
        }

        // POST: TicketsDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketDetalle = await _context.TicketsDetalles.FindAsync(id);
            if (ticketDetalle != null)
            {
                _context.TicketsDetalles.Remove(ticketDetalle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketDetalleExists(int id)
        {
            return _context.TicketsDetalles.Any(e => e.Id == id);
        }
    }
}
