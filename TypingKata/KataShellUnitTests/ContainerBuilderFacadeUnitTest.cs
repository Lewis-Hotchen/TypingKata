﻿using System.Linq;
using System.Security.Cryptography.X509Certificates;
using KataShellUnitTests.HelperClasses;
using NUnit.Framework;
using TypingKata;

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

            Assert.Contains(typeof(ResolveHelper), target.RegisterHistory.ToArray());
        }

        private ContainerBuilderFacade CreateTarget() {
            return new ContainerBuilderFacade();
        }
    }
}