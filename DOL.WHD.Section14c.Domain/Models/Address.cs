﻿using System.ComponentModel.DataAnnotations;

namespace DOL.WHD.Section14c.Domain.Models
{
    public class Address : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string County { get; set; }
    }
}
