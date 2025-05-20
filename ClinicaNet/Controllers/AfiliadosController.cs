using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaNet.Data;
using ClinicaNet.Models;
using ClosedXML.Excel;
using ClinicaNet.ViewModel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaNet.Controllers
{
    [Authorize]
    public class AfiliadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public AfiliadosController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Afiliados
        public async Task<IActionResult> Index(string buscarNombre, string buscarApellido, long? buscarDNI, int pagina = 1)
        {
            // PRIMERA PARTE DE LA CONSULTA:
            // Se arma la base de la consulta sobre la tabla Afiliados (ya sea con filtros o sin ellos).

            var applicationDbContext = _context.Afiliados.AsQueryable();

            // Aplicar filtros si se proporcionan
            if (!string.IsNullOrEmpty(buscarNombre))
            {
                applicationDbContext = applicationDbContext.Where(x => x.Nombre.Contains(buscarNombre));
            }

            if (!String.IsNullOrEmpty(buscarApellido))
            {
                applicationDbContext = applicationDbContext.Where(x => x.Apellido.Contains(buscarApellido));
            }

            if (buscarDNI.HasValue) {

                applicationDbContext = applicationDbContext.Where(x => x.DNI.ToString().Contains(buscarDNI.ToString()));
            }

            int RegistrosPorPagina = 5; // Cantidad de registros que se mostrarán por página.

            // SEGUNDA PARTE DE LA CONSULTA:
            /* En este punto se termina de construir la consulta, incluyendo los filtros (si existen o no) y la paginación. Aca se calcula qué registros 
               de afiliados deben mostrarse según la página actual. Por ejemplo, si deben mostrarse 5 registros, aquí se seleccionan esos 5. La consulta 
               aún no se ejecuta hasta que se llame a ToListAsync(), lo cual ocurre más abajo. 

              EJEMPLO DE ESTA CONSULTA: Si estoy en la pagina 2:
             
              var registroMostrar = applicationDbContext.Skip((2 - 1) * 5).Take(5); //Paso 1: Calcula cuántos registros se deben omitir → (2 - 1) * 5 = 5
              var registroMostrar = applicationDbContext.Skip(5).Take(5); //Paso 2: RESULTADO:
                                                                           Skip(5): omite los primeros 5 registros
                                                                           Take(5): toma los 5 registros siguientes (a partir del 6, si existen)

              En resumen: Skip omite los registros anteriores, Take selecciona los que corresponden a la página actual, es decir, al omitir los primeros 
              5 registros, Take te muestra los 5 registros siguientes, comenzando desde el 6. */

            var registroMostrar = applicationDbContext.Skip((pagina - 1) * RegistrosPorPagina).Take(RegistrosPorPagina);

            // Se construye el ViewModel, ejecutando la consulta para traer solo los registros de la página actual.
            AfiliadosViewModel afiliadosViewModel = new AfiliadosViewModel()
            {
                Afiliados = await registroMostrar.ToListAsync(), /* registroMostrar es la última parte de la consulta (ya incluye los filtros y la 
                                            paginación). Al hacer ToListAsync(), se ejecuta la consulta final en la BBDD y se obtienen los resultados.*/
                buscarNombre = buscarNombre,
                buscarApellido = buscarApellido,
                buscarDNI = buscarDNI
            };

            afiliadosViewModel.Paginador.PaginaActual = pagina; // Página actual que se está mostrando
            afiliadosViewModel.Paginador.RegistrosPorPagina = RegistrosPorPagina;  // Cantidad de registros por página
            afiliadosViewModel.Paginador.TotalRegistros = await applicationDbContext.CountAsync(); // Cantidad total de registros (filtrados o no)

            // Se agregan los filtros al diccionario para que se conserven al navegar entre páginas

            if (!string.IsNullOrEmpty(buscarNombre)) 
            {
                afiliadosViewModel.Paginador.ValoresQueryString.Add("buscarNombre", buscarNombre);
            }
            if (!string.IsNullOrEmpty(buscarApellido))
            {
                afiliadosViewModel.Paginador.ValoresQueryString.Add("buscarApellido", buscarApellido);
            }
            if (buscarDNI.HasValue)
            {
                afiliadosViewModel.Paginador.ValoresQueryString.Add("buscarDNI", buscarDNI.ToString());
            }


            return View(afiliadosViewModel);
        }

        // GET: Afiliados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var afiliado = await _context.Afiliados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (afiliado == null)
            {
                return NotFound();
            }

            return View(afiliado);
        }

        // GET: Afiliados/Create
        [Authorize(Roles = "Asistente")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Afiliados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,DNI,FechaNacimiento")] Afiliado afiliado)
        {
            if (ModelState.IsValid)
            {
   
                var archivos = HttpContext.Request.Form.Files; 

                if (archivos !=null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0]; 

                    if(archivoFoto.Length > 0) 
                    {
                      
                        var pathDestino = Path.Combine(_env.WebRootPath, "images\\fotos");

                        
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "");

                       
                        var extension = Path.GetExtension(archivoFoto.FileName);

                       
                        archivoDestino += extension;


                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            afiliado.NombreFoto = archivoDestino;
                        }


                    }
                }

                _context.Add(afiliado);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(afiliado);
        }

        // GET: Afiliados/Edit/5
       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var afiliado = await _context.Afiliados.FindAsync(id);
            if (afiliado == null)
            {
                return NotFound();
            }
            return View(afiliado);
        }

        // POST: Afiliados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,DNI,FechaNacimiento,NombreFoto")] Afiliado afiliado)
        {
            if (id != afiliado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;

                if(archivos !=null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];

                    if(archivoFoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(_env.WebRootPath, "images\\fotos");

                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", " ");
                        var extension = Path.GetExtension(archivoFoto.FileName);
                        archivoDestino += extension;

                        using (var fileStream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(fileStream);

                            if (!string.IsNullOrEmpty(afiliado.NombreFoto)) //Esto valida sino hay foto anterior asociada porque sino lo pongo falla
                            {
                                var archivoFotoAnterior = Path.Combine(pathDestino, afiliado.NombreFoto);

                                if (System.IO.File.Exists(archivoFotoAnterior))
                                {
                                    System.IO.File.Delete(archivoFotoAnterior);
                                }

                            }

                            afiliado.NombreFoto = archivoDestino;
                        }
                    }
                }

                try
                {
                    _context.Update(afiliado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AfiliadoExists(afiliado.Id))
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
            return View(afiliado);
        }

        // GET: Afiliados/Delete/5
        [Authorize(Roles = "Asistente")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var afiliado = await _context.Afiliados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (afiliado == null)
            {
                return NotFound();
            }

            return View(afiliado);
        }

        // POST: Afiliados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var afiliado = await _context.Afiliados.FindAsync(id);
            if (afiliado != null)
            {
                _context.Afiliados.Remove(afiliado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AfiliadoExists(int id)
        {
            return _context.Afiliados.Any(e => e.Id == id);
        }

        // Importar afiliados
        public async Task<IActionResult> ImportarExcel()
        {
            try
            {
                var archivos = HttpContext.Request.Form.Files;

                if (archivos != null && archivos.Count > 0)
                {
                    var archivoExcel = archivos[0];

                    if (archivoExcel.Length > 0)
                    {

                        var workbook = new XLWorkbook(archivoExcel.OpenReadStream()); //Creamos el archivo excel
                        var hoja = workbook.Worksheet(1); //Accedemos a la primera hoja del Excel

                        // Obtenemos el número de la primera y de la ultima fila que contienen datos en la hoja
                        var primeraFila = hoja.FirstRowUsed().RangeAddress.FirstAddress.RowNumber;
                        var ultimaFila = hoja.LastRowUsed().RangeAddress.LastAddress.RowNumber;

                        List<Afiliado> listaAfiliados = new List<Afiliado>();

                        for (int i = primeraFila; i <= ultimaFila; i++)
                        {
                            var fila = hoja.Row(i);

                            Afiliado afiliado = new Afiliado();

                            afiliado.Nombre = fila.Cell(1).GetString();
                            afiliado.Apellido = fila.Cell(2).GetString();
                            afiliado.DNI = fila.Cell(3).GetValue<long>();
                            afiliado.FechaNacimiento = DateOnly.FromDateTime(fila.Cell(4).GetDateTime());

                            listaAfiliados.Add(afiliado);
                        }

                        _context.Afiliados.AddRange(listaAfiliados);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return RedirectToAction("Index", "Afiliados");
        }


    }
}
