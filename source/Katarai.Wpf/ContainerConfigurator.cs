using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using Katarai.Controls;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;
using Katarai.Wpf.Utils;
using PeanutButter.TrayIcon;
using PeanutButter.Utils;

namespace Katarai.Wpf
{
    public class ContainerConfigurator
    {
        private List<Assembly> _autoAssemblies = new List<Assembly>();

        public ContainerConfigurator()
        {
            RegisterAllSingleImplementationsFrom(GetType().Assembly);
        }

        public void RegisterAllSingleImplementationsFrom(Assembly assembly)
        {
            if (_autoAssemblies.Contains(assembly))
                return;
            _autoAssemblies.Add(assembly);
        }

        public void Configure(SimpleContainer container)
        {
            RegisterManualInstances(container);
            RegisterSingletons(container);
            AutoRegisterOneToOneMappings(container);
        }

        private static void RegisterManualInstances(SimpleContainer container)
        {
            var trayIcon = new TrayIcon();
            container.RegisterInstance(typeof(ITrayIcon), null, trayIcon);
        }

        private Dictionary<Type, Type> _singletons = new Dictionary<Type, Type>()
        {
            { typeof(IConvenientWindowManager), typeof(ConvenientWindowManager) },
            { typeof(IEventAggregator), typeof(EventAggregator) },
            { typeof(ISettingsManager), typeof(SettingsManager) },
            { typeof(IKataraiApp), typeof(KataraiApp) },
            { typeof(IPlayerNotifier), typeof(PlayerNotifier) },
            { typeof(IToast), typeof(Toast) }
        }; 

        private void RegisterSingletons(SimpleContainer container)
        {
            _singletons.ForEach(kvp =>
            {
                Console.WriteLine($"Registering Singleton: {kvp.Value.Name} for {kvp.Key.Name}");
                container.RegisterSingleton(kvp.Key, null, kvp.Value);
            });
        }

        private void AutoRegisterOneToOneMappings(SimpleContainer container)
        {
            _autoAssemblies.ForEach(asm =>
            {
                var allTypes = asm.GetTypes();
                var interfaces = allTypes.Where(t => t.IsInterface);
                interfaces.ForEach(iface =>
                {
                    var implementingTypes = allTypes.Where(t => iface.IsAssignableFrom(t) &&
                                                                !t.IsInterface &&
                                                                !t.IsAbstract);
                    if (implementingTypes.Count() != 1)
                        return; // can't be auto about it;
                    var implementation = implementingTypes.First();
                    if (container.GetInstance(iface, null) != null)
                        return; // already registered somewhere else
                    Console.WriteLine($"Registering PerRequest: {implementation.Name} for {iface.Name}");
                    container.RegisterPerRequest(iface, null, implementation);
                });
            });
        }
    }
}