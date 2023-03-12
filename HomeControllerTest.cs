using CapstoneProject.Controllers;
using CapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProjectTesting
{
	public class HomeControllerTest
	{

		[Fact]
		public void AddAppointmentTest()
		{
			//Arrange
			var controller = new HomeController();

			AppointmentInfo[] testData = new AppointmentInfo[]
			{
				new AppointmentInfo{Name="Andy", 
					PhoneNumber="6479872984",
					CourseLevel="Beginner",
					//Date=new DateTime(2023, 6, 31),
					//Time=new DateTime(2023, 6, 31, 5, 10, 20)
				},
			};

			var mock = new Mock<IDataSource>();
			mock.SetupGet(m => m.Appointment).Returns(testData);
			controller.dataSource = mock.Object;
			//Act
			var model = (controller.AddAppointment() as ViewResult)?.ViewData.Model as IEnumerable<AppointmentInfo>;

			//assert
			Assert.Equal(testData, model, Comparer.Get<AppointmentInfo>((a1, a2) => a1?.Name == a2?.Name &&
										a1?.PhoneNumber == a2?.PhoneNumber && 
										a1?.CourseLevel == a2?.CourseLevel));
			mock.VerifyGet(m => m.Appointment, Times.Once());

		}
	}
}
