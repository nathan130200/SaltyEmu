﻿using System;
using System.Linq.Expressions;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.ECS.Systems;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Movements;
using ChickenAPI.Game.Movements.DataObjects;

namespace ChickenAPI.Game.IAs
{
    public class IASystem : SystemBase
    {
        private readonly IMap _map;
        private readonly IPathfinder _pathfinder;
        private readonly IRandomGenerator _random;

        public IASystem(IEntityManager entityManager, IMap map) : base(entityManager)
        {
            _map = map;
            _pathfinder = ChickenContainer.Instance.Resolve<IPathfinder>();
            _random = ChickenContainer.Instance.Resolve<IRandomGenerator>();
        }

        protected override double RefreshRate => 0.45;

        protected override Expression<Func<IEntity, bool>> Filter => entity => MovableFilter(entity);

        private static bool MovableFilter(IEntity entity)
        {
            if (entity.Type == VisualType.Character)
            {
                return false;
            }


            return entity is IMovableEntity;
        }

        protected override void Execute(IEntity entity)
        {
            int i = 0;

            if (!(entity is IMovableEntity mov))
            {
                return;
            }

            MovableComponent movableComponent = mov.Movable;
            if (movableComponent.Waypoints != null && movableComponent.Waypoints.Length != 0 || entity.Type == VisualType.Character || movableComponent.Speed == 0)
            {
                return;
            }

            if (_random.Next(0, 100) < 35)
            {
                // wait max 2500 millisecs before having a new movement
                movableComponent.LastMove = DateTime.UtcNow.AddMilliseconds(_random.Next(2500));
                return;
            }

            Position<short> dest = null;
            while (dest == null && i < 25)
            {
                short xpoint = (short)_random.Next(0, 4);
                short ypoint = (short)_random.Next(0, 4);
                short firstX = movableComponent.Actual.X;
                short firstY = movableComponent.Actual.Y;
                dest = _map.GetFreePosition(firstX, firstY, xpoint, ypoint);

                i++;
            }

            if (dest == null)
            {
                return;
            }

            movableComponent.Waypoints = _pathfinder.FindPath(movableComponent.Actual, dest, _map);
        }
    }
}