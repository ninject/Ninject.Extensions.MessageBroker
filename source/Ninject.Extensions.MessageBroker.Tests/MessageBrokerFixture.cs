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

using Ninject.Extensions.MessageBroker.Model.Channels;
using Xunit;

#endregion

namespace Ninject.Extensions.MessageBroker.Tests
{
    public class MessageBrokerFixture
    {
        [Fact]
        public void OnePublisherOneSubscriber()
        {
            using ( var kernel = new StandardKernel() )
            {
                var pub = kernel.Get<PublisherMock>();
                Assert.NotNull( pub );

                var sub = kernel.Get<SubscriberMock>();
                Assert.NotNull( sub );

                Assert.True( pub.HasListeners );
                Assert.Null( sub.LastMessage );

                pub.SendMessage( "Hello, world!" );

                Assert.Equal( sub.LastMessage, "Hello, world!" );
            }
        }

        [Fact]
        public void ManyPublishersOneSubscriber()
        {
            using ( var kernel = new StandardKernel() )
            {
                var pub1 = kernel.Get<PublisherMock>();
                var pub2 = kernel.Get<PublisherMock>();
                Assert.NotNull( pub1 );
                Assert.NotNull( pub2 );

                var sub = kernel.Get<SubscriberMock>();
                Assert.NotNull( sub );

                Assert.True( pub1.HasListeners );
                Assert.True( pub2.HasListeners );
                Assert.Null( sub.LastMessage );

                pub1.SendMessage( "Hello, world!" );
                Assert.Equal( sub.LastMessage, "Hello, world!" );

                sub.LastMessage = null;
                Assert.Null( sub.LastMessage );

                pub2.SendMessage( "Hello, world!" );
                Assert.Equal( sub.LastMessage, "Hello, world!" );
            }
        }

        [Fact]
        public void OnePublisherManySubscribers()
        {
            using ( var kernel = new StandardKernel() )
            {
                var pub = kernel.Get<PublisherMock>();
                Assert.NotNull( pub );

                var sub1 = kernel.Get<SubscriberMock>();
                var sub2 = kernel.Get<SubscriberMock>();
                Assert.NotNull( sub1 );
                Assert.NotNull( sub2 );

                Assert.True( pub.HasListeners );
                Assert.Null( sub1.LastMessage );
                Assert.Null( sub2.LastMessage );

                pub.SendMessage( "Hello, world!" );
                Assert.Equal( sub1.LastMessage, "Hello, world!" );
                Assert.Equal( sub2.LastMessage, "Hello, world!" );
            }
        }

        [Fact]
        public void ManyPublishersManySubscribers()
        {
            using ( var kernel = new StandardKernel() )
            {
                var pub1 = kernel.Get<PublisherMock>();
                var pub2 = kernel.Get<PublisherMock>();
                Assert.NotNull( pub1 );
                Assert.NotNull( pub2 );

                var sub1 = kernel.Get<SubscriberMock>();
                var sub2 = kernel.Get<SubscriberMock>();
                Assert.NotNull( sub1 );
                Assert.NotNull( sub2 );

                Assert.True( pub1.HasListeners );
                Assert.True( pub2.HasListeners );
                Assert.Null( sub1.LastMessage );
                Assert.Null( sub2.LastMessage );

                pub1.SendMessage( "Hello, world!" );
                Assert.Equal( sub1.LastMessage, "Hello, world!" );
                Assert.Equal( sub2.LastMessage, "Hello, world!" );

                sub1.LastMessage = null;
                sub2.LastMessage = null;
                Assert.Null( sub1.LastMessage );
                Assert.Null( sub2.LastMessage );

                pub2.SendMessage( "Hello, world!" );
                Assert.Equal( sub1.LastMessage, "Hello, world!" );
                Assert.Equal( sub2.LastMessage, "Hello, world!" );
            }
        }

        [Fact]
        public void DisabledChannelsDoNotUnbindButEventsAreNotSent()
        {
            using ( var kernel = new StandardKernel() )
            {
                var pub = kernel.Get<PublisherMock>();
                Assert.NotNull( pub );

                var sub = kernel.Get<SubscriberMock>();
                Assert.NotNull( sub );

                Assert.Null( sub.LastMessage );

                var messageBroker = kernel.Components.Get<IMessageBroker>();
                messageBroker.DisableChannel( "message://PublisherMock/MessageReceived" );
                Assert.True( pub.HasListeners );

                pub.SendMessage( "Hello, world!" );
                Assert.Null( sub.LastMessage );
            }
        }

        [Fact]
        public void ClosingChannelUnbindsPublisherEventsFromChannel()
        {
            using ( var kernel = new StandardKernel() )
            {
                var pub = kernel.Get<PublisherMock>();
                Assert.NotNull( pub );

                var sub = kernel.Get<SubscriberMock>();
                Assert.NotNull( sub );

                Assert.Null( sub.LastMessage );

                var messageBroker = kernel.Components.Get<IMessageBroker>();
                messageBroker.CloseChannel( "message://PublisherMock/MessageReceived" );
                Assert.False( pub.HasListeners );

                pub.SendMessage( "Hello, world!" );
                Assert.Null( sub.LastMessage );
            }
        }

        [Fact]
        public void DisposingObjectRemovesSubscriptionsRequestedByIt()
        {
            using ( var kernel = new StandardKernel() )
            {
                var pub = kernel.Get<PublisherMock>();
                Assert.NotNull( pub );

                var sub = kernel.Get<SubscriberMock>();
                Assert.NotNull( sub );

                var messageBroker = kernel.Components.Get<IMessageBroker>();
                IMessageChannel channel = messageBroker.GetChannel( "message://PublisherMock/MessageReceived" );
                Assert.Equal( channel.Subscriptions.Count, 1 );

                // TODO: This no longer works.
                //kernel.Release( sub );

                Assert.Empty( channel.Subscriptions );
            }
        }
    }
}