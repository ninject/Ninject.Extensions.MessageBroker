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
using System.Reflection;
using Ninject.Extensions.MessageBroker.Model.Channels;
using Ninject.Infrastructure.Disposal;

#endregion

namespace Ninject.Extensions.MessageBroker.Model.Publications
{
    /// <summary>
    /// The stock definition of a <see cref="IMessagePublication"/>.
    /// </summary>
    public class StandardMessagePublication : DisposableObject, IMessagePublication
    {
        #region Fields

        private MethodInfo _broadcastMethod;
        private IMessageChannel _channel;
        private EventInfo _evt;
        private Delegate _interceptDelegate;
        private object _publisher;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the channel associated with the publication.
        /// </summary>
        public IMessageChannel Channel
        {
            get { return _channel; }
        }

        /// <summary>
        /// Gets the object that publishes events to the channel.
        /// </summary>
        public object Publisher
        {
            get { return _publisher; }
        }

        /// <summary>
        /// Gets the event that will be published to the channel.
        /// </summary>
        public EventInfo Event
        {
            get { return _evt; }
        }

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
                if ( _interceptDelegate != null )
                {
                    Disconnect();
                }

                _channel = null;
                _evt = null;
                _publisher = null;
            }

            base.Dispose( disposing );
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the StandardMessagePublication class.
        /// </summary>
        /// <param name="channel">The channel associated with the publication.</param>
        /// <param name="publisher">The object that publishes events to the channel.</param>
        /// <param name="evt">The event that will be published to the channel.</param>
        public StandardMessagePublication( IMessageChannel channel, object publisher, EventInfo evt )
        {
            _channel = channel;
            _publisher = publisher;
            _evt = evt;

            Connect();
        }

        #endregion

        #region Private Methods

        private void Connect()
        {
            _interceptDelegate = Delegate.CreateDelegate( _evt.EventHandlerType, _channel, GetBroadcastMethod() );
            _evt.AddEventHandler( _publisher, _interceptDelegate );
        }

        private void Disconnect()
        {
            _evt.RemoveEventHandler( _publisher, _interceptDelegate );
            _interceptDelegate = null;
        }

        private MethodInfo GetBroadcastMethod()
        {
            if ( _broadcastMethod != null )
            {
                return _broadcastMethod;
            }

            // We have to look this up via the concrete type of the channel because Mono doesn't
            // support calling ldftn with interface methods.
            _broadcastMethod = _channel.GetType().GetMethod( "Broadcast", new[] {typeof (object), typeof (object)} );

            return _broadcastMethod;
        }

        #endregion
    }
}