﻿using System;
using System.Collections.Concurrent;
using ChickenAPI.Game._Network;

namespace World.Network
{
    public class SocketSessionManager
    {
        private readonly ConcurrentDictionary<string, ISession> _sessions = new ConcurrentDictionary<string, ISession>();

        public void RegisterSession(string key, ISession sessionId)
        {
            _sessions.TryAdd(key, sessionId);
        }

        public bool GetSession(string key, out ISession sessionId) => _sessions.TryGetValue(key, out sessionId);

        public void UnregisterSession(string key)
        {
            _sessions.TryRemove(key, out ISession _);
        }

        #region Singleton

        private static readonly Lazy<SocketSessionManager> LazyInstance = new Lazy<SocketSessionManager>(() => new SocketSessionManager());

        public static SocketSessionManager Instance => LazyInstance.Value;

        #endregion
    }
}