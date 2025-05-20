using System.ComponentModel.DataAnnotations;

namespace ClinicaNet.Models
{
    public class Afiliado
    {
        public int Id { get; set; }

        [Required]
        public string? Nombre { get; set; }

        [Required]
        public string? Apellido { get; set; }

        [Required]
        public long? DNI { get; set; }

        [Required]
        [Display(Name = "Fecha de nacimiento")]
        public DateOnly FechaNacimiento { get; set; }
        
        [Display(Name = "Foto afiliado")]
        public string? NombreFoto { get; set; } //Guarda el nombre del archivo de la imagen

        public List<Ticket>? Tickets { get; set; } // Muchos Tickets pueden estar asociados a un mismo Afiliado.


        public string NombreCompleto => $"{Nombre} {Apellido}";

    }
}

/* Una persona puede hacer muchos reclamos distintos. Un solo Afiliado → muchos tickets distintos.

Marcos puede tener, esto se mostrara en el Index de Tickets:

- Ticket 101: "No puedo ingresar"

- Ticket 102: "No me anda el pago"

- Ticket 103: "Quiero cambiar el plan"
 
 */