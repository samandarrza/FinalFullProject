﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Team
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(80)]
        public string? Image { get; set; }
        [MaxLength(30)]
        public string Position { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
