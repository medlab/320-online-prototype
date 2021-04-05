using Finally.Models;
using System;
using System.Linq;

namespace Finally.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

            
            if (context.Students.Any())
            {
                return;   
            }

            var students = new Student[]
            {
            new Student{FirstMidName="zhang",LastName="San",EnrollmentDate=DateTime.Parse("2005-09-01")},
            new Student{FirstMidName="Li",LastName="Si",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Wang",LastName="Wu",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstMidName="Zhao",LastName="Liu",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Han",LastName="Meimei",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Li",LastName="Lei",EnrollmentDate=DateTime.Parse("2001-09-01")},
            };
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();
        }
    }
}