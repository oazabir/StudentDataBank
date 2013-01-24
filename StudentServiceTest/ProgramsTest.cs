using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace StudentServiceTest
{
    [TestClass]
    public class ProgramsTest
    {
        private readonly static XNamespace ns = Config.Default.Namespace;
        [TestMethod]
        public void TestUniversityPrograms()
        {
            var url = Config.Default.BaseUrl + Config.Default.Universities + Config.Default.SingleUniversity + Config.Default.Programs;
            XDocument programs = XDocument.Load(url);
            Assert.IsNotNull(programs.Element(ns + "Programs").Element(ns + "Program"));
            Assert.AreEqual(Config.Default.UniversityProgramCode, programs.Element(ns + "Programs").Element(ns + "Program").Element(ns + "Code").Value);
        }

        [TestMethod]
        public void TestProgramCourses()
        {
            var url = Config.Default.BaseUrl + Config.Default.Universities + Config.Default.SingleUniversity + Config.Default.Programs + Config.Default.UniversityProgramCode + Config.Default.ProgramCourses;
            XDocument programs = XDocument.Load(url);
            Assert.IsNotNull(programs.Element(ns + "Courses").Element(ns + "Course"));
            Assert.AreEqual(Config.Default.ProgramCourseCode, programs.Element(ns + "Courses").Element(ns + "Course").Element(ns + "Code").Value);
        }
    }
}
