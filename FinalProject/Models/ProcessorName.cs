using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class ProcessorName
    {
        public int Id { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
        public List<Phone>? Phones { get; set; }

    }
}
