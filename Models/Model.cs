using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BEDAssignment2.Models
{
    public class Model
    {
        public Model(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public long ModelId { get; set; }
        [MaxLength(64)]
        public string? FirstName { get; set; }
        [MaxLength(32)]
        public string? LastName { get; set; }
        [MaxLength(254)]
        public string? Email { get; set; }
        [MaxLength(12)]
        public string? PhoneNo { get; set; }
        [MaxLength(64)]
        public string? AddresLine1 { get; set; }
        [MaxLength(64)]
        public string? AddresLine2 { get; set; }
        [MaxLength(9)]
        public string? Zip { get; set; }
        [MaxLength(64)]
        public string? City { get; set; }
        [Column(TypeName = "date")]
        public DateTime? BirthDay { get; set; }
        public double? Height { get; set; }
        public int? ShoeSize { get; set; }
        [MaxLength(32)]
        public string? HairColor { get; set; }
        [MaxLength(1000)]
        public string? Comments { get; set; }

        //kan bruges til at ignore en property fuldstændig
        //[JsonIgnore]
        public List<Job>? Jobs { get; set; }
        //[JsonIgnore]
        public List<Expense>? Expenses { get; set; }
    }
}
