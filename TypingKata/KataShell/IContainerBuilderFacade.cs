using System;
using System.Collections.Generic;
using Autofac;

namespace TypingKata {

    /// <summary>
    /// This class is used to use DI to inject a container builder. And keep track of registered classes for reconstruction of Container.
    /// </summary>
    public interface IContainerBuilderFacade { 

        /// <summary>
        /// Returns the Cached builder.
        /// </summary>
        /// <returns>The ContainerBuilder.</returns>
        ContainerBuilder GetCachedBuilder();

        /// <summary>
        /// Determines if the register history has any entries.
        /// </summary>
        /// <returns>True if any items exist, false otherwise.</returns>
        bool IsRegisterHistoryEmpty();

        /// <summary>
        /// Clears the Register History.
        /// </summary>
        void ClearHistory();

            /// <summary>
        /// Register a type to the Cached Container Builder with interface.
        /// </summary>
        /// <typeparam name="TInt"></typeparam>
        /// <typeparam name="TImp"></typeparam>
        /// <returns>This instance of <see cref="IContainerBuilderFacade"/>.</returns>
        IContainerBuilderFacade RegisterType<TInt, TImp>();

        /// <summary>
        /// Register a type to the Cached Container Builder with a type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        IContainerBuilderFacade RegisterType(Type t);

        /// <summary>
        /// Build the ContainerBuilder and register all types.
        /// </summary>
        /// <returns>This instance of <see cref="IContainerBuilderFacade"/>.</returns>
        IContainerBuilderFacade Build();
    }
}