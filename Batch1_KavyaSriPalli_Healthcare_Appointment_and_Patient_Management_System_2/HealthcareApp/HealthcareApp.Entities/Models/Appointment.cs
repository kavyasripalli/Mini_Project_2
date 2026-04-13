using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareApp.Entities.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan TimeSlot { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }

        // Navigation Properties (VERY IMPORTANT)
        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor? Doctor { get; set; }
    }
}