using CapstoneProject.Models;

namespace CapstoneProjectTesting
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			//Arrange
			AppointmentInfo a = new AppointmentInfo
			{
				Name = "Andy",
				PhoneNumber = "6479872984",
				CourseLevel = "Beginner",
				// Date = new DateTime(2023, 6, 31),
				//Time = new DateTime(2023, 6, 31, 5, 10)
			};

			//Act
			String testName = a.Name;
			String testPhoneNumber = a.PhoneNumber;
			String testCourseLevel = a.CourseLevel;
			//DateTime testDataTime = a.Date;
			//DateTime testTime = a.Time;


			//Asssert
			Assert.Equal("Andy", testName);
			Assert.Equal("6479872984", testPhoneNumber);
			Assert.Equal("Beginner", testCourseLevel);
			//Assert.Equal(a.Date, testDataTime);
			//Assert.Equal(a.Time, testTime);
		}
	}
}