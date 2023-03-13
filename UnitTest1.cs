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
				Date = new System.DateTime(2023, 3, 21) ,
				Time = new System.DateTime(2023, 3, 21,15,30,0)
			};

			//Act
			String testName = a.Name;
			String testPhoneNumber = a.PhoneNumber;
			String testCourseLevel = a.CourseLevel;
			DateTime testDataTime = a.Date;
			DateTime testTime = a.Time;


			//Asssert
			Assert.Equal("Andy", testName);
			Assert.Equal("6479872984", testPhoneNumber);
			Assert.Equal("Beginner", testCourseLevel);
			Assert.Equal(new System.DateTime(2023, 3, 21), testDataTime);
			Assert.Equal(new System.DateTime(2023, 3, 21, 15, 30, 0), testTime);
		}
	}
}