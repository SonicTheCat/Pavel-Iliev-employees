namespace PairOfEmployeesWeb.Services
{
    using PairOfEmployeesWeb.Models;

    public interface IEmployeePairService
    {
        Dictionary<int, List<EmployeePair>> FindAllPairs(IEnumerable<Employee> employees);

        public Dictionary<int, EmployeePair> FindBestPairs(Dictionary<int, List<EmployeePair>> allPairs);
    }
}