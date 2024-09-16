using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMD.Appointment.Domain.Services;

namespace CMD.UnitTest
{
    /// <summary>
    /// Contains unit tests for the <see cref="DateValidator"/> class, specifically testing the <see cref="DateValidator.ValidateDate(DateOnly)"/> method.
    /// </summary>
    [TestClass]
    public class DateValidatorUnitTest
    {
        /// <summary>
        /// Tests that <see cref="DateValidator.ValidateDate(DateOnly)"/> returns <c>false</c> when the provided date is in the past.
        /// </summary>
        [TestMethod]
        public void ValidateDate_ReturnsFalse_WhenDateIsInThePast()
        {
            // Arrange
            DateOnly pastDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));

            // Act
            bool result = DateValidator.ValidateDate(pastDate);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests that <see cref="DateValidator.ValidateDate(DateOnly)"/> returns <c>true</c> when the provided date is today.
        /// </summary>
        [TestMethod]
        public void ValidateDate_ReturnsTrue_WhenDateIsToday()
        {
            // Arrange
            DateOnly todayDate = DateOnly.FromDateTime(DateTime.Now);

            // Act
            bool result = DateValidator.ValidateDate(todayDate);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests that <see cref="DateValidator.ValidateDate(DateOnly)"/> returns <c>true</c> when the provided date is exactly 30 days from today.
        /// </summary>
        [TestMethod]
        public void ValidateDate_ReturnsTrue_WhenDateIsExactly30DaysFromToday()
        {
            // Arrange
            DateOnly maxDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));

            // Act
            bool result = DateValidator.ValidateDate(maxDate);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests that <see cref="DateValidator.ValidateDate(DateOnly)"/> returns <c>false</c> when the provided date is greater than 30 days from today.
        /// </summary>
        [TestMethod]
        public void ValidateDate_ReturnsFalse_WhenDateIsGreaterThan30DaysFromToday()
        {
            // Arrange
            DateOnly futureDate = DateOnly.FromDateTime(DateTime.Now.AddDays(31));

            // Act
            bool result = DateValidator.ValidateDate(futureDate);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests that <see cref="DateValidator.ValidateDate(DateOnly)"/> returns <c>true</c> when the provided date is 1 day in the future.
        /// </summary>
        [TestMethod]
        public void ValidateDate_ReturnsTrue_WhenDateIs1DayInTheFuture()
        {
            // Arrange
            DateOnly futureDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

            // Act
            bool result = DateValidator.ValidateDate(futureDate);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
