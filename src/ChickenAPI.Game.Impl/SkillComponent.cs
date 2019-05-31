﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game.Skills.Extensions;
using ChickenAPI.Packets.Enumerations;

namespace ChickenAPI.Game.Skills
{
    public class SkillComponent : IComponent, ISkillCapacity
    {
        private static readonly ISkillService SkillService = new Lazy<ISkillService>(() => ChickenContainer.Instance.Resolve<ISkillService>()).Value;

        public SkillComponent(ISkillEntity entity)
        {
            Entity = entity;

            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            int tmp = 200 + 20 * (byte)player.Character.Class;
            entity.AddSkill(SkillService.GetById(tmp));
            entity.AddSkill(SkillService.GetById(tmp + 1));

            if (player.Character.Class != CharacterClassType.Adventurer)
            {
                return;
            }

            entity.AddSkill(SkillService.GetById(tmp + 9));

            IEnumerable<SkillDto> skills = SkillService.GetByClassIdAsync((byte)player.Character.Class).ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (SkillDto skillDto in skills.Where(s => s.LevelMinimum < player.JobLevel && s.Id >= 200 && s.Id != 209 && s.Id <= 210))
            {
                entity.AddSkill(skillDto);
            }
        }

        public SkillComponent(ISkillEntity entity, IEnumerable<NpcMonsterSkillDto> skills) : this(entity)
        {
            if (skills == null)
            {
                return;
            }

            // add skills to component
            foreach (NpcMonsterSkillDto skill in skills)
            {
                entity.AddSkill(skill.Skill);
            }
        }

        public SkillComponent(ISkillEntity entity, IEnumerable<CharacterSkillDto> skills) : this(entity)
        {
            if (skills == null)
            {
                return;
            }

            foreach (CharacterSkillDto characterSkill in skills)
            {
                CharacterSkills.Add(characterSkill.Id, characterSkill);
                entity.AddSkill(characterSkill.Skill);
            }
        }

        public Dictionary<Guid, CharacterSkillDto> CharacterSkills { get; } = new Dictionary<Guid, CharacterSkillDto>();

        public IDictionary<long, SkillDto> Skills { get; } = new Dictionary<long, SkillDto>();

        public IDictionary<long, SkillDto> SkillsByCastId { get; } = new Dictionary<long, SkillDto>();

        public List<(DateTime, long)> CooldownsBySkillId { get; } = new List<(DateTime, long)>();
        public bool HasSkill(long skillId) => Skills.ContainsKey(skillId);

        public bool CanCastSkill(long skillId) => CooldownsBySkillId.Any(s => s.Item2 == skillId);

        public IEntity Entity { get; }
    }
}