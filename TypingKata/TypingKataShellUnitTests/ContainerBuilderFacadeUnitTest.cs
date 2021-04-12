using KataIocModule;
using KataShellUnitTests.HelperClasses;
using NUnit.Framework;

namespace KataShellUnitTests {

    [TestFixture]
    public class ContainerBuilderFacadeUnitTest {

        [SetUp]
        public void Setup() {
        }

        [Test]
        public void ShouldAddTypeToHistoryWhenTypeRegistered() {
            var target = CreateTarget();
            target.RegisterType<IResolveHelper, ResolveHelper>();

            Assert.IsFalse(target.IsRegisterHistoryEmpty());
        }

        private ContainerBuilderFacade CreateTarget() {
            return new ContainerBuilderFacade();
        }
    }
}