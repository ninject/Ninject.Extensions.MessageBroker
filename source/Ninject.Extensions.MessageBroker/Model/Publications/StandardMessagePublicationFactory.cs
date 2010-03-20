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

using System.Reflection;
using Ninject.Components;
using Ninject.Extensions.MessageBroker.Model.Channels;

#endregion

namespace Ninject.Extensions.MessageBroker.Model.Publications
{
    /// <summary>
    /// Creates <see cref="IMessagePublication"/>s.
    /// </summary>
    public class StandardMessagePublicationFactory : NinjectComponent, IMessagePublicationFactory
    {
        #region IMessagePublicationFactory Members

        /// <summary>
        /// Creates a publication for the specified channel.
        /// </summary>
        /// <param name="channel">The channel that will receive the publications.</param>
        /// <param name="publisher">The object that will publish events.</param>
        /// <param name="evt">The event that will be published to the channel.</param>
        /// <returns>The newly-created publicaton.</returns>
        public IMessagePublication Create( IMessageChannel channel, object publisher, EventInfo evt )
        {
            return new StandardMessagePublication( channel, publisher, evt );
        }

        #endregion
    }
}