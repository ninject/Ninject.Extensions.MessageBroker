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
using System.Text;
using Ninject.Injection;
using Ninject.Planning.Directives;

#endregion

namespace Ninject.Extensions.MessageBroker.Planning.Directives
{
    /// <summary>
    /// A directive that describes a message subscription.
    /// </summary>
    public class SubscriptionDirective : IDirective
    {
        #region Fields

        private readonly string _channel;
        private readonly MethodInjector _injector;
        private readonly DeliveryThread _thread;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the channel that is to be susbcribed to.
        /// </summary>
        public string Channel
        {
            get { return _channel; }
        }


        /// <summary>
        /// Gets the injector that triggers the method.
        /// </summary>
        public MethodInjector Injector
        {
            get { return _injector; }
        }


        /// <summary>
        /// Gets the thread on which the message should be delivered.
        /// </summary>
        public DeliveryThread Thread
        {
            get { return _thread; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionDirective"/> class.
        /// </summary>
        /// <param name="channel">The name of the channel that is to be susbcribed to.</param>
        /// <param name="injector">The injector that triggers the method.</param>
        /// <param name="thread">The thread on which the message should be delivered.</param>
        public SubscriptionDirective( string channel, MethodInjector injector, DeliveryThread thread )
        {
            Ensure.ArgumentNotNullOrEmpty( channel, "channel" );
            Ensure.ArgumentNotNull( injector, "injector" );

            _channel = channel;
            _injector = injector;
            _thread = thread;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Builds the value that uniquely identifies the directive. This is called the first time
        /// the key is accessed, and then cached in the directive.
        /// </summary>
        /// <returns>The directive's unique key.</returns>
        /// <remarks>
        /// This exists because most directives' keys are based on reading member information,
        /// especially parameters. Since it's a relatively expensive procedure, it shouldn't be
        /// done each time the key is accessed.
        /// </remarks>
        protected object BuildKey()
        {
            var sb = new StringBuilder();

            sb.Append( _channel );
            sb.Append( _injector.Method.Name );

            ParameterInfo[] parameters = _injector.Method.GetParameters();
            foreach ( ParameterInfo parameter in parameters )
            {
                sb.Append( parameter.ParameterType.FullName );
            }

            return sb.ToString();
        }

        #endregion
    }
}