﻿using System;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Data.AccessLayer;
using ChickenAPI.Data.TransferObjects;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Utils;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace NosSharp.World.Network
{
    public class Server
    {
        private static readonly Logger Log = Logger.GetLogger<Server>();
        public static WorldServerDto WorldServer;
        private static bool _running { get; set; }
        public static string WorldGroup { get; set; }
        public static string Ip { get; set; }
        public static int Port { get; set; }

        public static bool RegisterServer()
        {
            var worldServer = new WorldServerDto
            {
                WorldGroup = WorldGroup,
                Ip = Ip,
                Port = Port,
                Color = ChannelColor.White,
                Id = Guid.Empty,
                ChannelId = 0
            };
            var api = Container.Instance.Resolve<IServerApiService>();
            if (api?.RegisterServer(worldServer) == true)
            {
                return true;
            }

            WorldServer = worldServer;
            ClientSession.SetWorldServerId(WorldServer.Id);
            _running = true;
            Log.Info($"Registering server {WorldServer.Id}");
            return false;
        }

        public static void UnregisterServer()
        {
            if (_running == false)
            {
                return;
            }

            var api = Container.Instance.Resolve<IServerApiService>();
            if (api == null)
            {
                return;
            }

            Log.Info($"Unregister server {WorldServer.Id}");

            var sessionManager = Container.Instance.Resolve<ISessionService>();
            api.UnregisterServer(WorldServer.Id);
            sessionManager.UnregisterSessions(WorldServer.Id);
            _running = false;
        }

        public static async Task RunServerAsync(int port)
        {
            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap
                    .Option(ChannelOption.SoBacklog, 100)
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .ChildHandler(new ClientSessionInitializer());

                IChannel bootstrapChannel = await bootstrap.BindAsync(port).ConfigureAwait(false);
                Console.ReadLine();

                await bootstrapChannel.CloseAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error("RunServerAsync", ex);
            }
            finally
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
            }
        }
    }
}