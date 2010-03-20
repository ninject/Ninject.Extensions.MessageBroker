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

using System;
using System.Collections.Generic;
using System.Globalization;
using Ninject.Components;
using Ninject.Extensions.MessageBroker.Model.Channels;
using Ninject.Extensions.MessageBroker.Properties;
using Ninject.Infrastructure;

#endregion

namespace Ninject.Extensions.MessageBroker
{
    /// <summary>
    /// The stock implementation of a message broker.
    /// </summary>
    public class StandardMessageBroker : NinjectComponent, IMessageBroker, IHaveKernel
    {
        private readonly IKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardMessageBroker"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public StandardMessageBroker( IKernel kernel )
        {
            _kernel = kernel;
        }

        #region Fields

        private Dictionary<string, IMessageChannel> _channels = new Dictionary<string, IMessageChannel>();

        #endregion

        #region Disposal

        /// <summary>
        /// Releases all resources held by the object.
        /// </summary>
        /// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
        public override void Dispose( bool disposing )
        {
            if ( disposing && !IsDisposed )
            {
                foreach ( IMessageChannel messageChannel in _channels.Values )
                {
                    messageChannel.Dispose();
                }
                _channels = null;
            }

            base.Dispose( disposing );
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a channel with the specified name, creating it first if necessary.
        /// </summary>
        /// <param name="name">The name of the channel to create or retrieve.</param>
        /// <returns>The object representing the channel.</returns>
        public IMessageChannel GetChannel( string name )
        {
            Ensure.NotDisposed( this );

            if ( !_channels.ContainsKey( name ) )
            {
                var factory = Kernel.Components.Get<IMessageChannelFactory>();
                _channels.Add( name, factory.Create( name ) );
            }

            return _channels[name];
        }


        /// <summary>
        /// Closes a channel, removing it from the message broker.
        /// </summary>
        /// <param name="name">The name of the channel to close.</param>
        public void CloseChannel( string name )
        {
            Ensure.NotDisposed( this );
            ThrowIfChannelDoesNotExist( name );

            IMessageChannel channel = _channels[name];
            channel.Dispose();

            _channels.Remove( name );
        }


        /// <summary>
        /// Enables a channel, causing it to pass messages through as they occur.
        /// </summary>
        /// <param name="name">The name of the channel to enable.</param>
        public void EnableChannel( string name )
        {
            Ensure.NotDisposed( this );
            ThrowIfChannelDoesNotExist( name );
            _channels[name].Enable();
        }


        /// <summary>
        /// Disables a channel, which will block messages from being passed.
        /// </summary>
        /// <param name="name">The name of the channel to disable.</param>
        public void DisableChannel( string name )
        {
            Ensure.NotDisposed( this );
            ThrowIfChannelDoesNotExist( name );
            _channels[name].Disable();
        }

        #endregion

        #region Private Methods

        private void ThrowIfChannelDoesNotExist( string name )
        {
            if ( !_channels.ContainsKey( name ) )
            {
                throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture,
                                                                    Resources.Ex_MessageChannelDoesNotExist, name ) );
            }
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