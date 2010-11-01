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

    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs( string message )
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}