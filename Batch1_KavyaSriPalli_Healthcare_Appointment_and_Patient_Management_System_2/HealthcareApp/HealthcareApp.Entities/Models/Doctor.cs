using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HealthcareApp.Entities.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        public string? Name { get; set; } = string.Empty;

        [Required]
        public string Specialization { get; set; } = string.Empty;

        [Required]
        public TimeSpan AvailableFrom { get; set; }

        [Required]
        public TimeSpan AvailableTo { get; set; }
    }
}