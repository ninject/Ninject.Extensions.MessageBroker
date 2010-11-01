// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

namespace Ninject.Extensions.MessageBroker
{
    using System;

    public class PublisherMock
    {
        public bool HasListeners
        {
            get { return ( MessageReceived != null ); }
        }

        [Publish( "message://PublisherMock/MessageReceived" )]
        public event EventHandler<MessageEventArgs> MessageReceived;

        public void SendMessage( string message )
        {
            EventHandler<MessageEventArgs> evt = MessageReceived;

            if ( evt != null )
            {
                evt( this, new MessageEventArgs( message ) );
            }
        }
    }
}