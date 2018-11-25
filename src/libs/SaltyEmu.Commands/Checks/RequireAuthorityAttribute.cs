﻿using System;
using System.Threading.Tasks;
using ChickenAPI.Enums;
using ChickenAPI.Game.Entities.Player;
using Qmmands;
using SaltyEmu.Commands.Entities;

namespace SaltyEmu.Commands.Checks
{
    public sealed class RequireAuthorityAttribute : CheckBaseAttribute
    {
        /// <summary>
        /// This represents the Authority level required to execute a command.
        /// </summary>
        public AuthorityType Authority { get; }

        public RequireAuthorityAttribute(AuthorityType authority)
        {
            Authority = authority;
        }

        /// <inheritdoc />
        /// <summary>
        /// This is a check (pre-condition) before trying to execute a command that needs to pass this check.
        /// </summary>
        /// <param name="context">Context of the command. It needs to be castable to a SaltyCommandContext in our case.</param>
        /// <returns></returns>
        public override Task<CheckResult> CheckAsync(ICommandContext context, IServiceProvider _)
        {
            if (!(context is SaltyCommandContext ctx))
            {
                return Task.FromResult(new CheckResult("Invalid context."));
            }

            if (ctx.Sender is IPlayerEntity player && player.Session.Account.Authority < AuthorityType.GameMaster)
            {
                return Task.FromResult(new CheckResult("You need to be GameMaster"));
            }

            return Task.FromResult(CheckResult.Successful);
        }
    }
}