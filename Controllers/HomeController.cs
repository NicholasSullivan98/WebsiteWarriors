﻿using CapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CapstoneProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
                return View();
            } 
            else 
            {
                ViewBag.Error = "Failed to Create Appointment";
                return View();
            }
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
    }
}