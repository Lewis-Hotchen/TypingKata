using System.Threading;
using Autofac;
using KataIocModule;
using KataShellUnitTests.HelperClasses;
using Moq;
using NUnit.Framework;
using TypingKata;

namespace TypingKataShellUnitTests {

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class BootStrapperUnitTest {

        private Mock<ILog4NetConfigurator> _log4NetConfigurator;
        private Mock<IContainerBuilderFacade> _builder;
        
        [SetUp]
        public void Setup() {
            _log4NetConfigurator = new Mock<ILog4NetConfigurator>();
            _builder = new Mock<IContainerBuilderFacade>();
            BootStrapper.Log4NetConfigurator = _log4NetConfigurator.Object;
        }

        [Test]
        public void ShouldBuildContainer() {
            StartBootStrapper();
            Assert.NotNull(BootStrapper.Container);
        }

        [Test]
        public void ShouldRegisterTypeCorrectly() {
            StartBootStrapper();
            Mock<IContainerBuilderFacade> containerBuilderFacade = new Mock<IContainerBuilderFacade>();
            containerBuilderFacade.Setup(x => x.Build()).Returns(containerBuilderFacade.Object);
            containerBuilderFacade.Setup(x => x.GetCachedBuilder()).Returns(new ContainerBuilder());
            _builder.Setup(x => x.RegisterType<It.IsAnyType, It.IsAnyType>()).Returns(containerBuilderFacade.Object);
            _builder.Setup(x => x.Build()).Returns(containerBuilderFacade.Object);
            _builder.Setup(x => x.GetCachedBuilder()).Returns(new ContainerBuilder());
            BootStrapper.RegisterType<RootView, ResolveHelper, IResolveHelper>(_builder.Object);
            _builder.Verify();
        }

        public static void StartBootStrapper() {
            BootStrapper.Start<RootView>();
        }
    }
}