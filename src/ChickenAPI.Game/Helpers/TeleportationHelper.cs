﻿using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Helpers
{
    public static class TeleportationHelper
    {
        private static readonly IMapManager MapManager = new Lazy<IMapManager>(() => ChickenContainer.Instance.Resolve<IMapManager>()).Value;

        public static void TeleportTo(this IPlayerEntity player, IMapLayer layer, short x, short y)
        {
            if (player.CurrentMap == layer)
            {
                player.TeleportTo(x, y);
                return;
            }

            player.Position.X = x;
            player.Position.Y = y;
            player.TransferEntity(layer);
        }


        public static void TeleportTo(this IPlayerEntity player, short mapId, short x, short y)
        {
            player.TeleportTo(MapManager.GetBaseMapLayer(mapId), x, y);
        }

        public static void TeleportTo(this IPlayerEntity player, short x, short y)
        {
            // improve that
            player.Position.X = x;
            player.Position.Y = y;
            player.Broadcast(player.GenerateTpPacket(x, y));
        }
    }
}