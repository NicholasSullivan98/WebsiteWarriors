using CapstoneProject.Models;
using CapstoneProject.Models.Account_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

using Azure;
using Azure.Communication.Email;

namespace CapstoneProject.Controllers
{
    public class HomeController : Controller
    {
        public IAppointmentRepository _appointmentRepository;
        public IAccountRepository _accountRepository;
        public IReviewRepository _reviewRepository;
        public IDataSource dataSource = new AppointmentDataSource();

        public static bool loggedIn { get; set; } = false;
        public static string loggedInPassword { get; set; } = string.Empty;
        public static string loggedInName { get; set; } = string.Empty;
        public static string loggedInEmail { get; set; } = string.Empty;
        public static int loggedInID { get; set; } = 0;

        public static string loggedInPhoneNum { get; set; } = string.Empty;


        [ActivatorUtilitiesConstructor]
		public HomeController(IAppointmentRepository appointmentRepository, IAccountRepository accountRepository, IReviewRepository reviewRepository)
        {
            _appointmentRepository = appointmentRepository;
            _accountRepository = accountRepository;
            _reviewRepository = reviewRepository;
        }

		[HttpGet]
        public IActionResult Reviews()
        {
            ViewBag.LoggedInName = loggedInName;
            ViewBag.LoggedInID = loggedInID;
            return View(new ManageReviewPageModel
            {
                Reviews = _reviewRepository.GetAllReviews
            });
        }

        [HttpGet]
        public IActionResult AddReview()
        {
            ViewBag.LoggedInName = loggedInName;
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
        public IActionResult AddReview(ReviewInformation ri)
        {
            if (ModelState.IsValid)
            {
                _reviewRepository.AddReview(ri);
                return RedirectToAction("Reviews");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult DeleteReview(int id)
        {
            _reviewRepository.DeleteReview(id);
            return RedirectToAction("Reviews");
        }

        [HttpGet]
        public IActionResult AddAppointment()
        {
            if (loggedIn == true)
            {
                ViewBag.Name = loggedInName;
                ViewBag.PhoneNumber = loggedInPhoneNum;
                ViewBag.Email = loggedInEmail;
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
                bool verifyPassword = false;

                IEnumerable<AccountInformation> loggedInUser = _accountRepository.GetAllAccounts.Where(r => r.Email == li.Email);

                foreach (AccountInformation account in loggedInUser)
                {
                    Debug.WriteLine("Email: " + account.Email);
                    Debug.WriteLine("Password: " + account.Password);
                    Debug.WriteLine("ID: " + account.AccountID);
                    verifyPassword = BCrypt.Net.BCrypt.Verify(li.Password, account.Password);
                    loggedInEmail = account.Email;
                    loggedInName = account.ParentFirstName + " " + account.ParentLastName;
                    loggedInID = account.AccountID;
                    loggedInPhoneNum = account.PhoneNumber;
                    Debug.WriteLine("ID in loggedInId: " + loggedInID);
                }

                if (loggedInEmail.IsNullOrEmpty() != true && verifyPassword == true)
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

            return View(new ManageReviewPageModel
            {
                Reviews = _reviewRepository.GetAllReviews.Where(r => r.ReviewID <= 3)
            });

        }

        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SendEmail()
        {

            if (loggedIn == true && loggedInName == "Michelle De Melo" && loggedInID == 1)
            {
                return View(new ManageAppointPageModel
                {
                    Appointments = _appointmentRepository.GetAllAppointments
                });
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
        public async Task<IActionResult> SendEmail(string r)
        {
            string connectionString = "endpoint=https://capstonecommunicationservice.canada.communication.azure.com/;accesskey=tdzMvNimn8bBnlCCB4zVFPpYs0yeWyB+zFnwioDhOFRoqv35k4ckD0OzUBvOy3QKvnaOutP7NDrBDKHtzLEcYA==";
            EmailClient emailClient = new EmailClient(connectionString);

            List<AppointmentInfo> accounts = _appointmentRepository.GetUnpaidAppointments().ToList();

            var recipient = string.Empty;
            var recipientName = string.Empty;
            var recipientStudentName = string.Empty;
            var appointmentDate = DateTime.Now.ToLongDateString();
            var appointmentTime = DateTime.Now.ToLongTimeString();

            for (var j = 0; j < accounts.Count(); j++)
            {

                AppointmentInfo appointment = accounts.ElementAt(j);
                
                recipient = appointment.Email;
                recipientName = appointment.ParentName;
                recipientStudentName = appointment.Name;
                appointmentDate = appointment.Date.ToLongDateString();
                appointmentTime = appointment.Time.ToLongTimeString();
                

            

                var subject = "Payment Reminder Email";
                var htmlContent = "<html>" +
                                    "<body>" +
                                        "<h1>Hello " + recipientName + ",</h1>" +
                                        "<br/><h4>This email is a reminder for you to pay for your up coming lesson for " + recipientStudentName + " at " + appointmentTime + " on " + appointmentDate + "</h4>" +
                                        "<p>This is an automated email please do not reply.</p>" +
                                        "<h1>Made By Me Studio </h1>" +
                                    "</body>" +
                                  "</html>";
                var sender = "MadeByMeStudioDoNotReply@30fb12fd-b2f0-4146-a977-bd128260e935.azurecomm.net";

                //var recipient = recipientsTest;

                Debug.WriteLine("Mail Recipient: " + recipient);
                try
                {
                    Debug.WriteLine("Sending email...");
                    EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                        Azure.WaitUntil.Completed,
                        sender,
                        recipient,
                        subject,
                        htmlContent);

                    EmailSendResult statusMonitor = emailSendOperation.Value;

                    Debug.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");
                    ViewBag.Result = "Email Sent.";

                    /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                    string operationId = emailSendOperation.Id;
                    Debug.WriteLine($"Email operation id = {operationId}");
                }
                catch (RequestFailedException ex)
                {
                    /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                    Debug.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
                    ViewBag.Result = "Email send failed.";
                }
            }
            return View(new ManageAppointPageModel
            {
                Appointments = _appointmentRepository.GetAllAppointments
            });
        }

        [HttpPost]
        public IActionResult SaveAppointment(AppointmentInfo appointment)
        {
            Debug.WriteLine("Made it in Save Appointment: " + appointment.AppointmentID);
            Debug.WriteLine("Name: " + appointment.Name);
            ViewBag.Response = "Saved";
            _appointmentRepository.UpdateAppointmentPayment(appointment.AppointmentID);
            return RedirectToAction("SendEmail");
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