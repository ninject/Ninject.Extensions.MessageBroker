#region License

//
// Author: Nate Kohari <nkohari@gmail.com>
// Copyright (c) 2007-2009, Enkari, Ltd.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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

        public IKernel Kernel
        {
            get { return _kernel; }
        }

        #endregion
    }
}