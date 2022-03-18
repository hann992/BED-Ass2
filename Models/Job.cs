using System.ComponentModel.DataAnnotations;

namespace BEDAssignment2.Models
{
    public class Job
    {
        public Job(string? customer, DateTimeOffset? startDate, int? days, string? location, string? comments)
        {
            Customer = customer;
            StartDate = startDate;
            Days = days;
            Location = location;
            Comments = comments;
        }

        public long JobId { get; set; }
        [MaxLength(64)] public string? Customer { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public int? Days { get; set; }
        [MaxLength(128)] public string? Location { get; set; }
        [MaxLength(2000)] public string? Comments { get; set; }
        public List<Model>? Models { get; set; }
        public List<Expense>? Expenses { get; set; }
    }

    public class JobsWithoutIDWithoutExpensesWithoutModels
    {
        public JobsWithoutIDWithoutExpensesWithoutModels(string? customer, DateTimeOffset? startDate, int? days, string? location, string? comments)
        {
            Customer = customer;
            StartDate = startDate;
            Days = days;
            Location = location;
            Comments = comments;

        }
        public string? Customer { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public int? Days { get; set; }

        public string? Location { get; set; }

        public string? Comments { get; set; }
    }

    public class JobsWithoutExpensesWithoutModels
    {
        public JobsWithoutExpensesWithoutModels(string? customer, DateTimeOffset? startDate, int? days, string? location, string? comments)
        {
            Customer = customer;
            StartDate = startDate;
            Days = days;
            Location = location;
            Comments = comments;

        }
        public long JobId { get; set; }
        public string? Customer { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public int? Days { get; set; }

        public string? Location { get; set; }

        public string? Comments { get; set; }
    }
}
