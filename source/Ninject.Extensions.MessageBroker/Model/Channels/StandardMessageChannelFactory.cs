#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using Ninject.Components;
using Ninject.Infrastructure;

#endregion

namespace Ninject.Extensions.MessageBroker.Model.Channels
{
    /// <summary>
    /// The stock implementation of a <see cref="IMessageChannelFactory"/>.
    /// </summary>
    public class StandardMessageChannelFactory : NinjectComponent, IMessageChannelFactory, IHaveKernel
    {
        private readonly IKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardMessageChannelFactory"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public StandardMessageChannelFactory( IKernel kernel )
        {
            _kernel = kernel;
        }

        #region IMessageChannelFactory Members

        /// <summary>
        /// Creates a channel with the specified name.
        /// </summary>
        /// <param name="name">The channel's name.</param>
        /// <returns>The newly-created channel.</returns>
        public IMessageChannel Create( string name )
        {
            return new StandardMessageChannel( Kernel, name );
        }

        #endregion

        #region Implementation of IHaveKernel

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        /// <value>The kernel.</value>
        public IKernel Kernel
        {
            get { return _kernel; }
        }

        #endregion
    }
}