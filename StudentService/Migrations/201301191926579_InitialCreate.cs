namespace StudentService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Programs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 255),
                        University_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Universities", t => t.University_Id)
                .Index(t => t.University_Id);
            
            CreateTable(
                "dbo.Universities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 255),
                        Address = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UniversityStudents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Firstname = c.String(nullable: false, maxLength: 50),
                        Lastname = c.String(nullable: false, maxLength: 50),
                        StudentId = c.String(nullable: false, maxLength: 50),
                        University_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Universities", t => t.University_Id)
                .Index(t => t.University_Id);
            
            CreateTable(
                "dbo.UniversityStudentLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniversityCode = c.String(nullable: false, maxLength: 50),
                        StudentId = c.String(nullable: false, maxLength: 50),
                        Student_Id = c.Int(),
                        University_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UniversityStudents", t => t.Student_Id)
                .ForeignKey("dbo.Universities", t => t.University_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.University_Id);
            
            CreateTable(
                "dbo.StudentCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseCode = c.String(nullable: false, maxLength: 50),
                        Score = c.Single(nullable: false),
                        Grade = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Student_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UniversityStudents", t => t.Student_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.StudentPrograms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramCode = c.String(nullable: false, maxLength: 50),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LastRefreshedAt = c.DateTime(nullable: false),
                        CGPA = c.Single(nullable: false),
                        Student_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UniversityStudents", t => t.Student_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.CourseCrediteds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequiredCourseCode = c.String(nullable: false, maxLength: 50),
                        CreditedCourseCode = c.String(nullable: false, maxLength: 50),
                        CreditedCourseUniversityCode = c.String(nullable: false, maxLength: 50),
                        Score = c.Single(nullable: false),
                        Grade = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        StudentProgram_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudentPrograms", t => t.StudentProgram_Id)
                .Index(t => t.StudentProgram_Id);
            
            CreateTable(
                "dbo.UniversityCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Code = c.String(nullable: false, maxLength: 50),
                        UniversalCourseCode = c.String(nullable: false, maxLength: 50),
                        University_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Universities", t => t.University_Id)
                .Index(t => t.University_Id);
            
            CreateTable(
                "dbo.ProgramCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Program_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.Program_Id)
                .Index(t => t.Program_Id);
            
            CreateTable(
                "dbo.UniversalCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        Type = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentUserMaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false, maxLength: 50),
                        UniversityCode = c.String(nullable: false, maxLength: 50),
                        RegisteredUniversity_Id = c.Int(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        StudentUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.StudentUser_Id)
                .ForeignKey("dbo.Universities", t => t.RegisteredUniversity_Id, cascadeDelete: true)
                .Index(t => t.StudentUser_Id)
                .Index(t => t.RegisteredUniversity_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.StudentUserMaps", new[] { "RegisteredUniversity_Id" });
            DropIndex("dbo.StudentUserMaps", new[] { "StudentUser_Id" });
            DropIndex("dbo.ProgramCourses", new[] { "Program_Id" });
            DropIndex("dbo.UniversityCourses", new[] { "University_Id" });
            DropIndex("dbo.CourseCrediteds", new[] { "StudentProgram_Id" });
            DropIndex("dbo.StudentPrograms", new[] { "Student_Id" });
            DropIndex("dbo.StudentCourses", new[] { "Student_Id" });
            DropIndex("dbo.UniversityStudentLinks", new[] { "University_Id" });
            DropIndex("dbo.UniversityStudentLinks", new[] { "Student_Id" });
            DropIndex("dbo.UniversityStudents", new[] { "University_Id" });
            DropIndex("dbo.Programs", new[] { "University_Id" });
            DropForeignKey("dbo.StudentUserMaps", "RegisteredUniversity_Id", "dbo.Universities");
            DropForeignKey("dbo.StudentUserMaps", "StudentUser_Id", "dbo.Users");
            DropForeignKey("dbo.ProgramCourses", "Program_Id", "dbo.Programs");
            DropForeignKey("dbo.UniversityCourses", "University_Id", "dbo.Universities");
            DropForeignKey("dbo.CourseCrediteds", "StudentProgram_Id", "dbo.StudentPrograms");
            DropForeignKey("dbo.StudentPrograms", "Student_Id", "dbo.UniversityStudents");
            DropForeignKey("dbo.StudentCourses", "Student_Id", "dbo.UniversityStudents");
            DropForeignKey("dbo.UniversityStudentLinks", "University_Id", "dbo.Universities");
            DropForeignKey("dbo.UniversityStudentLinks", "Student_Id", "dbo.UniversityStudents");
            DropForeignKey("dbo.UniversityStudents", "University_Id", "dbo.Universities");
            DropForeignKey("dbo.Programs", "University_Id", "dbo.Universities");
            DropTable("dbo.StudentUserMaps");
            DropTable("dbo.Users");
            DropTable("dbo.UniversalCourses");
            DropTable("dbo.ProgramCourses");
            DropTable("dbo.UniversityCourses");
            DropTable("dbo.CourseCrediteds");
            DropTable("dbo.StudentPrograms");
            DropTable("dbo.StudentCourses");
            DropTable("dbo.UniversityStudentLinks");
            DropTable("dbo.UniversityStudents");
            DropTable("dbo.Universities");
            DropTable("dbo.Programs");
        }
    }
}
