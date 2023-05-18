namespace PairOfEmployeesWeb.Models
{
    using PairOfEmployeesWeb.Utils;

    public class Employee
    {
        [CsvMapper(Order = 0)]
        public int Id { get; set; }

        [CsvMapper(Order = 1)]
        public int ProjectId { get; set; }

        [CsvMapper(Order = 2)]
        public DateOnly DateFrom { get; set; }

        [CsvMapper(Order = 3, CanBeSendAsNull = true)]
        public DateOnly DateTo { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public override string ToString()
        {
            return
                $"Id: {this.Id}, " +
                $"ProjectId: {this.ProjectId}, " +
                $"From: {this.DateFrom}, " +
                $"To: {this.DateTo}";
        }
    }
}