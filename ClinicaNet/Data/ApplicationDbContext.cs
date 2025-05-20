using ClinicaNet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicaNet.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        base.OnModelCreating(modelBuilder);
    }


    /*Los DbSet<T> son necesarios para que EF Core sepa que una clase debe ser mapeada a una tabla en la BBDD. Él nombre de la propiedad va en 
     plural porque son los nombres de las tablas.*/

    public DbSet<Ticket> Tickets {get; set;}
    public DbSet<TicketDetalle> TicketsDetalles { get; set;}
    public DbSet<Estado> Estados {get; set;}
    public DbSet<Afiliado> Afiliados {get; set;}

}

/*
 DATOS:

- Cada entidad es una clase que representa una tabla en la base de datos.
- El modelo de datos es el conjunto de todas las entidades que EF Core usará para crear la base de datos y realizar operaciones sobre ella.
- DbContext es donde defines qué entidades (tablas) usas, y es el puente entre el modelo de datos y la base de datos.
- OnModelCreating (y ModelBuilder) se usan para personalizar cómo las entidades se mapean a las tablas y establecer relaciones entre ellas.
- Mapear significa vincular las entidades (clases) con las tablas en la base de datos, y las propiedades de esas clases (atributos o campos que 
  describen las características de una entidad) con las columnas de esas tablas, ya que cada propiedad se convierte en una columna en la BBDD.
 */