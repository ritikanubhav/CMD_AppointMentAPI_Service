using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Appointment.Domain.Services;

namespace CMD.UnitTest
{
    [TestClass]
    public class DateValidatorUnitTest
    {
        //private DateValidator dateValidator;

        //[TestInitialize]
        //public void Setup()
        //{
        //    dateValidator = new DateValidator(); 
        //}

        //[TestMethod]
        //public void ValidateDate_ReturnsFalse_WhenDateIsInThePast()
        //{
        //    // Arrange
        //    DateOnly pastDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));

        //    // Act
        //    bool result = dateValidator.ValidateDate(pastDate);

        //    // Assert
        //    Assert.IsFalse(result);
        //}

        //[TestMethod]
        //public void ValidateDate_ReturnsTrue_WhenDateIsToday()
        //{
        //    // Arrange
        //    DateOnly todayDate = DateOnly.FromDateTime(DateTime.Now);

        //    // Act
        //    bool result = dateValidator.ValidateDate(todayDate);

        //    // Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void ValidateDate_ReturnsTrue_WhenDateIsExactly30DaysFromToday()
        //{
        //    // Arrange
        //    DateOnly maxDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));

        //    // Act
        //    bool result = dateValidator.ValidateDate(maxDate);

        //    // Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void ValidateDate_ReturnsFalse_WhenDateIsGreaterThan30DaysFromToday()
        //{
        //    // Arrange
        //    DateOnly futureDate = DateOnly.FromDateTime(DateTime.Now.AddDays(31));

        //    // Act
        //    bool result = dateValidator.ValidateDate(futureDate);

        //    // Assert
        //    Assert.IsFalse(result);
        //}

        //[TestMethod]
        //public void ValidateDate_ReturnsTrue_WhenDateIs1DayInTheFuture()
        //{
        //    // Arrange
        //    DateOnly futureDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        //    // Act
        //    bool result = dateValidator.ValidateDate(futureDate);

        //    // Assert
        //    Assert.IsTrue(result);
        //}
    }
}
