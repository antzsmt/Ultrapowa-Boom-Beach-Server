namespace UCS.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using Helpers;
    using Logic;
    using Packets;

    internal class ResourcesManager : IDisposable
    {
        internal static ConcurrentDictionary<IntPtr, Device> m_vClients            = null;
        internal static ConcurrentDictionary<long, Level> m_vInMemoryLevels        = null;
        //internal static ConcurrentDictionary<long, Alliance> m_vInMemoryAlliances  = null;
        internal static List<Level> m_vOnlinePlayers                               = null;
        private static DatabaseManager m_vDatabase                                 = null;

        public ResourcesManager()
        {
            m_vDatabase          = new DatabaseManager();
            m_vOnlinePlayers     = new List<Level>();
            m_vClients           = new ConcurrentDictionary<IntPtr, Device>();
            m_vInMemoryLevels    = new ConcurrentDictionary<long, Level>();
            //m_vInMemoryAlliances = new ConcurrentDictionary<long, Alliance>();
        }

        public static void AddClient(Socket _Socket)
        {
            Device c = new Device(_Socket)
            {
                IPAddress = ((System.Net.IPEndPoint) _Socket.RemoteEndPoint).Address.ToString()
            };
            m_vClients.TryAdd(c.Socket.Handle, c);
        }

        public static void AddClient(Device Client)
        {
            m_vClients.TryAdd(Client.Socket.Handle, Client);
        }

        public static void DropClient(IntPtr socketHandle)
        {
            try
            {
                Device _Client = null;
                m_vClients.TryRemove(socketHandle, out _Client);
                if (_Client.Player != null)
                {
                    LogPlayerOut(_Client.Player);
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void DropClient(Device client)
        {
            try
            {
                m_vClients.TryRemove(client.SocketHandle);
                if (client.Player != null)
                {
                    LogPlayerOut(client.Player);
                }
            }
            catch (Exception e)
            {
            }
        }

        public static List<long> GetAllPlayerIds() => m_vDatabase.GetAllPlayerIds();

        public static Device GetClient(IntPtr socketHandle) => m_vClients.ContainsKey(socketHandle) ? m_vClients[socketHandle] : null;

        public static List<Device> GetConnectedClients() => m_vClients.Values.ToList();

        public static Level GetPlayer(long id, bool persistent = false)
        {
            Level result = GetInMemoryPlayer(id);
            if (result == null)
            {
                result = m_vDatabase.GetAccount(id);
                if (persistent)
                {
                    LoadLevel(result);
                }
            }
            return result;
        }

        public static void DisconnectClient(Device _Client)
        {
            //Processor.Send(new OutOfSyncMessage(_Client));
            DropClient(_Client.SocketHandle);
        }

        public static bool IsClientConnected(IntPtr socketHandle) => m_vClients[socketHandle] != null && m_vClients[socketHandle].IsClientSocketConnected();

        public static bool IsPlayerOnline(Level l) => m_vOnlinePlayers.Contains(l);

        public static void LoadLevel(Level level)
        {
            m_vInMemoryLevels.TryAdd(level.Avatar.UserID, level);
        }

        public static void LogPlayerIn(Level l, Device c)
        {
            l.Client = c;
            c.Player = l;

            if (!m_vOnlinePlayers.Contains(l))
            {
                m_vOnlinePlayers.Add(l);
                LoadLevel(l);
                Program.TitleU();
            }
            else
            {
                int i = m_vOnlinePlayers.IndexOf(l);
                m_vOnlinePlayers[i] = l;
            }
        }

        public static void LogPlayerOut(Level level)
        {
            Resources.DatabaseManager.Save(level);
            m_vOnlinePlayers.Remove(level);
            m_vInMemoryLevels.TryRemove(level.Avatar.UserID);
            m_vClients.TryRemove(level.Client.SocketHandle);
            Program.TitleD();
        }

        private static Level GetInMemoryPlayer(long id) => m_vInMemoryLevels.ContainsKey(id) ? m_vInMemoryLevels[id] : null;

        /*public static void AddAllianceInMemory(Alliance all)
        {
            m_vInMemoryAlliances.TryAdd(all.m_vAllianceId, all);
        }*/

        /*public static void AddAllianceInMemory(List<Alliance> all)
        {
            for (int i = 0, allCount = all.Count; i < allCount; i++)
            {
                Alliance a = all[i];
                m_vInMemoryAlliances.TryAdd(a.m_vAllianceId, a);
            }
        }*/

        //public static bool InMemoryAlliancesContain(long key) => m_vInMemoryAlliances.Keys.Contains(key);

        //public static bool InMemoryAlliancesContain(Alliance all) => m_vInMemoryAlliances.Values.Contains(all);

        /*public static Alliance GetInMemoryAlliance(long key)
        {
            Alliance a;
            m_vInMemoryAlliances.TryGetValue(key, out a);
            return a;
        }*/

        /*public static void RemoveAllianceFromMemory(long key)
        {
            m_vInMemoryAlliances.TryRemove(key);
        }*/

        public static void SetGameObject(Level level, string json)
        {
            level.Avatar.LoadFromJSON(json);

            LogPlayerOut(level);
        }

        public void Dispose()
        {
            //m_vInMemoryAlliances.Clear();
            m_vInMemoryLevels.Clear();
            m_vOnlinePlayers.Clear();
        }
    }
}