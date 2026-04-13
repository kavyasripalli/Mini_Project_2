using HealthcareApp.BLL.Services.Interfaces;
using HealthcareApp.Entities.Models;
using HealthcareApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HealthcareApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public HomeController(
            IPatientService patientService,
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }
        public async Task<IActionResult> Index()
        {
            // GET ALL DATA
            var patients = await _patientService.GetAllAsync();
            var doctors = await _doctorService.GetAllAsync();
            var appointments = await _appointmentService.GetAllAsync();

            // COUNTS (for cards)
            ViewBag.PatientCount = patients.Count();
            ViewBag.DoctorCount = doctors.Count();
            ViewBag.AppointmentCount = appointments.Count();

            // PIE CHART DATA
            ViewBag.BookedCount = appointments.Count(a => a.Status == AppointmentStatus.Booked);
            ViewBag.CompletedCount = appointments.Count(a => a.Status == AppointmentStatus.Completed);
            ViewBag.CancelledCount = appointments.Count(a => a.Status == AppointmentStatus.Cancelled);

            // TODAY UPCOMING (ONLY BOOKED)
            var today = DateTime.Now.Date;

            var todayAppointments = appointments
                .Where(a => a.Date.Date == today &&
                            a.TimeSlot >= DateTime.Now.TimeOfDay &&
                            a.Status == AppointmentStatus.Booked)
                .OrderBy(a => a.TimeSlot)
                .ToList();

            ViewBag.TodayAppointments = todayAppointments;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
