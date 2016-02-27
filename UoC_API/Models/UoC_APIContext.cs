using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;

namespace UoC_API.Models
{
    public class UoC_APIContext : DbContext
    {

        public UoC_APIContext() : base("name=UoC_APIContext")
        {
            if (!Database.Exists()) Database.CreateIfNotExists();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UoC_APIContext, MyConfiguration>());
        }

        public DbSet<Student>    Students    { get; set; }
        public DbSet<Course>     Courses     { get; set; }
        public DbSet<Test>       Tests       { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<ToDoItem>   ToDoItems   { get; set; }

        public class MyConfiguration : DbMigrationsConfiguration<UoC_APIContext>
        {
            public MyConfiguration()
            {
                AutomaticMigrationsEnabled = true;
                AutomaticMigrationDataLossAllowed = true;
            }

            protected override void Seed(UoC_APIContext context)
            {
                var students = new List<Student>
                    {
                        new Student {
                            ID = 1,
                            FirstMidName = "Carson",
                            LastName = "Alexander",
                            Email = "1234@notrealdomain.com"
                        },
                        new Student {
                            ID = 2,
                            FirstMidName = "Meredith",
                            LastName = "Alonso",
                            Email = "5678@notrealdomain.com"
                        }
                    };
                students.ForEach(s => context.Students.AddOrUpdate(p => p.ID, s));
                context.SaveChanges();

                var toDoList = new List<ToDoItem>
            {
                new ToDoItem
                {
                    ID = 1,
                    Message = "It wasnt me, you cant prove it",
                    Student = context.Students.Find(1)
                },
                new ToDoItem
                {
                    ID = 2,
                    Message = "Swagger",
                    Student = context.Students.Find(1)
                },
                new ToDoItem
                {
                    ID = 3,
                    Message = "Finish MSA Project",
                    Student = context.Students.Find(1)
                },
                new ToDoItem
                {
                    ID = 4,
                    Message = "Fix the Internet",
                    Student = context.Students.Find(2)
                },
                new ToDoItem
                {
                    ID = 5,
                    Message = "Fix the water",
                    Student = context.Students.Find(2)
                },
                new ToDoItem
                {
                    ID = 6,
                    Message = "Get new Laptop",
                    Student = context.Students.Find(2)
                }
            };
                toDoList.ForEach(s => context.ToDoItems.AddOrUpdate(p => p.ID, s));
                context.SaveChanges();

                var courses = new List<Course>
                        {
                            new Course {
                                ID = 1,
                                Student = context.Students.Find(2),
                                Title = "Chemistry 101",
                                Description = "Chemistry of the Living World",
                                Credits = 3,
                                Grade = Grade.Bp
                            },
                            new Course {
                                ID = 2,
                                Student = context.Students.Find(2),
                                Title = "Maths 108",
                                Description = "Concepts of Differentiation and Integration",
                                Credits = 4,
                                Grade = Grade.Cm
                            },
                            new Course {
                                ID = 3,
                                Student = context.Students.Find(2),
                                Title = "Physics 150",
                                Description = "Astronomical Phenomena",
                                Credits = 3,
                                Grade = Grade.A
                            },
                            new Course {
                                ID = 4,
                                Student = context.Students.Find(1),
                                Title = "Economics 101a",
                                Description = "Microeconomics",
                                Credits = 5,
                                Grade = Grade.Cm
                            },
                            new Course {
                                ID = 5,
                                Student = context.Students.Find(1),
                                Title = "Economics 101b",
                                Description = "Macroeconomics",
                                Credits = 5
                            }
                        };
                courses.ForEach(s => context.Courses.AddOrUpdate(p => p.ID, s));
                context.SaveChanges();

                var tests = new List<Test>
                        {
                            new Test {
                                ID = 1,
                                Course = context.Courses.SingleOrDefault(c => c.ID == 1),
                                Name = "Test 1",
                                TestDateTime = DateTime.Parse("15/04/2013 12:00 PM", new CultureInfo("en-NZ")),
                                Weighting = 15,
                                Marks = 50,
                                Score = 42
                            },
                            new Test {
                                ID = 2,
                                Course = context.Courses.SingleOrDefault(c => c.ID == 1),
                                Name = "Test 2",
                                TestDateTime = DateTime.Parse("1/05/2013 3:00 PM", new CultureInfo("en-NZ")),
                                Weighting = 10
                            },
                            new Test {
                                ID = 3,
                                Course = context.Courses.SingleOrDefault(c => c.ID == 5),
                                Name = "Test 1",
                                TestDateTime = DateTime.Parse("15/04/2013 12:00 PM", new CultureInfo("en-NZ")),
                                Weighting = 15,
                                Marks = 50,
                                Score = 42
                            },
                            new Test {
                                ID = 4,
                                Course = context.Courses.SingleOrDefault(c => c.ID == 3),
                                Name = "Test",
                                TestDateTime = DateTime.Parse("17/04/2013 11:00 AM", new CultureInfo("en-NZ")),
                                Weighting = 15,
                                Marks = 50,
                                Score = 42
                            },
                            new Test {
                                ID = 5,
                                Course = context.Courses.SingleOrDefault(c => c.ID == 3),
                                Name = "Test 2",
                                TestDateTime = DateTime.Parse("27/04/2013 9:00 AM", new CultureInfo("en-NZ")),
                                Weighting = 12.5,
                                Marks = 100,
                                Score = 53
                            },
                        };
                tests.ForEach(s => context.Tests.AddOrUpdate(p => p.ID, s));
                context.SaveChanges();

                var assignments = new List<Assignment>
                        {
                            new Assignment {
                                ID = 1,
                                Course = context.Courses.SingleOrDefault(c => c.ID == 2),
                                Name = "Assignment 1",
                                DueDateTime = DateTime.Parse("15/05/2013 12:00 PM", new CultureInfo("en-NZ")),
                                Weighting = 5,
                                Marks = 50,
                                Score = 42
                            },
                            new Assignment {
                                ID = 2,
                                Course = context.Courses.SingleOrDefault(c => c.ID == 2),
                                Name = "Assignment 2",
                                DueDateTime = DateTime.Parse("7/06/2013 11:59 PM", new CultureInfo("en-NZ")),
                                Weighting = 5
                            },
                            new Assignment {
                                ID = 3,
                                Course = context.Courses.SingleOrDefault(c => c.ID == 4),
                                Name = "Assignment",
                                DueDateTime = DateTime.Parse("15/04/2013 12:00 PM", new CultureInfo("en-NZ")),
                                Weighting = 10,
                                Marks = 100,
                                Score = 99
                            },
                            new Assignment {
                                ID = 4,
                                Course = context.Courses.SingleOrDefault(c => c.ID == 5),
                                Name = "Quiz",
                                DueDateTime = DateTime.Parse("17/04/2013 10:00 AM", new CultureInfo("en-NZ")),
                                Weighting = 2,
                                Marks = 10,
                                Score = 7
                            },
                            new Assignment {
                                ID = 5,
                                Course = context.Courses.SingleOrDefault(c => c.ID == 1),
                                Name = "Lab 1",
                                DueDateTime = DateTime.Parse("27/04/2013 9:00 AM", new CultureInfo("en-NZ")),
                                Weighting = 6.33,
                                Marks = 50,
                                Score = 36
                            },
                        };
                assignments.ForEach(s => context.Assignments.AddOrUpdate(p => p.ID, s));
                context.SaveChanges();
            }
        }
    }
}
