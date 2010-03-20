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

using System.Reflection;
using System.Threading;
using Ninject.Extensions.MessageBroker.Model.Channels;
using Ninject.Infrastructure.Disposal;
using Ninject.Injection;

#endregion

namespace Ninject.Extensions.MessageBroker.Model.Subscriptions
{
    /// <summary>
    /// The stock implementation of a <see cref="IMessageSubscription"/>.
    /// </summary>
    public class StandardMessageSubscription : DisposableObject, IMessageSubscription
    {
        #region Fields

        private readonly DeliveryThread _deliveryThread;
        private IMessageChannel _channel;
        private MethodInjector _injector;
        private object _subscriber;
#if !SILVERLIGHT && !NETCF
        private readonly SynchronizationContext _syncContext;
#endif

        #endregion

        #region Properties

        /// <summary>
        /// Gets the channel associated with the subscription.
        /// </summary>
        public IMessageChannel Channel
        {
            get { return _channel; }
        }

        /// <summary>
        /// Gets the object that will receive the channel events.
        /// </summary>
        public object Subscriber
        {
            get { return _subscriber; }
        }

        /// <summary>
        /// Gets the injector that will be triggered when an event occurs.
        /// </summary>
        public MethodInjector Injector
        {
            get { return _injector; }
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
                _channel = null;
                _subscriber = null;
                _injector = null;
            }

            base.Dispose( disposing );
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardMessageSubscription"/> class.
        /// </summary>
        /// <param name="channel">The channel associated with the subscription.</param>
        /// <param name="subscriber">The object that will receive the channel events.</param>
        /// <param name="injector">The injector that will be triggered an event occurs.</param>
        /// <param name="deliveryThread">The thread context that should be used to deliver the message.</param>
        public StandardMessageSubscription( IMessageChannel channel, object subscriber, MethodInjector injector,
                                            DeliveryThread deliveryThread )
        {
            _channel = channel;
            _subscriber = subscriber;
            _injector = injector;
            _deliveryThread = deliveryThread;

#if !SILVERLIGHT && !NETCF
            if ( deliveryThread == DeliveryThread.UserInterface )
            {
                _syncContext = SynchronizationContext.Current;
            }
#endif
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Triggers the event handler associated with the subscription.
        /// </summary>
        /// <param name="sender">The object that published the event.</param>
        /// <param name="args">The event arguments.</param>
        public void Deliver( object sender, object args )
        {
            Ensure.NotDisposed( this );

            switch ( _deliveryThread )
            {
                case DeliveryThread.Background:
                    DeliverViaBackgroundThread( sender, args );
                    break;

#if !SILVERLIGHT && !NETCF
                case DeliveryThread.UserInterface:
                    DeliverViaSynchronizationContext( sender, args );
                    break;
#endif

                default:
                    DeliverMessage( sender, args );
                    break;
            }
        }

        #endregion

        #region Private Methods

        private void DeliverMessage( object sender, object args )
        {
            try
            {
                _injector.Invoke( _subscriber, new[] {sender, args} );
            }
            catch ( TargetInvocationException ex )
            {
                if ( ex.InnerException != null )
                {
                    throw ex.InnerException;
                }
            }
        }

#if !SILVERLIGHT && !NETCF
        private void DeliverViaSynchronizationContext( object sender, object args )
        {
            if ( _syncContext != null )
            {
                _syncContext.Send( delegate
                                   {
                                       DeliverMessage( sender, args );
                                   }, null );
            }
            else
            {
                DeliverMessage( sender, args );
            }
        }
#endif

        private void DeliverViaBackgroundThread( object sender, object args )
        {
            ThreadPool.QueueUserWorkItem( s => DeliverMessage( sender, args ) );
        }

        #endregion
    }
}