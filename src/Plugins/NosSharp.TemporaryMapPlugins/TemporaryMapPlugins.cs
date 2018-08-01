﻿using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.AccessLayer.Item;
using ChickenAPI.Managers;
using ChickenAPI.Plugins;

namespace NosSharp.TemporaryMapPlugins
{
    public class TemporaryMapPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<TemporaryMapPlugin>();
        public string Name => nameof(TemporaryMapPlugin);

        public void OnDisable()
        {
            // nothing
        }

        public void OnEnable()
        {
            // nothing
        }

        public void OnLoad()
        {
            Log.Info("Loading...");
            Container.Builder.Register(s => new LazyMapManager()).As<IMapManager>().SingleInstance();
            Container.Builder.Register(c => new SimpleItemInstanceFactory(c.Resolve<IItemService>())).As<IItemInstanceFactory>();
            Log.Info("Loaded !");
        }

        public void ReloadConfig()
        {
            // nothing
        }

        public void SaveConfig()
        {
            // nothing
        }

        public void SaveDefaultConfig()
        {
            // nothing
        }
    }
}