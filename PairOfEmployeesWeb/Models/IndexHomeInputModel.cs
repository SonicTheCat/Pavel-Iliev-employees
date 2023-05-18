namespace PairOfEmployeesWeb.Models
{
    using System.ComponentModel.DataAnnotations;

    public class IndexHomeInputModel
    {
        [Required]
        // [FileExtensions(Extensions = "csv")]
        public IFormFile File { get; set; }

        public Dictionary<int,EmployeePair>? Pairs { get; set; }
    }
}