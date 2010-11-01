// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

namespace Ninject.Extensions.MessageBroker
{
    public class SubscriberMock
    {
        public string LastMessage { get; set; }

        [Subscribe( "message://PublisherMock/MessageReceived" )]
        public void OnMessageReceived( object sender, MessageEventArgs args )
        {
            LastMessage = args.Message;
        }
    }
}