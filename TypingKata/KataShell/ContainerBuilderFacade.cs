using System;
using System.Collections.Generic;
using Autofac;
using log4net;

namespace TypingKata {

    /// <summary>
    /// This class is used to use DI to inject a container builder. And keep track of registered classes for reconstruction of Container.
    /// </summary>
    public class ContainerBuilderFacade : IContainerBuilderFacade {

        private ILog _log = LogManager.GetLogger(nameof(ContainerBuilderFacade));
        private readonly ContainerBuilder _builder;
        private IList<Type> RegisterHistory { get; set; }
        private bool _isBuilt;
        /// <summary>
        /// Instantiates new <see cref="ContainerBuilderFacade"/>.
        /// </summary>
        public ContainerBuilderFacade() {
            RegisterHistory = new List<Type>();
            _builder = new ContainerBuilder();
            _isBuilt = false;
        }

        /// <summary>
        /// Register History of types to the builder when finished adding types to builder.
        /// </summary>
        public IContainerBuilderFacade Build() {
            foreach (var type in RegisterHistory) {
                _builder.RegisterType(type);
            }

            _isBuilt = true;
            return this;
        }

        /// <summary>
        /// Get's the Container Builder cached in the class.
        /// </summary>
        /// <returns>The container builder.</returns>
        public ContainerBuilder GetCachedBuilder() {
            if (!_isBuilt) {
                _log.Error($"History has not been registered. Use {nameof(Build)} to register.");
            }

            return _builder;
        }

        /// <summary>
        /// Determines if the register history has any entries.
        /// </summary>
        /// <returns>True if any items exist, false otherwise.</returns>
        public bool IsRegisterHistoryEmpty() {
            return RegisterHistory.Count == 0;
        }

        /// <summary>
        /// Clears the Register History.
        /// </summary>
        public void ClearHistory() {
            RegisterHistory = new List<Type>();
            _isBuilt = false;
        }

        /// <summary>
        /// Register a new type to an interface.
        /// </summary>
        /// <typeparam name="TInt"></typeparam>
        /// <typeparam name="TImp"></typeparam>
        public IContainerBuilderFacade RegisterType<TInt, TImp>() {
            _builder.RegisterType<TImp>().As<TInt>();
            RegisterHistory.Add(typeof(TImp));
            _isBuilt = false;
            return this;
        }

        /// <summary>
        /// Register new type from a type.
        /// </summary>
        /// <param name="t">The type to register.</param>
        public IContainerBuilderFacade RegisterType(Type t) {
            _builder.RegisterType(t);
            RegisterHistory.Add(t);
            _isBuilt = false;
            return this;
        }   
    }
}