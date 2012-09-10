namespace Creelio.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;

    public sealed class SettingsManager<TSettings, TInterface>
        where TSettings : ApplicationSettingsBase, TInterface
    {
        private static Lazy<IEnumerable<PropertyInfo>> _settingsProperties =
            new Lazy<IEnumerable<PropertyInfo>>(SelectSettingsProperties);

        public SettingsManager(TSettings settings)
        {
            Settings = settings;
        }

        public TInterface Settings { get; set; }

        private static IEnumerable<PropertyInfo> SettingsProperties
        {
            get
            {
                return _settingsProperties.Value;
            }
        }

        public void Load(TInterface target)
        {
            SetSettingsProperties(target, Settings);
        }

        public void Save(TInterface source)
        {
            SetSettingsProperties(Settings, source);
            (Settings as ApplicationSettingsBase).Save();
        }

        private static void SetSettingsProperties(TInterface target, TInterface source)
        {
            foreach (var property in SettingsProperties)
            {
                var value = property.GetValue(source, null);
                property.SetValue(target, value, null);
            }
        }

        private static IEnumerable<PropertyInfo> SelectSettingsProperties()
        {
            var type = typeof(TInterface);
            return from p in type.GetProperties()
                   where p.DeclaringType == type
                   select p;
        }
    }
}