using CapstoneProject.Models;
using CapstoneProject.Models.Account_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

using Azure;
using Azure.Communication.Email;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using System.Xml.Linq;

namespace CapstoneProject.Controllers
{
    public class HomeController : Controller
    {
        public IAppointmentRepository _appointmentRepository;
        public IAccountRepository _accountRepository;
        public IReviewRepository _reviewRepository;
        public IConfirmationRepository _confirmationRepository;
        public IStudentRepository _studentRepository;
        public IDataSource dataSource = new AppointmentDataSource();

        public static bool loggedIn { get; set; } = false;
        public static string loggedInPassword { get; set; } = string.Empty;
        public static string loggedInName { get; set; } = string.Empty;
        public static string loggedInEmail { get; set; } = string.Empty;
        public static int loggedInID { get; set; } = 0;
        public static string loggedInPhoneNum { get; set; } = string.Empty;

        // For Email System
        public static string sender { get; set; } = string.Empty;
        public static string recipient { get; set; } = string.Empty;
        public static string subject { get; set; } = string.Empty;
        public static string htmlContent { get; set; } = string.Empty;


        [ActivatorUtilitiesConstructor]
		public HomeController(IAppointmentRepository appointmentRepository, IAccountRepository accountRepository, IReviewRepository reviewRepository, IConfirmationRepository confirmationRepository, IStudentRepository studentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _accountRepository = accountRepository;
            _reviewRepository = reviewRepository;
            _confirmationRepository = confirmationRepository;
            _studentRepository = studentRepository;
        }

		[HttpGet]
        public IActionResult Reviews()
        {
            ViewBag.LoggedInName = loggedInName;
            ViewBag.LoggedIn = loggedIn;
            ViewBag.LoggedInID = loggedInID;
            return View(new ManageReviewPageModel
            {
                Reviews = _reviewRepository.GetAllReviews
            });
        }

        [HttpGet]
        public IActionResult AddReview()
        {
            ViewBag.LoggedIn = loggedIn;
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
            ViewBag.LoggedInName = loggedInName;
            ViewBag.LoggedIn = loggedIn;
            if (loggedIn == true)
            {
                ViewBag.Name = loggedInName;
                ViewBag.PhoneNumber = loggedInPhoneNum;
                ViewBag.Email = loggedInEmail;
                return View(new AddAppointPageModel
                {
                    Accounts = _accountRepository.GetLoggedInAccountInfo(loggedInID),
                    Students = _studentRepository.GetAllStudents,
                    Appointments = _appointmentRepository.GetAllAppointments
                });
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
                return View(new AddAppointPageModel
                {
                    Accounts = _accountRepository.GetLoggedInAccountInfo(loggedInID),
                    Students = _studentRepository.GetAllStudents,
                    Appointments = _appointmentRepository.GetAllAppointments
                });
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
            ViewBag.LoggedInName = loggedInName;
            ViewBag.LoggedIn = loggedIn;
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
            ViewBag.LoggedInName = loggedInName;
            ViewBag.LoggedIn = loggedIn;
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

        [HttpGet]
        public IActionResult EmailConfirmed(int id)
        {
            //ViewBag.ID = id;

            var accountToVerify = _confirmationRepository.GetAllConfirmations.FirstOrDefault(r => r.ConfirmationID == id) ?? null;

            if (accountToVerify != null)
            {

                StudentInformation newStudent = new StudentInformation();
                AccountInformation confirmedUser = new AccountInformation();
                
                newStudent.StudentFirstName = accountToVerify.StudentFirstName;
                newStudent.StudentLastName = accountToVerify.StudentLastName;
                
                
                //_studentRepository.AddStudent(newStudent);

                var getStudentID = _studentRepository.GetAllStudents.FirstOrDefault(r => r.StudentFirstName == newStudent.StudentFirstName);

                confirmedUser.ParentFirstName = accountToVerify.ParentFirstName;
                confirmedUser.ParentLastName = accountToVerify.ParentLastName;
                //confirmedUser.ParentAccountID = getStudentID.ParentAccountID;
                //confirmedUser.StudentInformation.StudentFirstName = accountToVerify.StudentFirstName;
                //confirmedUser.StudentInformation.StudentLastName = accountToVerify.StudentLastName;
                confirmedUser.Email = accountToVerify.Email;
                confirmedUser.PhoneNumber = accountToVerify.PhoneNumber;
                confirmedUser.Password = accountToVerify.Password;
                confirmedUser.PasswordConformation = accountToVerify.PasswordConformation;

                _accountRepository.AddUser(confirmedUser);

                var getNewAccountID = _accountRepository.GetAllAccounts.FirstOrDefault(r => r.ParentFirstName == confirmedUser.ParentFirstName && r.ParentLastName == confirmedUser.ParentLastName && r.Email == confirmedUser.Email);

                newStudent.ParentAccountID = getNewAccountID.AccountID;

                //Debug.WriteLine("ParentAccountID: " +  newStudent.ParentAccountID);

                _studentRepository.AddStudent(newStudent);

                //confirmedUser.ParentAccountID = newStudent.ParentAccountID;

                //_accountRepository.UpdateUser(confirmedUser, getNewAccountID.AccountID);

                _confirmationRepository.DeleteConfirmation(id);
            }
            else
            {
                ViewBag.Result = "Account has already been verified. Please sign in.";
            }

            return View();
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            //SendConfirmationEmail(sender, recipient, subject, htmlContent);
            return View();
        }

        public async Task<IActionResult> CallConfirmationEmailSend()
        {
            Debug.WriteLine("Made it in CallConfirm");
            SendConfirmationEmail();
            TempData["Message"] = "Email re-sent";
            return RedirectToAction("VerifyEmail");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(EmailConfirmationInformation ei)
        {
            if (ModelState.IsValid)
            {
                AccountInformation loggedInUser = new AccountInformation();
                var noDuplicate = false;

                loggedInUser = _accountRepository.GetAllAccounts.FirstOrDefault(r => r.Email == ei.Email) ?? null;
              
                Debug.WriteLine("No Duplicate Value: " + noDuplicate);

                if (loggedInUser == null)
                {
                    //_accountRepository.AddUser(ai);

                    //EmailConfirmationInformation ei = new EmailConfirmationInformation();
                    //ei.Email = ai.Email;
                    _confirmationRepository.AddConfirmation(ei);

                    EmailConfirmationInformation newConfrimation = _confirmationRepository.GetAllConfirmations.FirstOrDefault(r => r.Email == ei.Email);

                    //Send Email
                    /*
                    string connectionString = "endpoint=https://capstonecommunicationservice.canada.communication.azure.com/;accesskey=tdzMvNimn8bBnlCCB4zVFPpYs0yeWyB+zFnwioDhOFRoqv35k4ckD0OzUBvOy3QKvnaOutP7NDrBDKHtzLEcYA==";
                    EmailClient emailClient = new EmailClient(connectionString);
                    */
                    var host = Request.Host.Host;
                    var port = Request.Host.Port;
                    var scheme = Request.Scheme;

                    Debug.WriteLine("Host: " + host + " Port: " + port + " Scheme: " + scheme);

                    var url = scheme + "://" + host + ":" + port + "/Home/EmailConfirmed/" + newConfrimation.ConfirmationID;

                    recipient = newConfrimation.Email;
                    var recipientName = ei.ParentFirstName + " " + ei.ParentLastName;

                    subject = "Confirmation Email";
                    htmlContent = "<html>" +
                                        "<body>" +
                                            "<h1>Hello " + recipientName + ",</h1>" +
                                            "<br/><p>This email was sent as a confirmation email for you to verify your account for Made By Me Studio</p>" +
                                            "<a href ='" + url + "'>" + url + "</a>" +
                                            "<p>This is an automated email please do not reply.</p>" +
                                            "<h1>Made By Me Studio </h1>" +
                                        "</body>" +
                                      "</html>";
                    sender = "MadeByMeStudioDoNotReply@30fb12fd-b2f0-4146-a977-bd128260e935.azurecomm.net";

                    SendConfirmationEmail();

                    return RedirectToAction("VerifyEmail");
                }
                else
                {
                    ViewBag.Result = "Account with that email already exists";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public async void SendConfirmationEmail()
        {
            string connectionString = "endpoint=https://capstonecommunicationservice.canada.communication.azure.com/;accesskey=tdzMvNimn8bBnlCCB4zVFPpYs0yeWyB+zFnwioDhOFRoqv35k4ckD0OzUBvOy3QKvnaOutP7NDrBDKHtzLEcYA==";
            EmailClient emailClient = new EmailClient(connectionString);

            Debug.WriteLine("Mail Recipient: " + recipient);
            try
            {
                Debug.WriteLine("Sending email...");
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                    Azure.WaitUntil.Completed,
                    sender,
                    recipient,
                    subject,
                    htmlContent
                );

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
            ViewBag.LoggedInName = loggedInName;
            ViewBag.LoggedIn = loggedIn;
            return View(new ManageAccountPageModel
            {
                Accounts = _accountRepository.GetLoggedInAccountInfo(loggedInID),
                Students = _studentRepository.GetAllStudents
            });
        }

        public IActionResult Services()
        {
            ViewBag.LoggedInName = loggedInName;
            ViewBag.LoggedIn = loggedIn;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SendEmail()
        {
            ViewBag.LoggedInName = loggedInName;
            ViewBag.LoggedIn = loggedIn;
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

        [HttpGet]
        public IActionResult ResetPassword(int id)
        {
            ViewBag.LoggedInName = loggedInName;
            ViewBag.LoggedIn = loggedIn;
            AccountInformation account = _accountRepository.GetLoggedInAccountInfo(id);

            return View(account);
        }

        [HttpPost]
        public IActionResult ResetPassword(AccountInformation ai, int id)
        {

            Debug.WriteLine("Old Password: " + ai.OldPassword);
            Debug.WriteLine("");

            AccountInformation loggedInUser = _accountRepository.GetAllAccounts.FirstOrDefault(r => r.Email == ai.Email);
            Debug.WriteLine("Old Password in Database: " + loggedInUser.Password);

            bool verifyPassword = false;
            if (ModelState.IsValid)
            {
                verifyPassword = BCrypt.Net.BCrypt.Verify(ai.OldPassword, loggedInUser.Password);
                if (verifyPassword == true)
                {
                    _accountRepository.UpdateUser(ai, id);
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Result = "Old Password was Incorrect";
                    AccountInformation account = _accountRepository.GetLoggedInAccountInfo(id);

                    return View(account);
                }

            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordEmail(string r)
        {
            string connectionString = "endpoint=https://capstonecommunicationservice.canada.communication.azure.com/;accesskey=tdzMvNimn8bBnlCCB4zVFPpYs0yeWyB+zFnwioDhOFRoqv35k4ckD0OzUBvOy3QKvnaOutP7NDrBDKHtzLEcYA==";
            EmailClient emailClient = new EmailClient(connectionString);

            var recipient = loggedInEmail;
            var recipientName = loggedInName;

            //var scheme = Request.Url.Scheme;
            var host = Request.Host.Host;
            var port = Request.Host.Port;
            var scheme = Request.Scheme;

            Debug.WriteLine("Host: " + host + " Port: " + port + " Scheme: " + scheme);

            var url = scheme + "://" + host + ":" + port + "/Home/ResetPassword/" + loggedInID;

            var subject = "Reset Password";
            var htmlContent = "<html>" +
                                "<body>" +
                                    "<h1>Hello " + recipientName + ",</h1>" +
                                    "<p>This is a test reset password email.</p>" +
                                    "<a href ='" + url + "'>" + url + "</a>" +
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
            
            return RedirectToAction("Account");
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

        [HttpGet]
        public IActionResult AddChild() 
        {
            ViewBag.LoggedInName = loggedInName;
            ViewBag.LoggedIn = loggedIn;
            TempData["AccountID"] = loggedInID;
            return View();
        }

        [HttpPost]
        public IActionResult AddChild(StudentInformation si)
        {
            _studentRepository.AddStudent(si);
            return RedirectToAction("Account");
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