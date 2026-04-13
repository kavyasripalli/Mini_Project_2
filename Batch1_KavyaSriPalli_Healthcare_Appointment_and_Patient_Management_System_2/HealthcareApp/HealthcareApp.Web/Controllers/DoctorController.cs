using Microsoft.AspNetCore.Mvc;
using HealthcareApp.BLL.Services.Interfaces;
using HealthcareApp.BLL.DTOs;

namespace HealthcareApp.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        // ================== INDEX ==================
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        // ================== CREATE ==================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Validation failed!";
                    return View(dto);
                }

                await _service.AddAsync(dto);

                TempData["Success"] = "Doctor added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        // ================== EDIT ==================
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _service.GetByIdAsync(id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DoctorDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Validation failed!";
                    return View(dto);
                }

                await _service.UpdateAsync(dto);

                TempData["Success"] = "Doctor updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        // ================== DELETE ==================
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                TempData["Success"] = "Doctor deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}