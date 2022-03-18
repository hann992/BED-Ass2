using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

namespace BEDAssignment2.Models
{
    public class Model
    {
        public Model(string? firstName, string? lastName, string? email, string? phoneNo, string? addresLine1, string? addresLine2, string? zip, string? city, DateTime? birthDay, double? height, int? shoeSize, string? hairColor, string? comments)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNo = phoneNo;
            AddresLine1 = addresLine1;
            AddresLine2 = addresLine2;
            Zip = zip;
            City = city;
            BirthDay = birthDay;
            Height = height;
            ShoeSize = shoeSize;
            HairColor = hairColor;
            Comments = comments;
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

        public List<Job>? Jobs { get; set; }
        public List<Expense>? Expenses { get; set; }

        //public List<Job>? Jobs = new List<Job>();

        //public List<Expense>? Expenses = new List<Expense>();   
    }
}
