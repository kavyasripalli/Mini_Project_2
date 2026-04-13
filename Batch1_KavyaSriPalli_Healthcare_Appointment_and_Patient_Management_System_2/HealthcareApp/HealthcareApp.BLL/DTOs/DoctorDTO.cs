using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthcareApp.BLL.DTOs
{
    public class DoctorDTO
    {
        public int DoctorId { get; set; }
        [Required(ErrorMessage = "Doctor name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "Available From is required")]
        public TimeSpan AvailableFrom { get; set; }

        [Required(ErrorMessage = "Available To is required")]
        public TimeSpan AvailableTo { get; set; }
    }
}
