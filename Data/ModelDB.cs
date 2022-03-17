using BEDAssignment2.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BEDAssignment2.Data
{
    public class ModelDB : DbContext
    {
        public ModelDB(DbContextOptions<ModelDB> options)
            : base(options) { }

        public DbSet<Model> Models => Set<Model>();
        public DbSet<Job> Jobs => Set<Job>();
        public DbSet<Expense> Expenses => Set<Expense>();
    }
}
