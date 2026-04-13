using HealthcareApp.BLL.DTOs;
using HealthcareApp.BLL.Services.Implementation;
using HealthcareApp.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApp.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _service;

        public PatientController(IPatientService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string searchTerm)
{
    var data = await _service.SearchAsync(searchTerm);
    return View(data);
}

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _service.GetByIdAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);

                if (result)
                {
                    TempData["Success"] = "Patient deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete patient";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientDTO dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _service.AddAsync(dto);

                    if (result)
                    {
                        TempData["Success"] = "Patient added successfully!";
                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError("", "Failed to add patient");
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Something went wrong!";
                return RedirectToAction("Index");
            }

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PatientDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }
                {
                    var result = await _service.UpdateAsync(dto);

                    if (result)
                    {
                        TempData["Success"] = "Patient updated successfully!";
                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError("", "Failed to update patient");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong");
            }

            return View(dto);
        }
    }
}