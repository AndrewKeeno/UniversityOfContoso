using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace UoC_Site_UserAccounts.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Student : IdentityUser
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<ToDoItem> ToDoItems { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Student> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class UserDbContext : IdentityDbContext<Student>
    {
        public UserDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static UserDbContext Create()
        {
            return new UserDbContext();
        }
    }
}