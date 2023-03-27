using CapstoneProject.Models;
using CapstoneProject.Models.Account_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace CapstoneProject.Controllers
{
    public class HomeController : Controller
    {
        public IAppointmentRepository _appointmentRepository;
        public IAccountRepository _accountRepository;
        public IDataSource dataSource = new AppointmentDataSource();

        public static bool loggedIn { get; set; } = false;
        public static string loggedInPassword { get; set; } = string.Empty;
        public static string loggedInName { get; set; } = string.Empty;
        public static string loggedInEmail { get; set; } = string.Empty;
        public static int loggedInID { get; set; } = 0;


        [ActivatorUtilitiesConstructor]
		public HomeController(IAppointmentRepository appointmentRepository, IAccountRepository accountRepository)
        {
            _appointmentRepository = appointmentRepository;
            _accountRepository = accountRepository;
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
            if (loggedIn == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
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
            if (loggedIn == true && loggedInName == "Michelle De Melo" && loggedInID == 1)
            {
                return View(new ManageAppointPageModel
                {
                    Appointments = _appointmentRepository.GetAllAppointments
                });
            }
            else if(loggedIn == true){
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult EditAppointment(int id)
        {
            if (loggedIn == true && loggedInName == "Michelle De Melo" && loggedInID == 1)
            {
                AppointmentInfo appointment = _appointmentRepository.GetAppointment(id);
                return View(appointment);
            }
            else if (loggedIn == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login");
            }
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

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(AccountInformation ai)
        {
            if (ModelState.IsValid)
            {
                _accountRepository.AddUser(ai);
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            loggedIn = false;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginInformation li)
        {
            if (ModelState.IsValid)
            {
                loggedInEmail = " ";
                loggedInPassword = " ";
                loggedInID = 0;

                IEnumerable<AccountInformation> loggedInUser = _accountRepository.GetAllAccounts.Where(r => r.Email == li.Email && r.Password == li.Password);

                foreach (AccountInformation account in loggedInUser)
                {
                    Debug.WriteLine("Email: " + account.Email);
                    Debug.WriteLine("Password: " + account.Password);
                    loggedInEmail = account.Email;
                    loggedInPassword = account.Password;
                    loggedInName = account.ParentFirstName + " " + account.ParentLastName;
                    loggedInID = account.AccountID;
                }

                if (loggedInEmail != " " && loggedInPassword != " " && loggedInEmail != null && loggedInEmail.IsNullOrEmpty() != true)
                {
                    Debug.WriteLine("Made it in if");
                    loggedIn = true;
                    ViewBag.LoggedIn = true;
                    ViewBag.LoggedInName = loggedInName;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Given Email or Password is incorrect";
                    return View();
                }

            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            loggedIn = false;
            loggedInEmail = string.Empty;
            loggedInName = string.Empty;
            loggedInPassword = string.Empty;
            loggedInID = 0;
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            if (loggedIn != false)
            {
                ViewBag.LoggedIn = true;
                ViewBag.LoggedInName = loggedInName;
            }
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