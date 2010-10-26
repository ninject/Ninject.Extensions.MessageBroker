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

using System;
using System.Reflection;
using Ninject.Extensions.MessageBroker.Model.Channels;

#endregion

namespace Ninject.Extensions.MessageBroker.Model.Publications
{
    /// <summary>
    /// A message publication handled by a message broker.
    /// </summary>
    public interface IMessagePublication : IDisposable
    {
        /// <summary>
        /// Gets the channel associated with the publication.
        /// </summary>
        IMessageChannel Channel { get; }


        /// <summary>
        /// Gets the object that publishes events to the channel.
        /// </summary>
        object Publisher { get; }


        /// <summary>
        /// Gets the event that will be published to the channel.
        /// </summary>
        EventInfo Event { get; }
    }
}