using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using StructureMap;
using StructureMap.Web.Pipeline;

namespace MyApp.Framework.Infrastructure
{
    public static class IoC
    {
        #region Fields

        private static readonly Lazy<Container> Bootstrapper =
            new Lazy<Container>(Initialize, LazyThreadSafetyMode.ExecutionAndPublication);

        #endregion

        #region Properties

        internal static IContainer Container => Bootstrapper.Value;

        #endregion

        #region Methods

        private static Container Initialize()
        {
            var container = new Container(ioc =>
            {
                ioc.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.AssembliesFromApplicationBaseDirectory();
                    scan.LookForRegistries();
                });
            });

            // container.AssertConfigurationIsValid();

            return container;
        }

        public static IList<T> GetAllInstances<T>()
        {
            return Container.GetAllInstances<T>().ToList();
        }

        public static T Resolve<T>()
        {
            return Container.GetInstance<T>();
        }

        public static T Resolve<T>(string name)
        {
            return Container.GetInstance<T>(name);
        }

        public static void BuildUp(object target)
        {
            Container.BuildUp(target);
        }

        /// <summary>
        ///     a simple helper method that return a string with the contents
        ///     (resolved objects) in the container. Really helpful when troubleshooting
        /// </summary>
        /// <returns></returns>
        public static string WhatDoIHave()
        {
            return Container.WhatDoIHave();
        }

        public static void ReleasePerRequesInstances()
        {
            HttpContext.Current.Items.Clear();
            HttpContextLifecycle.DisposeAndClearAll();
        }

        public static void RegisterAssembly(Assembly assembly)
        {
            Container.Configure(a => { a.Scan(scan => scan.Assembly(assembly)); });
        }

        #endregion
    }
}