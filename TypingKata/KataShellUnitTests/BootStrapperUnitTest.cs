using System;
using System.Collections.Generic;
using Autofac;
using KataShellUnitTests.HelperClasses;
using Moq;
using NUnit.Framework;
using TypingKata;

namespace KataShellUnitTests {

    public class BootStrapperUnitTest {

        private Mock<ILog4NetConfigurator> _log4NetConfigurator;
        private Mock<IContainerBuilderFacade> _builder;
        
        [SetUp]
        public void Setup() {
            _log4NetConfigurator = new Mock<ILog4NetConfigurator>();
            _builder = new Mock<IContainerBuilderFacade>();
            BootStrapper.Log4NetConfigurator = _log4NetConfigurator.Object;
            _builder.Setup(x => x.RegisterHistory).Returns(new List<Type>{typeof(ResolveHelper)});
        }

        [Test]
        public void ShouldBuildContainer() {
            StartBootStrapper();

            Assert.NotNull(BootStrapper.Container);
        }

        [Test]
        public void ShouldRegisterTypeCorrectly() {
            StartBootStrapper();
            _builder.Setup(x => x.RegisterType<It.IsAnyType, It.IsAnyType>());
            _builder.Setup(x => x.RegisterHistoryToBuilder());
            _builder.Setup(x => x.GetCachedBuilder()).Returns(new ContainerBuilder());
            BootStrapper.RegisterType<ResolveHelper, IResolveHelper>(_builder.Object);
            _builder.Verify();
        }

        public static void StartBootStrapper() {
            BootStrapper.Start();
        }
    }
}