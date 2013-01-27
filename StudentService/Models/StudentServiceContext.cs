using System.Data.Entity;
using StudentService.Models.Entity;

namespace StudentService.Models
{
    public class StudentServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<StudentService.Models.StudentServiceContext>());

        public StudentServiceContext() : base("name=StudentServiceContext")
        {
        }

        public DbSet<EIProgram> Programs { get; set; }

        public DbSet<EducationalInstitute> EducationalInstitutes { get; set; }

        public DbSet<EIProgramRequiredCourse> ProgramCourses { get; set; }

        public DbSet<UniversalCourse> UniversalCourses { get; set; }

        public DbSet<EIStudent> Students { get; set; }

        public DbSet<EIStudentEnrolledProgram> StudentPrograms { get; set; }

        public DbSet<StudentLinkToOtherEI> StudentLinks { get; set; }

        public DbSet<EIStudentCourseTaken> StudentCourses { get; set; }

        public DbSet<EICourse> EducationalInstituteCourses { get; set; }

        public DbSet<CourseCreditedTowardsProgram> CourseCrediteds { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<StudentUser> StudentUsers { get; set; }

        public DbSet<StudentUserMap> StudentUserMaps { get; set; }
    }
}
