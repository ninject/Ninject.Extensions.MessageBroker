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
using Ninject.Extensions.MessageBroker.Model.Channels;
using Ninject.Injection;

#endregion

namespace Ninject.Extensions.MessageBroker.Model.Subscriptions
{
    /// <summary>
    /// A message subscription handled by a message broker.
    /// </summary>
    public interface IMessageSubscription : IDisposable
    {
        /// <summary>
        /// Gets the channel associated with the subscription.
        /// </summary>
        IMessageChannel Channel { get; }


        /// <summary>
        /// Gets the object that will receive the channel events.
        /// </summary>
        object Subscriber { get; }


        /// <summary>
        /// Gets the injector that will be triggered when an event occurs.
        /// </summary>
        MethodInjector Injector { get; }


        /// <summary>
        /// Triggers the event handler associated with the subscription.
        /// </summary>
        /// <param name="sender">The object that published the event.</param>
        /// <param name="args">The event arguments.</param>
        void Deliver( object sender, object args );
    }
}