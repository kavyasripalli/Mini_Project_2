using System;
using System.Collections.Generic;
using System.Text;
using HealthcareApp.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace HealthcareApp.BLL.DTOs
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Please select a patient")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a patient")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please select a doctor")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a doctor")]
        public int DoctorId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please select time")]
        public TimeSpan TimeSlot { get; set; }

        public AppointmentStatus Status { get; set; }

        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
    }
}