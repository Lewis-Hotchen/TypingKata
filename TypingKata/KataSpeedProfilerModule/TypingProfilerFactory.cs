﻿using Autofac;
using Autofac.Core;
using KataIocModule;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Simple factory to create TypingProfiler.
    /// </summary>
    public class TypingProfilerFactory : ITypingProfilerFactory {

        /// <summary>
        /// Create typing profiler.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="userWords">The User words stack.</param>
        /// <param name="timer">The Timer.</param>
        /// <returns>New ITypingProfiler.</returns>
        public ITypingProfiler CreateTypingProfiler(ICursor cursor, IWordStack userWords, ITypingTimer timer, IMarkovChainGenerator generator) {
            return BootStrapper.Resolve<ITypingProfiler>(new Parameter[] {
                new NamedParameter("cursor", cursor),
                new NamedParameter("userWords", userWords),
                new NamedParameter("timer", timer),
                new NamedParameter("markovChainGenerator", generator), 
            });
        }
    }
}