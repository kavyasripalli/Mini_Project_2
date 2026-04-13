using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HealthcareApp.BLL.DTOs
{
    public class PatientDTO
    {
        public int PatientId { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(1, 120)]
        public int Age { get; set; }

        [Required(ErrorMessage = "Please select gender")]
        public string Gender { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string? MedicalNotes { get; set; }
    }
}