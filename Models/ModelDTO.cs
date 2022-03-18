namespace BEDAssignment2.Models
{
    public class ModelWithoutIdWithoutExpensesWithoutJobs
    {
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }
        
        public string? Email { get; set; }
        
        public string? PhoneNo { get; set; }
        
        public string? AddresLine1 { get; set; }
        
        public string? AddresLine2 { get; set; }
        
        public string? Zip { get; set; }
        
        public string? City { get; set; }
        
        public DateTime? BirthDay { get; set; }
        public double? Height { get; set; }
        public int? ShoeSize { get; set; }
        
        public string? HairColor { get; set; }
        
        public string? Comments { get; set; }
    }

    public class ModelWithoutExpensesWithoutJobs
    {
        public ModelWithoutExpensesWithoutJobs(long modelId, string? firstName, string? lastName, string? email, string? phoneNo, string? addresLine1, string? addresLine2, string? zip, string? city, DateTime? birthDay, double? height, int? shoeSize, string? hairColor, string? comments)
        {
            ModelId = modelId;
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

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNo { get; set; }

        public string? AddresLine1 { get; set; }

        public string? AddresLine2 { get; set; }

        public string? Zip { get; set; }

        public string? City { get; set; }

        public DateTime? BirthDay { get; set; }
        public double? Height { get; set; }
        public int? ShoeSize { get; set; }

        public string? HairColor { get; set; }

        public string? Comments { get; set; }
    }





}
