using System;
using System.Collections.Generic;
using Autofac;

namespace TypingKata {

    /// <summary>
    /// This class is used to use DI to inject a container builder. And keep track of registered classes for reconstruction of Container.
    /// </summary>
    public class ContainerBuilderFacade : IContainerBuilderFacade {

        private readonly ContainerBuilder _builder;
        public IList<Type> RegisterHistory { get; }

        /// <summary>
        /// Instantiates new <see cref="ContainerBuilderFacade"/>.
        /// </summary>
        public ContainerBuilderFacade() {
            RegisterHistory = new List<Type>();
            _builder = new ContainerBuilder();
        }

        /// <summary>
        /// Register History of types to the builder when finished adding types to builder.
        /// </summary>
        public void RegisterHistoryToBuilder() {
            foreach (var type in RegisterHistory) {
                _builder.RegisterType(type);
            }
        }

        /// <summary>
        /// Get's the Container Builder cached in the class.
        /// </summary>
        /// <returns>The container builder.</returns>
        public ContainerBuilder GetCachedBuilder() {
            return _builder;
        }

        /// <summary>
        /// Register a new type to an interface.
        /// </summary>
        /// <typeparam name="TInt"></typeparam>
        /// <typeparam name="TImp"></typeparam>
        public void RegisterType<TInt, TImp>() {
            _builder.RegisterType<TImp>().As<TInt>();
            RegisterHistory.Add(typeof(TImp));
        }

        /// <summary>
        /// Register new type from a type.
        /// </summary>
        /// <param name="t">The type to register.</param>
        public void RegisterType(Type t) {
            _builder.RegisterType(t);
            RegisterHistory.Add(t);
        }   
    }
}