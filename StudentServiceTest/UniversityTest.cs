using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Net;
using System.Net.Http;
using System.Text;

namespace StudentServiceTest
{
    [TestClass]
    public class UniversityTest
    {
        private readonly static XNamespace ns = Config.Default.Namespace;
            
        [TestMethod]
        public void TestGetAllUniversities()
        {
            var url = Config.Default.BaseUrl + Config.Default.Universities;
            XDocument universities = XDocument.Load(url);
            Assert.IsNotNull(universities.Element(ns + "EducationalInstitutes"), "<Universities> root node not found");
            Assert.IsNotNull(universities.Element(ns + "EducationalInstitutes").Element(ns + "University"), "<Universities> does not contain any <University>");
        }

        [TestMethod]
        public void TestSingleUniversity()
        {
            var url = Config.Default.BaseUrl + Config.Default.Universities + Config.Default.SingleUniversity;
            XDocument university = XDocument.Load(url);
            Assert.AreEqual(university.Element(ns + "University").Element(ns + "Code").Value, 
                Config.Default.SingleUniversity.TrimEnd('/'));
        }

        [TestMethod]
        public void TestAddingUniversity()
        {
            var url = Config.Default.BaseUrl + Config.Default.Universities;
            XElement university = new XElement(ns + "University",
                new XElement(ns + "Address", "New City, New Country"),
                new XElement(ns + "Code", "NewUni"),
                new XElement(ns + "Name", "New University"));
            using (var client = new HttpClient())
            {
                var result = client.PostAsync(url, new StringContent(university.ToString(), Encoding.UTF8, "application/xml")).Result;
                Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
                XDocument doc = XDocument.Load(result.Content.ReadAsStreamAsync().Result);
                Assert.AreEqual("NewUni", doc.Element(ns + "University").Element(ns + "Code").Value);
                // Delete the newly added
                Assert.AreEqual(HttpStatusCode.OK, client.DeleteAsync(url + "/NewUni").Result.StatusCode);
            }               
        }

        [TestMethod]
        public void TestUpdateUniversity()
        {
            using (var client = new HttpClient())
            {
                var url = Config.Default.BaseUrl + Config.Default.Universities + Config.Default.SingleUniversity;
                var doc = XDocument.Load(url);
                var name = doc.Element(ns + "University").Element(ns + "Name");
                var oldName = name.Value;
                name.Value = "New Name";
                Assert.AreEqual(HttpStatusCode.OK, client.PutAsync(url, 
                    new StringContent(doc.ToString(), Encoding.UTF8, "application/xml"))
                    .Result.StatusCode);

                name.Value = oldName;
                Assert.AreEqual(HttpStatusCode.OK, client.PutAsync(url,
                    new StringContent(doc.ToString(), Encoding.UTF8, "application/xml"))
                    .Result.StatusCode);

            }
        }
    }
}
