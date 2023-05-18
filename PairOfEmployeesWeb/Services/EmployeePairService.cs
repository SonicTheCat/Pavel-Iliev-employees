namespace PairOfEmployeesWeb.Services
{
    using PairOfEmployeesWeb.Models;

    public class EmployeePairService : IEmployeePairService
    {
        public Dictionary<int, List<EmployeePair>> FindAllPairs(IEnumerable<Employee> employees)
        {
            var result = new Dictionary<int, List<EmployeePair>>();
            var groupsByProject = employees.GroupBy(x => x.ProjectId);

            foreach (var group in groupsByProject)
            {
                var currentGroup = group.ToList();
                var currentKey = group.Key;

                result.Add(currentKey, new List<EmployeePair>());

                for (int i = 0; i < currentGroup.Count - 1; i++)
                {
                    var currentEmployee = currentGroup[i];
                    var nextEmployee = currentGroup[i + 1];

                    if (HasOverlapingDates(currentEmployee, nextEmployee))
                    {
                        AddPairToResult(result, currentKey, currentEmployee, nextEmployee);
                    }
                }
            }

            return result;
        }

        public Dictionary<int, EmployeePair> FindBestPairs(Dictionary<int, List<EmployeePair>> allPairs)
        {
            var result = new Dictionary<int, EmployeePair>(); 

            foreach (var pair in allPairs)
            {
                var longestPair = pair.Value.OrderByDescending(x => x.DaysWorked).FirstOrDefault();
                if (longestPair == null) continue;
                result.Add(pair.Key, longestPair);
            }

            return result;
        }

        private bool HasOverlapingDates(Employee currentEmployee, Employee nextEmployee)
        {
            return (currentEmployee.DateFrom <= nextEmployee.DateTo) &&
                   (currentEmployee.DateTo >= nextEmployee.DateFrom);
        }

        private void AddPairToResult(Dictionary<int, List<EmployeePair>> result, int currentKey, Employee currentEmployee, Employee nextEmployee)
        {
            var pair = new EmployeePair
            {
                FirstEmployeeId = currentEmployee.Id,
                SecondEmployeeId = nextEmployee.Id,
                DaysWorked = this.FindOutDaysWorked(currentEmployee, nextEmployee)
            };

            result[currentKey].Add(pair);
        }

        private int FindOutDaysWorked(Employee currentEmployee, Employee nextEmployee)
        {
            var maxDateFrom = Math.Max(currentEmployee.DateFrom.DayNumber, nextEmployee.DateFrom.DayNumber);
            var minDateTo = Math.Min(currentEmployee.DateTo.DayNumber, nextEmployee.DateTo.DayNumber);

            return minDateTo - maxDateFrom;
        }
    }
}