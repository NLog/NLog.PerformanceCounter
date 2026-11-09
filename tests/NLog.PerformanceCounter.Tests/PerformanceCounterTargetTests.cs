namespace NLog.PerformanceCounter.Tests
{
    using NLog.Config;
    using NLog.Targets;
    using Xunit;

    public class PerformanceCounterTargetTests
    {
        [Fact]
        public void InitializeTarget_ThrowsWhenCategoryNameIsMissing()
        {
            // Arrange
            var target = new TestPerformanceCounterTarget { CounterName = "Counter" };

            // Act
            var exception = Assert.Throws<NLogConfigurationException>(target.InitializeForTest);

            // Assert
            Assert.Contains(nameof(PerformanceCounterTarget.CategoryName), exception.Message);
        }

        [Fact]
        public void InitializeTarget_ThrowsWhenCounterNameIsMissing()
        {
            // Arrange
            var target = new TestPerformanceCounterTarget { CategoryName = "Category" };

            // Act
            var exception = Assert.Throws<NLogConfigurationException>(target.InitializeForTest);

            // Assert
            Assert.Contains(nameof(PerformanceCounterTarget.CounterName), exception.Message);
        }

        [Fact]
        public void DefaultOptions_AreInitialized()
        {
            // Arrange
            var target = new PerformanceCounterTarget();

            // Act
            var autoCreate = target.AutoCreate;
            var instanceName = target.InstanceName;
            var counterHelp = target.CounterHelp;
            var counterType = target.CounterType;
            var incrementValue = target.IncrementValue;

            // Assert
            Assert.False(autoCreate);
            Assert.Equal(string.Empty, instanceName);
            Assert.Equal(string.Empty, counterHelp);
            Assert.Equal(System.Diagnostics.PerformanceCounterType.NumberOfItems32, counterType);
            Assert.Equal("1", incrementValue.Render(NLog.LogEventInfo.CreateNullEvent()));
        }

        [Fact]
        public void ConstructorWithName_SetsTargetName()
        {
            // Arrange
            const string name = "performance-counter";

            // Act
            var target = new PerformanceCounterTarget(name);

            // Assert
            Assert.Equal(name, target.Name);
        }

        private sealed class TestPerformanceCounterTarget : PerformanceCounterTarget
        {
            public void InitializeForTest()
            {
                InitializeTarget();
            }
        }
    }
}