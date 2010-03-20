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
using Ninject.Infrastructure.Disposal;

#endregion

namespace Ninject.Extensions.MessageBroker
{
    internal static class Ensure
    {
        public static void ArgumentNotNull( object argument, string name )
        {
            if ( argument == null )
            {
                throw new ArgumentNullException( name, "Cannot be null" );
            }
        }

        public static void ArgumentNotNullOrEmpty( string argument, string name )
        {
            if ( String.IsNullOrEmpty( argument ) )
            {
                throw new ArgumentException( "Cannot be null or empty", name );
            }
        }

        /// <summary>
        /// Throws an exception if the specified object has been disposed.
        /// </summary>
        /// <param name="obj">The object in question.</param>
        public static void NotDisposed( DisposableObject obj )
        {
            if ( obj.IsDisposed )
            {
                throw new ObjectDisposedException( obj.GetType().Name );
            }
        }
    }
}