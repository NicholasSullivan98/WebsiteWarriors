namespace CapstoneProject.Models
{
	public interface IDataSource
	{
		public IEnumerable<AppointmentInfo> Appointment { get; }
	}
}
