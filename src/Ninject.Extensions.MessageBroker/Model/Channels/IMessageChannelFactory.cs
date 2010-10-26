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

using Ninject.Components;

#endregion

namespace Ninject.Extensions.MessageBroker.Model.Channels
{
    /// <summary>
    /// Creates <see cref="IMessageChannel"/>s.
    /// </summary>
    public interface IMessageChannelFactory : INinjectComponent
    {
        /// <summary>
        /// Creates a channel with the specified name.
        /// </summary>
        /// <param name="name">The channel's name.</param>
        /// <returns>The newly-created channel.</returns>
        IMessageChannel Create( string name );
    }
}