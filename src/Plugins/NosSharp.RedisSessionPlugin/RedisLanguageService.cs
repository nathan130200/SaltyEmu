﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.i18n;
using SaltyEmu.RedisWrappers.Languages;
using SaltyEmu.RedisWrappers.Redis;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace SaltyEmu.RedisWrappers
{
    public class RedisLanguageService : ILanguageService
    {
        private readonly IRedisTypedClient<string> _client;

        public RedisLanguageService(RedisConfiguration configuration)
        {
            _client = new RedisClient(new RedisEndpoint
            {
                Host = configuration.Host,
                Port = configuration.Port,
                Password = configuration.Password
            }).As<string>();
        }

        private Dictionary<ChickenI18NKey, string> GetSetByLanguageKey(LanguageKey key)
        {
            switch (key)
            {
                case LanguageKey.EN:
                    return EnglishI18n.Languages;
                default:
                    return null;
            }
        }

        public string GetLanguage(string key, LanguageKey language)
        {
            return null;
        }

        public string GetLanguage(ChickenI18NKey key, LanguageKey type)
        {
            if (!GetSetByLanguageKey(type).TryGetValue(key, out string value))
            {
                value = type + "_" + key;
            }

            return value;
        }

        public void SetLanguage(string key, string value, LanguageKey type)
        {
            throw new NotImplementedException();
        }

        public void SetLanguage(ChickenI18NKey key, string value, LanguageKey type)
        {
            GetSetByLanguageKey(type)[key] = value;
        }
    }
}