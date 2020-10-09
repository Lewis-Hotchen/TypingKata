using System;
using System.Collections.Generic;
using Autofac;

namespace TypingKata {

    /// <summary>
    /// This class is used to use DI to inject a container builder. And keep track of registered classes for reconstruction of Container.
    /// </summary>
    public interface IContainerBuilderFacade {

        IList<Type> RegisterHistory { get; }
        ContainerBuilder GetCachedBuilder();
        void RegisterType<TInt, TImp>();
        void RegisterType(Type t);
        void RegisterHistoryToBuilder();
    }
}