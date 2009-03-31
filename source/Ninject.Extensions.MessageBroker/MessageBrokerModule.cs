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