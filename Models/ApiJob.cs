﻿using System.ComponentModel.DataAnnotations;

namespace BEDAssignment2.Models
{
    public class JobWithoutExpenses
    {
        
            public JobWithoutExpenses(string customer, DateTimeOffset startDate, int days, string location, string comments)
            {
                Customer = customer;
                StartDate = startDate;
                Days = days;
                Location = location;
                Comments = comments;
                Models = new List<Model>();
                
            }

            public long JobId { get; set; }
            [MaxLength(64)]
            public string? Customer { get; set; }
            public DateTimeOffset StartDate { get; set; }
            public int Days { get; set; }
            [MaxLength(128)]
            public string? Location { get; set; }
            [MaxLength(2000)]
            public string? Comments { get; set; }
            public List<Model>? Models { get; set; }
    }

    public class JobWithoutModels
    {

        public JobWithoutModels(string customer, DateTimeOffset startDate, int days, string location, string comments)
        {
            Customer = customer;
            StartDate = startDate;
            Days = days;
            Location = location;
            Comments = comments;
            Expenses = new List<Expense>();
        }

        public long JobId { get; set; }
        [MaxLength(64)]
        public string? Customer { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public int Days { get; set; }
        [MaxLength(128)]
        public string? Location { get; set; }
        [MaxLength(2000)]
        public string? Comments { get; set; }
        public List<Expense>? Expenses { get; set; }
    }






}

