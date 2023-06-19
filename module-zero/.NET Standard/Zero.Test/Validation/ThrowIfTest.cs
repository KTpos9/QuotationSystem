using System;
using Xunit;
using Zero.Validation;

namespace Zero.Test.Validation
{
    public class ThrowIfTest
    {
        [Fact]
        public void Null_ShouldThrow_WhenInputNull()
        {
            string value = null;
            Assert.Throws<ArgumentNullException>(() => ThrowIf.Null(value));
        }
    }
}