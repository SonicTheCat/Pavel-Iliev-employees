namespace PairOfEmployeesWeb.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PairOfEmployeesWeb.Models;
    using PairOfEmployeesWeb.Services;
    using PairOfEmployeesWeb.Utils;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        private readonly IFileReaderService<Employee> fileReaderService;
        private readonly IEmployeePairService employeePairService;
        private static Dictionary<int, EmployeePair>? pairs;

        public HomeController(IFileReaderService<Employee> fileReaderService, IEmployeePairService employeePairService)
        {
            this.fileReaderService = fileReaderService;
            this.employeePairService = employeePairService;
        }

        public IActionResult Index()
        {
            var model = new IndexHomeInputModel
            {
                Pairs = pairs
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(IndexHomeInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var employees = this.fileReaderService.Read(model.File.OpenReadStream());
                var allPairs = this.employeePairService.FindAllPairs(employees);
                pairs = this.employeePairService.FindBestPairs(allPairs);
            }
            catch (CsvReaderException ex)
            {
                this.ModelState.AddModelError("File", ex.Message);
                return this.View(model);
            }

            return this.RedirectToAction(nameof(this.Index), "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}