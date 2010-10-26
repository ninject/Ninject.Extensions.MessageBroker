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

using System.Collections.Generic;
using System.Reflection;
using Ninject.Components;
using Ninject.Extensions.MessageBroker.Planning.Directives;
using Ninject.Infrastructure.Language;
using Ninject.Injection;
using Ninject.Planning;
using Ninject.Planning.Strategies;
using Ninject.Selection;

#endregion

namespace Ninject.Extensions.MessageBroker.Planning.Strategies
{
    /// <summary>
    /// A planning strategy that examines types via reflection to determine if there are any
    /// message publications or subscriptions defined.
    /// </summary>
    public class EventReflectionStrategy : NinjectComponent, IPlanningStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventReflectionStrategy"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="injectorFactory">The injector factory.</param>
        public EventReflectionStrategy( ISelector selector, IInjectorFactory injectorFactory )
        {
            Ensure.ArgumentNotNull( selector, "selector" );
            Ensure.ArgumentNotNull( injectorFactory, "injectorFactory" );

            Selector = selector;
            InjectorFactory = injectorFactory;
        }

        #region Implementation of IPlanningStrategy

        /// <summary>
        /// Contributes to the specified plan.
        /// </summary>
        /// <param name="plan">The plan that is being generated.</param>
        public void Execute( IPlan plan )
        {
            EventInfo[] events =
                plan.Type.GetEvents( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );

            foreach ( EventInfo evt in events )
            {
                IEnumerable<PublishAttribute> attributes = evt.GetAttributes<PublishAttribute>();

                foreach ( PublishAttribute attribute in attributes )
                {
                    plan.Add( new PublicationDirective( attribute.Channel, evt ) );
                }
            }

            MethodInfo[] methods =
                plan.Type.GetMethods( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );

            foreach ( MethodInfo method in methods )
            {
                IEnumerable<SubscribeAttribute> attributes = method.GetAttributes<SubscribeAttribute>();

                foreach ( SubscribeAttribute attribute in attributes )
                {
                    MethodInjector injector = InjectorFactory.Create( method );
                    plan.Add( new SubscriptionDirective( attribute.Channel, injector, attribute.Thread ) );
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets the selector component.
        /// </summary>
        public ISelector Selector { get; private set; }

        /// <summary>
        /// Gets the injector factory component.
        /// </summary>
        public IInjectorFactory InjectorFactory { get; set; }
    }
}