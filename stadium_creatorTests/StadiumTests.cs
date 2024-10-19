using Microsoft.VisualStudio.TestTools.UnitTesting;
using stadium_creator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;

namespace stadium_creator.Tests
{
    [TestClass()]
    public class StadiumTests
    {
        [Test]
        public void StadiumInitialization_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            string name = "National Stadium";
            int upperStands = 3;
            int lowerStands = 2;

            // Act
            var stadium = new Stadium(name, upperStands, lowerStands);

            // Assert
            Assert.AreEqual(name, stadium.Name);
            Assert.AreEqual(upperStands, stadium.NumberOfUpperStands);
            Assert.AreEqual(lowerStands, stadium.NumberOfLowerStands);
            Assert.IsNotNull(stadium.EventList);
            Assert.IsInstanceOf<List<Event>>(stadium.EventList);
        }
    }

    [TestFixture]
    public class StandTests
    {
        [Test]
        public void StandInitialization_ShouldCreateEmptySeatsArray()
        {
            // Arrange
            int columns = 5;
            int rows = 3;

            // Act
            var stand = new Stand(columns, rows);

            // Assert
            Assert.AreEqual(columns, stand.NumberOfColumns);
            Assert.AreEqual(rows, stand.NumberOfRows);
            Assert.AreEqual(columns, stand.Seats.GetLength(0));
            Assert.AreEqual(rows, stand.Seats.GetLength(1));

            // Check if all seats are set to false
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Assert.IsFalse(stand.Seats[i, j]);
                }
            }
        }

        [Test]
        public void DisplaySeats_ShouldOutputSeatsCorrectly()
        {
            // Arrange
            var stand = new Stand(2, 2);
            stand.Seats[0, 0] = true; // Mark one seat as taken

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                // Act
                stand.DisplaySeats();

                // Assert
                string expected = "1 2  \r\n1 \u25a1 \u25a0 \r\n2 \u25a0 \u25a0 \r\n";
                Assert.AreEqual(expected, sw.ToString().Replace("\r\n", "\n"));
            }
        }
    }
}