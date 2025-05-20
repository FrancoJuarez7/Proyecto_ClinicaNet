using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaNet.Data;
using ClinicaNet.Models;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ClinicaNet.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaNet.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(string buscarAfiliado, int pagina = 1)
        {
            var applicationDbContext = _context.Tickets.Include(t => t.Afiliado).AsQueryable();

            if (!string.IsNullOrEmpty(buscarAfiliado))
            {
                applicationDbContext = applicationDbContext
                    .Where(x => (x.Afiliado.Nombre.Contains(buscarAfiliado) || 
                    x.Afiliado.Apellido.Contains(buscarAfiliado)));
            }

            int RegistrosPorPagina = 5;
            var registroMostrar = applicationDbContext.Skip((pagina - 1) * RegistrosPorPagina).Take(RegistrosPorPagina);

            TicketsViewModel ticketsViewModel = new TicketsViewModel()
            {
                Tickets = await registroMostrar.ToListAsync(),
                buscarAfiliado = buscarAfiliado
            };

            ticketsViewModel.Paginador.PaginaActual = pagina;
            ticketsViewModel.Paginador.RegistrosPorPagina = RegistrosPorPagina;

            ticketsViewModel.Paginador.TotalRegistros = await applicationDbContext.CountAsync();

            if (!string.IsNullOrEmpty(buscarAfiliado))
            {
                ticketsViewModel.Paginador.ValoresQueryString.Add("buscarAfiliado", buscarAfiliado);
            }

            return View(ticketsViewModel);
        }


        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Afiliado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Doctor")]
        public IActionResult Create()
        {
            ViewData["AfiliadoId"] = new SelectList(_context.Afiliados, "Id", "NombreCompleto");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AfiliadoId,FechaSolicitud,Observacion")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AfiliadoId"] = new SelectList(_context.Afiliados, "Id", "NombreCompleto", ticket.AfiliadoId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["AfiliadoId"] = new SelectList(_context.Afiliados, "Id", "NombreCompleto", ticket.AfiliadoId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AfiliadoId,FechaSolicitud,Observacion")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["AfiliadoId"] = new SelectList(_context.Afiliados, "Id", "NombreCompleto", ticket.AfiliadoId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Afiliado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
