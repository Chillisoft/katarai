using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Hardcodet.Wpf.TaskbarNotification;
using Katarai.Controls;
using Katarai.Wpf.Extensions;
using Katarai.Wpf.Utils;
using Katarai.Wpf.ViewModels;

namespace Katarai.Wpf
{
    public class KataraiBootstrapper: BootstrapperBase
    {
        public KataraiBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var configurator = new ContainerConfigurator();
            configurator.Configure(_container);

            SetupTrayIcon();

            _applicationController = _container.GetInstance<IApplicationController>();
            _applicationController.Start();
        }

        private void SetupTrayIcon()
        {
            var windowManager = _container.GetInstance<IConvenientWindowManager>();
            _trayIconViewModel = _container.GetInstance<ITrayIconViewModel>();
            windowManager.LaunchHiddenWindow(_trayIconViewModel);

            var window = _trayIconViewModel.GetWindow();
            var trayIcon = window.Content as TaskbarIcon;
            _container.RegisterInstance(typeof (IToast), null, new Toast(trayIcon));
        }

        private SimpleContainer _container = new SimpleContainer();
        private IApplicationController _applicationController;
        private ITrayIconViewModel _trayIconViewModel;

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
