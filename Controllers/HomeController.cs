using CapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CapstoneProject.Controllers
{
    public class HomeController : Controller
    {
        public IAppointmentRepository _appointmentRepository;
        public IDataSource dataSource = new AppointmentDataSource();

		[ActivatorUtilitiesConstructor]
		public HomeController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

		public HomeController()
		{
		}

		[HttpGet]
        public IActionResult Reviews()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddReview()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddAppointment()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult AddAppointment(AppointmentInfo ai)
        {
            if (ModelState.IsValid)
            {
                Debug.WriteLine("Name: " + ai.Name);
                Debug.WriteLine("Phone Number: " + ai.PhoneNumber);
                Debug.WriteLine("Course Level: " + ai.CourseLevel);
                Debug.WriteLine("Time: " + ai.Time.ToShortTimeString());
                Debug.WriteLine("Date: " + ai.Date.ToLongDateString());
                ViewBag.Response = "Appointment Created!";
                _appointmentRepository.AddAppointment(ai);
                return View();
            } 
            else 
            {
                ViewBag.Error = "Failed to Create Appointment";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ManageAppointments()
        {
            return View(new ManageAppointPageModel
            {
                Appointments = _appointmentRepository.GetAllAppointments
            });
        }

        [HttpGet]
        public IActionResult EditAppointment(int id)
        {
            AppointmentInfo appointment = _appointmentRepository.GetAppointment(id);
            return View(appointment);
        }

        [HttpPost]
        public IActionResult EditAppointment(AppointmentInfo ai, int id)
        {
            if (ModelState.IsValid)
            {
                Debug.WriteLine("Name: " + ai.Name);
                Debug.WriteLine("Phone Number: " + ai.PhoneNumber);
                Debug.WriteLine("Course Level: " + ai.CourseLevel);
                Debug.WriteLine("Time: " + ai.Time.ToShortTimeString());
                Debug.WriteLine("Date: " + ai.Date.ToLongDateString());
                ViewBag.Response = "Appointment Updated!";
                _appointmentRepository.UpdateAppointment(ai, id);
                return RedirectToAction("ManageAppointments");
            }
            else
            {
                ViewBag.UpdateError = "Failed to Update Appointment";
                AppointmentInfo appointment = _appointmentRepository.GetAppointment(id);
                return View(appointment);
            }
        }

        [HttpGet]
        public IActionResult DeleteAppointment(int id)
        {
            _appointmentRepository.DeleteAppointment(id);
            return RedirectToAction("ManageAppointments");
        }

        public IActionResult Index()
        {
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

		[HttpPost]
		public IActionResult testingAppointmentInfo()
		{
			return View(dataSource.Appointment);

		}
	}
}