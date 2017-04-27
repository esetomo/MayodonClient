using MayodonClient.Models;
using MayodonClient.Views;
using Microsoft.Practices.ServiceLocation;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MayodonClient
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Bootstrap();

            var mainWindow = ServiceLocator.Current.GetInstance<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        private void Bootstrap()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var container = new Container();
            container.RegisterSingleton(Config.Load());
            container.Verify();

            ServiceLocator.SetLocatorProvider(() => new Locator(container));
        }

        private class Locator : IServiceLocator
        {
            private readonly Container container;

            public Locator(Container container)
            {
                this.container = container;
            }

            public IEnumerable<object> GetAllInstances(Type serviceType)
            {
                return container.GetAllInstances(serviceType);
            }

            public IEnumerable<TService> GetAllInstances<TService>()
            {
                return GetAllInstances(typeof(TService)).OfType<TService>();
            }

            public object GetInstance(Type serviceType)
            {
                return container.GetInstance(serviceType);
            }

            public object GetInstance(Type serviceType, string key)
            {
                throw new NotImplementedException();

            }

            public TService GetInstance<TService>()
            {
                return (TService)GetInstance(typeof(TService));
            }

            public TService GetInstance<TService>(string key)
            {
                throw new NotImplementedException();
            }

            public object GetService(Type serviceType)
            {
                return GetInstance(serviceType);
            }
        }
    }
}
