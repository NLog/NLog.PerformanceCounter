// 
// Copyright (c) 2004-2021 Jaroslaw Kowalski <jaak@jkowalski.net>, Kim Christensen, Julian Verdurmen
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

namespace NLog.PerformanceCounter.Tests
{
    using System;
    using NLog.Config;
    using NLog.LayoutRenderers;
    using Xunit;

    public class PerformanceCounterLayoutRendererTests
    {
        public PerformanceCounterLayoutRendererTests()
        {
            LogManager.ThrowExceptions = true;
        }

        [SkippableFact]
        public void PerformanceCounterLayoutRendererTest()
        {
            // Arrange
            Skip.IfNot(Environment.OSVersion.Platform == PlatformID.Win32NT, "Performance counters are only supported on Windows.");

            var logFactory = new LogFactory().Setup().SetupExtensions(ext => ext.RegisterLayoutRenderer<PerformanceCounterLayoutRenderer>("performancecounter")).
                LoadConfigurationFromXml(@"
            <nlog>
                <targets><target name='debug' type='Debug' layout='${performancecounter:category=Process:counter=Working Set:format=F0:MachineName=}' /></targets>
                <rules>
                    <logger name='*' minlevel='Debug' writeTo='debug' />
                </rules>
            </nlog>").LogFactory;

            var debugTarget = logFactory.Configuration.FindTargetByName<NLog.Targets.DebugTarget>("debug");

            var logger = logFactory.GetLogger("A");

            // Act
            logger.Debug("a");
            var firstValue = long.Parse(debugTarget.LastMessage);
            logger.Debug("b");
            logger.Debug("c");
            logger.Debug("d");
            var lastValue = long.Parse(debugTarget.LastMessage);

            // Assert
            Assert.NotEqual(0, firstValue);
            Assert.NotEqual(0, lastValue);
        }

        [Fact]
        public void InitializeLayoutRenderer_ThrowsWhenCategoryIsMissing()
        {
            // Arrange
            var renderer = new TestPerformanceCounterLayoutRenderer { Counter = "Working Set" };

            // Act
            var exception = Assert.Throws<NLogConfigurationException>(renderer.InitializeForTest);

            // Assert
            Assert.Contains(nameof(PerformanceCounterLayoutRenderer.Category), exception.Message);
        }

        [Fact]
        public void InitializeLayoutRenderer_ThrowsWhenCounterIsMissing()
        {
            // Arrange
            var renderer = new TestPerformanceCounterLayoutRenderer { Category = "Process" };

            // Act
            var exception = Assert.Throws<NLogConfigurationException>(renderer.InitializeForTest);

            // Assert
            Assert.Contains(nameof(PerformanceCounterLayoutRenderer.Counter), exception.Message);
        }

        [Fact]
        public void DefaultOptions_AreInitialized()
        {
            // Arrange
            var renderer = new PerformanceCounterLayoutRenderer();

            // Act
            var instance = renderer.Instance;
            var machineName = renderer.MachineName;
            var format = renderer.Format;
            var culture = renderer.Culture;

            // Assert
            Assert.Null(instance);
            Assert.Null(machineName);
            Assert.Null(format);
            Assert.Null(culture);
        }

        private sealed class TestPerformanceCounterLayoutRenderer : PerformanceCounterLayoutRenderer
        {
            public void InitializeForTest()
            {
                InitializeLayoutRenderer();
            }
        }
    }
}
