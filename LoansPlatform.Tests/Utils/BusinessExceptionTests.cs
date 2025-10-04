using LoansPlatform.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansPlatform.Tests.Utils
{
    public class BusinessExceptionTests
    {
        [Fact]
        public void BusinessException_ShouldStoreMessage()
        {
            // Arrange
            var message = "Test error";

            // Act
            var exception = new BusinessException(message);

            // Assert
            exception.Message.Should().Be(message);
        }

        [Fact]
        public void BusinessException_ShouldBeOfTypeException()
        {
            // Arrange
            var exception = new BusinessException("Any error");

            // Assert
            exception.Should().BeAssignableTo<Exception>();
        }
    }
}
