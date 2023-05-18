namespace PairOfEmployeesWeb.Utils
{
    public class CsvMapperAttribute : Attribute
    {
        public int Order { get; set; }

        public bool CanBeSendAsNull { get; set; }
    }
}