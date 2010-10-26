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
using Ninject.Extensions.MessageBroker.Model.Channels;
using Ninject.Injection;

#endregion

namespace Ninject.Extensions.MessageBroker.Model.Subscriptions
{
    /// <summary>
    /// The stock definition of a <see cref="IMessageSubscriptionFactory"/>.
    /// </summary>
    public class StandardMessageSubscriptionFactory : NinjectComponent, IMessageSubscriptionFactory
    {
        #region IMessageSubscriptionFactory Members

        /// <summary>
        /// Creates a subscription for the specified channel.
        /// </summary>
        /// <param name="channel">The channel that will be subscribed to.</param>
        /// <param name="subscriber">The object that will receive events from the channel.</param>
        /// <param name="injector">The injector that will be called to trigger the event handler.</param>
        /// <param name="deliveryThread">The thread on which the subscription will be delivered.</param>
        /// <returns>The newly-created subscription.</returns>
        public IMessageSubscription Create( IMessageChannel channel, object subscriber, MethodInjector injector,
                                            DeliveryThread deliveryThread )
        {
            return new StandardMessageSubscription( channel, subscriber, injector, deliveryThread );
        }

        #endregion
    }
}