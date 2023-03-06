using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Key { get; set; }
        [MaxLength(200)]
        public string Value { get; set; }
    }
}
