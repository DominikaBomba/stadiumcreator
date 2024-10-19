using Microsoft.VisualStudio.TestTools.UnitTesting;
using stadium_creator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stadium_creator.Tests
{
    [TestClass()]
    public class PersonTests
    {
        [TestMethod()]
        public void PersonTest()
        {
            Person p1 = new Person();
            p1.Name = "Test";
            p1.Surname = "Test";
            p1.BirthDate = 2000;
            Person p2 = new Person("Test", "Test", 2000);
            Assert.AreEqual(p1, p2);
        }

    }
    [TestClass()]
    public class StadiumTests
    {
        [TestMethod()]
        public void StadiumTest()
        {
            Stadium s1 = new Stadium("Name", 12, 11);
            Stadium s2 = new Stadium();
            s2.Name = "Name";
            s2.NumberOfUpperStands = 12;
            s2.NumberOfLowerStands = 11;
            Assert.AreEqual(s1, s2);
        }

    }
}