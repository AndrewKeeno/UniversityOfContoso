using System;
using System.Collections.Generic;

namespace UoC_Site.Models
{
    public class StudentFull
    {
        public StudentFull(Student s)
        {
            ID = s.ID;
            LastName = s.LastName;
            FirstMidName = s.FirstMidName;
            Email = s.Email;
        }

        public StudentFull()
        {
            Courses = new List<CourseEager>();
            ToDoItems = new List<ToDoItemEager>();
        }

        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public string Email { get; set; }

        public ICollection<CourseEager> Courses { get; set; }
        public ICollection<ToDoItemEager> ToDoItems { get; set; }
    }

    public class ToDoItemEager
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public DateTime? SubmittedOn { get; set; }
    }

    public class CourseEager
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Credits { get; set; }
        public int? CompletionPercentage { get; set; }
        public Grade? Grade { get; set; }

        public ICollection<AssignmentEager> Assignments { get; set; }
        public ICollection<TestEager> Tests { get; set; }
    }

    public class TestEager
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? TestDateTime { get; set; }
        public double? Weighting { get; set; }
        public double? Marks { get; set; }
        public double? Score { get; set; }
    }

    public class AssignmentEager
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? DueDateTime { get; set; }
        public double? Weighting { get; set; }
        public double? Marks { get; set; }
        public double? Score { get; set; }
        public string LinkToWork { get; set; }
    }
}
