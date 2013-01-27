namespace StudentService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EIPrograms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 255),
                        EducationalInstitute_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EducationalInstitutes", t => t.EducationalInstitute_Id)
                .Index(t => t.EducationalInstitute_Id);
            
            CreateTable(
                "dbo.EducationalInstitutes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 255),
                        Address = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EIStudents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Firstname = c.String(nullable: false, maxLength: 50),
                        Lastname = c.String(nullable: false, maxLength: 50),
                        StudentId = c.String(nullable: false, maxLength: 50),
                        EducationalInstitute_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EducationalInstitutes", t => t.EducationalInstitute_Id)
                .Index(t => t.EducationalInstitute_Id);
            
            CreateTable(
                "dbo.StudentLinkToOtherEIs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EICode = c.String(nullable: false, maxLength: 50),
                        StudentId = c.String(nullable: false, maxLength: 50),
                        Status = c.Int(nullable: false),
                        Student_Id = c.Int(),
                        EducationalInstitute_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EIStudents", t => t.Student_Id)
                .ForeignKey("dbo.EducationalInstitutes", t => t.EducationalInstitute_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.EducationalInstitute_Id);
            
            CreateTable(
                "dbo.EIStudentCourseTakens",
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
                .ForeignKey("dbo.EIStudents", t => t.Student_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.EIStudentEnrolledPrograms",
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
                .ForeignKey("dbo.EIStudents", t => t.Student_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.CourseCreditedTowardsPrograms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequiredCourseCode = c.String(nullable: false, maxLength: 50),
                        CreditedCourseCode = c.String(nullable: false, maxLength: 50),
                        CreditedCourseEICode = c.String(nullable: false, maxLength: 50),
                        Score = c.Single(nullable: false),
                        Grade = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        StudentProgram_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EIStudentEnrolledPrograms", t => t.StudentProgram_Id)
                .Index(t => t.StudentProgram_Id);
            
            CreateTable(
                "dbo.EICourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Code = c.String(nullable: false, maxLength: 50),
                        UniversalCourseCode = c.String(nullable: false, maxLength: 50),
                        EducationalInstitute_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EducationalInstitutes", t => t.EducationalInstitute_Id)
                .Index(t => t.EducationalInstitute_Id);
            
            CreateTable(
                "dbo.EIProgramRequiredCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Program_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EIPrograms", t => t.Program_Id)
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
                        EICode = c.String(nullable: false, maxLength: 50),
                        RegisteredEI_Id = c.Int(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        StudentUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.StudentUser_Id)
                .ForeignKey("dbo.EducationalInstitutes", t => t.RegisteredEI_Id, cascadeDelete: true)
                .Index(t => t.StudentUser_Id)
                .Index(t => t.RegisteredEI_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.StudentUserMaps", new[] { "RegisteredEI_Id" });
            DropIndex("dbo.StudentUserMaps", new[] { "StudentUser_Id" });
            DropIndex("dbo.EIProgramRequiredCourses", new[] { "Program_Id" });
            DropIndex("dbo.EICourses", new[] { "EducationalInstitute_Id" });
            DropIndex("dbo.CourseCreditedTowardsPrograms", new[] { "StudentProgram_Id" });
            DropIndex("dbo.EIStudentEnrolledPrograms", new[] { "Student_Id" });
            DropIndex("dbo.EIStudentCourseTakens", new[] { "Student_Id" });
            DropIndex("dbo.StudentLinkToOtherEIs", new[] { "EducationalInstitute_Id" });
            DropIndex("dbo.StudentLinkToOtherEIs", new[] { "Student_Id" });
            DropIndex("dbo.EIStudents", new[] { "EducationalInstitute_Id" });
            DropIndex("dbo.EIPrograms", new[] { "EducationalInstitute_Id" });
            DropForeignKey("dbo.StudentUserMaps", "RegisteredEI_Id", "dbo.EducationalInstitutes");
            DropForeignKey("dbo.StudentUserMaps", "StudentUser_Id", "dbo.Users");
            DropForeignKey("dbo.EIProgramRequiredCourses", "Program_Id", "dbo.EIPrograms");
            DropForeignKey("dbo.EICourses", "EducationalInstitute_Id", "dbo.EducationalInstitutes");
            DropForeignKey("dbo.CourseCreditedTowardsPrograms", "StudentProgram_Id", "dbo.EIStudentEnrolledPrograms");
            DropForeignKey("dbo.EIStudentEnrolledPrograms", "Student_Id", "dbo.EIStudents");
            DropForeignKey("dbo.EIStudentCourseTakens", "Student_Id", "dbo.EIStudents");
            DropForeignKey("dbo.StudentLinkToOtherEIs", "EducationalInstitute_Id", "dbo.EducationalInstitutes");
            DropForeignKey("dbo.StudentLinkToOtherEIs", "Student_Id", "dbo.EIStudents");
            DropForeignKey("dbo.EIStudents", "EducationalInstitute_Id", "dbo.EducationalInstitutes");
            DropForeignKey("dbo.EIPrograms", "EducationalInstitute_Id", "dbo.EducationalInstitutes");
            DropTable("dbo.StudentUserMaps");
            DropTable("dbo.Users");
            DropTable("dbo.UniversalCourses");
            DropTable("dbo.EIProgramRequiredCourses");
            DropTable("dbo.EICourses");
            DropTable("dbo.CourseCreditedTowardsPrograms");
            DropTable("dbo.EIStudentEnrolledPrograms");
            DropTable("dbo.EIStudentCourseTakens");
            DropTable("dbo.StudentLinkToOtherEIs");
            DropTable("dbo.EIStudents");
            DropTable("dbo.EducationalInstitutes");
            DropTable("dbo.EIPrograms");
        }
    }
}
