﻿using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Groups.Events
{
    public class GroupInvitationRefuseEvent : GameEntityEvent
    {
        public GroupInvitDto Invitation { get; set; }
    }
}