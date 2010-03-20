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

using Ninject.Activation.Strategies;
using Ninject.Extensions.MessageBroker.Activation.Strategies;
using Ninject.Extensions.MessageBroker.Model.Channels;
using Ninject.Extensions.MessageBroker.Model.Publications;
using Ninject.Extensions.MessageBroker.Model.Subscriptions;
using Ninject.Extensions.MessageBroker.Planning.Strategies;
using Ninject.Modules;
using Ninject.Planning.Strategies;

#endregion

namespace Ninject.Extensions.MessageBroker
{
    /// <summary>
    /// Adds functionality to the kernel to support channel-based pub/sub messaging.
    /// </summary>
    public class MessageBrokerModule : NinjectModule
    {
        #region Properties

        /// <summary>
        /// Gets or sets the message broker.
        /// </summary>
        public IMessageBroker MessageBroker { get; set; }

        /// <summary>
        /// Gets or sets the channel factory.
        /// </summary>
        public IMessageChannelFactory ChannelFactory { get; set; }

        /// <summary>
        /// Gets or sets the publication factory.
        /// </summary>
        public IMessagePublicationFactory PublicationFactory { get; set; }

        /// <summary>
        /// Gets or sets the subscription factory.
        /// </summary>
        public IMessageSubscriptionFactory SubscriptionFactory { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Kernel.Components.Add<IPlanningStrategy, EventReflectionStrategy>();
            Kernel.Components.Add<IActivationStrategy, EventBindingStrategy>();

            Kernel.Components.Add<IMessageBroker, StandardMessageBroker>();
            Kernel.Components.Add<IMessageChannelFactory, StandardMessageChannelFactory>();
            Kernel.Components.Add<IMessagePublicationFactory, StandardMessagePublicationFactory>();
            Kernel.Components.Add<IMessageSubscriptionFactory, StandardMessageSubscriptionFactory>();
        }

        /// <summary>
        /// Unloads the module from the kernel.
        /// </summary>
        public override void Unload()
        {
            Kernel.Components.RemoveAll<EventReflectionStrategy>();
            Kernel.Components.RemoveAll<EventBindingStrategy>();
            Kernel.Components.RemoveAll<IMessageBroker>();
            Kernel.Components.RemoveAll<IMessageChannelFactory>();
            Kernel.Components.RemoveAll<IMessagePublicationFactory>();
            Kernel.Components.RemoveAll<IMessageSubscriptionFactory>();
        }

        #endregion
    }
}