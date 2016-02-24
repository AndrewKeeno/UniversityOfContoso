using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UoC_Site.Models
{
    public class Student
    {
        public int ID { get; set; }

        public string LastName { get; set; }

        public string FirstMidName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<ToDoItem> ToDoItems { get; set; }
    }

    public class Course
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "A Course name is required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        [Required(ErrorMessage = "A Test name is required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(200, MinimumLength = 1)]
        public string Description { get; set; }

        public int? Credits { get; set; }

        [Range(0.01, 100.00, ErrorMessage = "Completion percentage must be between 0.01 and 100")]
        public int? CompletionPercentage { get; set; }

        [EnumDataType(typeof(Grade))]
        public Grade? Grade { get; set; }

        public virtual Student Student { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
    }

    public enum Grade { Ap = 9, A = 8, Am = 7, Bp = 6, B = 5, Bm = 4, Cp = 3, C = 2, Cm = 1, Dp = 0, D = -1, Dm = -2, F = -3 }

    public class Test
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "A Test name is required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        //[DisplayFormat(DataFormatString = "dd/mm/yyyy hh:mm am/pm")]
        [DataType(DataType.DateTime)]
        public DateTime? TestDateTime { get; set; }

        [Range(0.01, 100.00, ErrorMessage = "Weighing must be between 0.01 and 100")]
        public double? Weighting { get; set; }

        public double? Marks { get; set; }

        public double? Score { get; set; }

        public virtual Course Course { get; set; }
    }

    public class Assignment
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "A Test name is required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DueDateTime { get; set; }

        [Range(0.01, 100.00, ErrorMessage = "Weighing must be between 0.01 and 100")]
        public double? Weighting { get; set; }

        public double? Marks { get; set; }

        public double? Score { get; set; }

        public string LinkToWork { get; set; }

        public virtual Course Course { get; set; }
    }

    public class ToDoItem
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "A Message name is required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(200, MinimumLength = 1)]
        public string Message { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Submitted On")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SubmittedOn { get; set; }

        public virtual Student Student { get; set; }
    }
}
