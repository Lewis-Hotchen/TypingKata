using Autofac.Core;

namespace KataShellUnitTests.HelperClasses {
    internal class ResolveHelper : Service, IResolveHelper {
        public override string Description { get; }
    }
}