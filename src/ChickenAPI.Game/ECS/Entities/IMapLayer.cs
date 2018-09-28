﻿using System;
using System.Collections.Generic;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.ECS.Entities
{
    public interface IMapLayer : IEntityManager
    {
        Guid Id { get; }

        /// <summary>
        ///     Get the base map of the layer
        /// </summary>
        IMap Map { get; }

        /// <summary>
        ///     Get all entities in the area between X and Y
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        IEnumerable<IEntity> GetEntitiesInRange(Position<short> pos, int range);

        #region Packets

        /// <summary>
        ///     Broadcast a packet to every entities in the context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packet"></param>
        void Broadcast<T>(T packet) where T : IPacket;

        void Broadcast<T>(IEnumerable<T> packets) where T : IPacket;

        void Broadcast(IEnumerable<IPacket> packets);

        /// <summary>
        ///     Broadcast a packet to every entities in the context except
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="packet"></param>
        void Broadcast<T>(IPlayerEntity sender, T packet) where T : IPacket;

        void Broadcast<T>(IPlayerEntity sender, IEnumerable<T> packets) where T : IPacket;

        #endregion
    }
}