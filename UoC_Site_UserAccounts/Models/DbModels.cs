using System.Collections.Generic;

namespace UoC_Site_UserAccounts.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public bool IsDone { get; set; }

        public virtual Student Student { get; set; }
    }

    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? Credits { get; set; }

        public Grade? Grade { get; set; }

        public virtual Student Student { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }

        public virtual ICollection<Test> Tests { get; set; }
    }

    public enum Grade { Ap = 9, A = 8, Am = 7, Bp = 6, B = 5, Bm = 4, Cp = 3, C = 2, Cm = 1, Dp = 0, D = 0, Dm = 0, F = 0 }

    public class Test
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? Weighting { get; set; }

        public double? Marks { get; set; }

        public double? OutOf { get; set; }

        public double? WeightedScore { get; set; }

        public virtual Course Course { get; set; }
    }

    public class Assignment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? Weighting { get; set; }

        public double? Marks { get; set; }

        public double? OutOf { get; set; }

        public double? WeightedScore { get; set; }

        public string LinkToWork { get; set; }

        public virtual Course Course { get; set; }
    }
}