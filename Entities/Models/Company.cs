using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Company
    {
        [Column("CompanyId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Company must have a name")]
        [MaxLength(60,ErrorMessage ="Max length for the name is 60 char")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Company adress is a required field")]
        [MaxLength(60, ErrorMessage = "Max length for the adress is 60 char")]
        public string? Adress { get; set; }
        public string? Country { get; set; }
        // les propriété navigationel utilise ICollection et pas des listes (trop rigide)
        public ICollection<Employee>? Employees { get; set; }

    }
}
