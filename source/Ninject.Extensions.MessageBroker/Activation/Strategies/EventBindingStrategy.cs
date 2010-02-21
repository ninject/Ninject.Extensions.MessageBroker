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