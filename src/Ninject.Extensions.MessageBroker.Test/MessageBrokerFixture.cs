// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

namespace Ninject.Extensions.MessageBroker
{
    using FluentAssertions;

    using Xunit;

    public class MessageBrokerFixture
    {
        [Fact]
        public void PublisherHasAListeners()
        {
            using (var kernel = this.CreateKernelWithMessageBrokerModule())
            {
                var pub = kernel.Get<PublisherMock>();

                pub.HasListeners.Should().BeTrue();
            }
        }

        [Fact]
        public void PublisherHasNoListenerWhenTheChannelIsClosed()
        {
            using (var kernel = this.CreateKernelWithMessageBrokerModule())
            {
                var pub = kernel.Get<PublisherMock>();
                var messageBroker = kernel.Components.Get<IMessageBroker>();

                messageBroker.CloseChannel("message://PublisherMock/MessageReceived");

                pub.HasListeners.Should().BeFalse();
            }
        }

        [Fact]
        public void SubscribersLastMessageIsInitiallyNull()
        {
            using (var kernel = this.CreateKernelWithMessageBrokerModule())
            {
                var sub = kernel.Get<SubscriberMock>();

                sub.LastMessage.Should().BeNull();
            }
        }
        
        [Fact]
        public void OnePublisherOneSubscriber()
        {
            using (var kernel = this.CreateKernelWithMessageBrokerModule())
            {
                var pub = kernel.Get<PublisherMock>();
                var sub = kernel.Get<SubscriberMock>();

                const string Message = "Hello, world!";
                pub.SendMessage(Message);

                sub.LastMessage.Should().Be(Message);
            }
        }

        [Fact]
        public void ManyPublishersOneSubscriber()
        {
            using (var kernel = this.CreateKernelWithMessageBrokerModule())
            {
                var pub1 = kernel.Get<PublisherMock>();
                var pub2 = kernel.Get<PublisherMock>();
                var sub = kernel.Get<SubscriberMock>();

                const string Message1 = "Hello, world!";
                pub1.SendMessage(Message1);

                sub.LastMessage.Should().Be(Message1);

                const string Message2 = "Hello, world2!";
                pub2.SendMessage(Message2);

                sub.LastMessage.Should().Be(Message2);
            }
        }

        [Fact]
        public void OnePublisherManySubscribers()
        {
            using (var kernel = this.CreateKernelWithMessageBrokerModule())
            {
                var pub = kernel.Get<PublisherMock>();
                var sub1 = kernel.Get<SubscriberMock>();
                var sub2 = kernel.Get<SubscriberMock>();

                const string Message = "Hello, world2!";
                pub.SendMessage(Message);

                sub1.LastMessage.Should().Be(Message);
                sub2.LastMessage.Should().Be(Message);
            }
        }

        [Fact]
        public void ManyPublishersManySubscribers()
        {
            using (var kernel = this.CreateKernelWithMessageBrokerModule())
            {
                var pub1 = kernel.Get<PublisherMock>();
                var pub2 = kernel.Get<PublisherMock>();
                var sub1 = kernel.Get<SubscriberMock>();
                var sub2 = kernel.Get<SubscriberMock>();

                const string Message1 = "Hello, world1!";
                pub1.SendMessage(Message1);

                sub1.LastMessage.Should().Be(Message1);
                sub2.LastMessage.Should().Be(Message1);

                const string Message2 = "Hello, world2!";
                pub2.SendMessage(Message2);

                sub1.LastMessage.Should().Be(Message2);
                sub2.LastMessage.Should().Be(Message2);
            }
        }

        [Fact]
        public void DisabledChannelsDoNotUnbindButEventsAreNotSent()
        {
            using (var kernel = this.CreateKernelWithMessageBrokerModule())
            {
                var pub = kernel.Get<PublisherMock>();
                var sub = kernel.Get<SubscriberMock>();

                var messageBroker = kernel.Components.Get<IMessageBroker>();
                messageBroker.DisableChannel("message://PublisherMock/MessageReceived");
                pub.SendMessage("Hello, world!");

                pub.HasListeners.Should().BeTrue();
                sub.LastMessage.Should().BeNull();
            }
        }

        [Fact]
        public void ClosingChannelUnbindsPublisherEventsFromChannel()
        {
            using (var kernel = this.CreateKernelWithMessageBrokerModule())
            {
                var pub = kernel.Get<PublisherMock>();
                var sub = kernel.Get<SubscriberMock>();

                var messageBroker = kernel.Components.Get<IMessageBroker>();
                messageBroker.CloseChannel("message://PublisherMock/MessageReceived");
                pub.SendMessage("Hello, world!");

                pub.HasListeners.Should().BeFalse();
                sub.LastMessage.Should().BeNull();
            }
        }

        private IKernel CreateKernelWithMessageBrokerModule()
        {
#if !SILVERLIGHT
            return new StandardKernel(new NinjectSettings { LoadExtensions = false }, new MessageBrokerModule());
#else
            return new StandardKernel(new MessageBrokerModule());
#endif
        }
    }
}