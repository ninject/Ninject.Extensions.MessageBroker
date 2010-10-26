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

using System.Collections.Generic;
using System.Linq;
using Ninject.Activation;
using Ninject.Activation.Strategies;
using Ninject.Extensions.MessageBroker.Model.Channels;
using Ninject.Extensions.MessageBroker.Planning.Directives;

#endregion

namespace Ninject.Extensions.MessageBroker.Activation.Strategies
{
    /// <summary>
    /// An activation strategy that binds events to message channels after instances are initialized,
    /// and unbinds them before they are destroyed.
    /// </summary>
    public class EventBindingStrategy : ActivationStrategy
    {
        /// <summary>
        /// Activates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference.</param>
        public override void Activate( IContext context, InstanceReference reference )
        {
            var messageBroker = context.Kernel.Components.Get<IMessageBroker>();

            List<PublicationDirective> publications = context.Plan.GetAll<PublicationDirective>().ToList();

            // I don't think this is needed in Ninject2
            //if (publications.Count > 0)
            //   context.ShouldTrackInstance = true;

            foreach ( PublicationDirective publication in publications )
            {
                IMessageChannel channel = messageBroker.GetChannel( publication.Channel );
                channel.AddPublication( reference.Instance, publication.Event );
            }

            List<SubscriptionDirective> subscriptions = context.Plan.GetAll<SubscriptionDirective>().ToList();

            // I don't think this is needed in Ninject2
            //if (subscriptions.Count > 0)
            //    context.ShouldTrackInstance = true;

            foreach ( SubscriptionDirective subscription in subscriptions )
            {
                IMessageChannel channel = messageBroker.GetChannel( subscription.Channel );
                channel.AddSubscription( reference.Instance, subscription.Injector, subscription.Thread );
            }
        }

        /// <summary>
        /// Deactivates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference.</param>
        public override void Deactivate( IContext context, InstanceReference reference )
        {
            var messageBroker = context.Kernel.Components.Get<IMessageBroker>();

            IEnumerable<PublicationDirective> publications = context.Plan.GetAll<PublicationDirective>();

            foreach ( PublicationDirective publication in publications )
            {
                IMessageChannel channel = messageBroker.GetChannel( publication.Channel );
                channel.RemovePublication( reference.Instance, publication.Event );
            }

            IEnumerable<SubscriptionDirective> subscriptions = context.Plan.GetAll<SubscriptionDirective>();

            foreach ( SubscriptionDirective subscription in subscriptions )
            {
                IMessageChannel channel = messageBroker.GetChannel( subscription.Channel );
                channel.RemoveSubscription( reference.Instance, subscription.Injector );
            }
        }
    }
}