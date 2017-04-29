using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UCS.Files;
using UCS.Logic;
using Timer = System.Threading.Timer;
using static UCS.Core.Logger;
using UCS.Logic.Enums;

namespace UCS.Core
{
    internal class ObjectManager : IDisposable
    {
        private static long m_vAllianceSeed;
        private static long m_vAvatarSeed;
        public static int m_vDonationSeed;
        private static DatabaseManager m_vDatabase;
        internal static string HomeDefault;
        public static bool m_vTimerCanceled;
        internal static Timer TimerReferenceRedis;
        internal static Timer TimerReferenceMysql;
        internal static Dictionary<int, string> NpcLevels;
        internal static FingerPrint FingerPrint;
        internal static int MaxPlayerID;
        internal static int MaxAllianceID;

        public ObjectManager()
        {
            m_vTimerCanceled       = false;

            m_vDatabase            = new DatabaseManager();

            NpcLevels              = new Dictionary<int, string>();
            FingerPrint            = new FingerPrint();

            MaxPlayerID            = (int)m_vDatabase.GetMaxPlayerId() + 1;
            MaxAllianceID          = (int)m_vDatabase.GetMaxAllianceId() + 1;

            m_vAvatarSeed          = MaxPlayerID;
            m_vAllianceSeed        = MaxAllianceID;

            HomeDefault = File.ReadAllText("Gamefiles/starting_home.json");

            //LoadNpcLevels();

            TimerReferenceRedis = new Timer(SaveRedis, null, 10000, 40000);
            TimerReferenceMysql = new Timer(SaveMysql, null, 40000, 27000);
            Say($"UBS Database has been succesfully loaded. ({Convert.ToInt32(MaxAllianceID + MaxPlayerID)} Rows)");
        }

        private static void SaveRedis(object state)
        {
            m_vDatabase.Save(ResourcesManager.m_vInMemoryLevels.Values.ToList(), Save.Redis);
            //m_vDatabase.Save(ResourcesManager.m_vInMemoryAlliances.Values.ToList(), Save.Redis);
        }

        private static async void SaveMysql(object state)
        {
            m_vDatabase.Save(ResourcesManager.m_vInMemoryLevels.Values.ToList()).Wait();
            //m_vDatabase.Save(ResourcesManager.m_vInMemoryAlliances.Values.ToList(), Save.Mysql).Wait();
        }

        /*public static Alliance CreateAlliance(long seed)
        {
            if (seed == 0)
                seed = m_vAllianceSeed;
            Alliance Alliance = new Alliance(seed);
            m_vAllianceSeed++;
            m_vDatabase.CreateAlliance(Alliance);
            ResourcesManager.AddAllianceInMemory(Alliance);
            return Alliance;
        }*/

        public static Level CreateAvatar
        {
            get
            {
                Level Player = new Level(m_vAvatarSeed, null);
                m_vAvatarSeed++;
                //Player.LoadFromJSON(HomeDefault);
                m_vDatabase.CreateAccount(Player);
                return Player;
            }
        }

        /*public static void LoadAllAlliancesFromDB()
        {
            ResourcesManager.AddAllianceInMemory(m_vDatabase.GetAllAlliances());
        }*/

        /*public static async Task<Alliance> GetAlliance(long allianceId)
        {
            try
            {
                Alliance alliance;
                if (ResourcesManager.InMemoryAlliancesContain(allianceId))
                {
                    return ResourcesManager.GetInMemoryAlliance(allianceId);
                }
                else
                {
                    alliance = m_vDatabase.GetAlliance(allianceId);
                    if (alliance != null)
                        ResourcesManager.AddAllianceInMemory(alliance);
                    else
                        return null;
                    return alliance;
                }
            }
            catch (Exception) { return null; }
        }*/

        //public static List<Alliance> GetInMemoryAlliances() => ResourcesManager.m_vInMemoryAlliances.Values.ToList();

        public static Level GetRandomOnlinePlayer()
        {
            int index = new Random().Next(0, ResourcesManager.m_vInMemoryLevels.Count);
            return ResourcesManager.m_vInMemoryLevels.Values.ToList().ElementAt(index);
        }

        public static void LoadNpcLevels()
        {
            int Count = 0;
            NpcLevels.Add(17000000, new StreamReader(@"Gamefiles/level/NPC/tutorial_npc.json").ReadToEnd());
            NpcLevels.Add(17000001, new StreamReader(@"Gamefiles/level/NPC/tutorial_npc2.json").ReadToEnd());

            for (int i = 2; i < 50; i++)
            {
                using (StreamReader sr = new StreamReader(@"Gamefiles/level/NPC/level" + (i + 1) + ".json"))
                {
                    NpcLevels.Add(i + 17000000, sr.ReadToEnd());
                }
                Count++;
            }

            Say($"NPC Levels  have been succesfully loaded. ({Count})");
        }

        public static void RemoveInMemoryAlliance(long id)
        {
            //ResourcesManager.RemoveAllianceFromMemory(id);
        }

        public void Dispose()
        {
            if (TimerReferenceRedis != null && TimerReferenceMysql != null)
            {
                TimerReferenceRedis.Dispose();
                TimerReferenceMysql.Dispose();
                TimerReferenceRedis = null;
                TimerReferenceMysql = null;
            }
        }
    }
}
