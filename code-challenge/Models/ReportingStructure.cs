namespace challenge.Models
{
    public class ReportingStructure
    {
        public ReportingStructure()
        {

        }

        public ReportingStructure(Employee employee, int numberOfReports)
        {
            Employee = employee ?? throw new System.ArgumentNullException(nameof(employee));
            NumberOfReports = numberOfReports;
        }

        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }

        public override string ToString()
        {
            return $"Number of Reports: {NumberOfReports}";
        }
    }
}