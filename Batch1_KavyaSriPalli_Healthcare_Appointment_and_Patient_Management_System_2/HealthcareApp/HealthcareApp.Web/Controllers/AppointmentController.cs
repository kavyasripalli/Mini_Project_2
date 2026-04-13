using Microsoft.AspNetCore.Mvc;
using HealthcareApp.BLL.Services.Interfaces;
using HealthcareApp.BLL.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HealthcareApp.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(
            IAppointmentService appointmentService,
            IPatientService patientService,
            IDoctorService doctorService,
            ILogger<AppointmentController> logger)   
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
            _logger = logger;   
        }

        public async Task<IActionResult> Index(DateTime? date, string status, string search, int page = 1)
        {
            _logger.LogInformation("Fetching appointment list");
            int pageSize = 5;

            if (date.HasValue || !string.IsNullOrEmpty(status))
            {
                pageSize = int.MaxValue;
                page = 1;
            }

            var allData = (await _appointmentService.GetAllAsync()).ToList();

            if (date.HasValue)
                allData = allData
                    .Where(a => a.Date.Date == date.Value.Date)
                    .ToList();

            if (!string.IsNullOrEmpty(status))
                allData = allData
                    .Where(a => a.Status.ToString() == status)
                    .ToList();

            if (!string.IsNullOrEmpty(search))
            {
                allData = allData
                    .Where(a => a.PatientName.ToLower().Contains(search.ToLower())
                             || a.DoctorName.ToLower().Contains(search.ToLower()))
                    .ToList();
            }

            // FOR TABLE (paginated)
            var totalCount = allData.Count();
            _logger.LogInformation($"Total records after filtering: {totalCount}");

            var pagedData = allData
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // SEND BOTH
            ViewBag.AllAppointments = allData; // for cards
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(pagedData);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Patients = new SelectList(await _patientService.GetAllAsync(), "PatientId", "Name");
            ViewBag.Doctors = new SelectList(await _doctorService.GetAllAsync(), "DoctorId", "Name");

            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _appointmentService.GetByIdAsync(id);

            ViewBag.Patients = new SelectList(await _patientService.GetAllAsync(), "PatientId", "Name");
            ViewBag.Doctors = new SelectList(await _doctorService.GetAllAsync(), "DoctorId", "Name");

            return View(data);
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting appointment ID: {id}");
            var success = await _appointmentService.DeleteAsync(id);

            if (!success)
            {
                _logger.LogWarning("Delete failed: Completed appointment cannot be deleted");
                TempData["Error"] = "Cannot delete completed appointment!";
            }
            else
            {
                _logger.LogInformation("Appointment deleted successfully");
                TempData["Success"] = "Appointment deleted!";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentDTO dto)
        {
            _logger.LogInformation("Create appointment started");
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Validation failed!";

                    ViewBag.Patients = new SelectList(await _patientService.GetAllAsync(), "PatientId", "Name");
                    ViewBag.Doctors = new SelectList(await _doctorService.GetAllAsync(), "DoctorId", "Name");

                    return View(dto);
                }

                var success = await _appointmentService.BookAppointmentAsync(dto);

                if (!success)
                {
                    _logger.LogWarning("Booking failed: Slot already taken");

                    ModelState.AddModelError("", "This slot is already booked!");

                    ViewBag.Patients = new SelectList(await _patientService.GetAllAsync(), "PatientId", "Name");
                    ViewBag.Doctors = new SelectList(await _doctorService.GetAllAsync(), "DoctorId", "Name");

                    return View(dto);
                }
                _logger.LogInformation("Appointment booked successfully");

                TempData["Success"] = "Appointment booked successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating appointment");
                ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);

                ViewBag.Patients = new SelectList(await _patientService.GetAllAsync(), "PatientId", "Name");
                ViewBag.Doctors = new SelectList(await _doctorService.GetAllAsync(), "DoctorId", "Name");

                return View(dto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppointmentDTO dto)
        {
            _logger.LogInformation($"Editing appointment ID: {dto.AppointmentId}");
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Validation failed!";

                    ViewBag.Patients = new SelectList(await _patientService.GetAllAsync(), "PatientId", "Name");
                    ViewBag.Doctors = new SelectList(await _doctorService.GetAllAsync(), "DoctorId", "Name");

                    return View(dto);
                }

                var success = await _appointmentService.UpdateAsync(dto);
                
                if (!success)
                {
                    _logger.LogWarning("Edit failed: Completed appointment cannot be modified");

                    ModelState.AddModelError("", "Completed appointments cannot be modified!");

                    ViewBag.Patients = new SelectList(await _patientService.GetAllAsync(), "PatientId", "Name");
                    ViewBag.Doctors = new SelectList(await _doctorService.GetAllAsync(), "DoctorId", "Name");

                    return View(dto);
                }
                _logger.LogInformation("Appointment updated successfully");

                TempData["Success"] = "Appointment updated!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while editing appointment");
                ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message); //FIX

                ViewBag.Patients = new SelectList(await _patientService.GetAllAsync(), "PatientId", "Name");
                ViewBag.Doctors = new SelectList(await _doctorService.GetAllAsync(), "DoctorId", "Name");

                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableSlots(int doctorId, DateTime date, int? appointmentId)
        {
            _logger.LogInformation($"Fetching slots for DoctorId: {doctorId} on {date}");
            var doctor = await _doctorService.GetByIdAsync(doctorId);

            if (doctor == null)
                return Json(new List<string>());

            var appointments = await _appointmentService.GetAllAsync();

            var bookedSlots = appointments
                .Where(a => a.DoctorId == doctorId
                         && a.Date.Date == date.Date
                         && a.AppointmentId != appointmentId) 
                .Select(a => a.TimeSlot)
                .ToList();

            List<object> slots = new List<object>();

            var start = doctor.AvailableFrom;
            var end = doctor.AvailableTo;

            for (var time = start; time < end; time = time.Add(TimeSpan.FromMinutes(15)))
            {
                bool isBooked = bookedSlots.Any(b =>
                    Math.Abs((b - time).TotalMinutes) < 15
                );

                slots.Add(new
                {
                    time = time.ToString(@"hh\:mm"),
                    isBooked = isBooked
                });
            }

            return Json(slots);
        }
    }
}