namespace UCS.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using Core.Settings;
    using Database;
    using Helpers;
    using Logic;
    using Logic.Enums;
    using static Core.Logger;

    internal class DatabaseManager
    {
        internal string Mysql;

        public void CreateAccount(Level l)
        {
            try
            {
                if (Constants.UseCacheServer)
                    Redis.Players.StringSet(l.Avatar.UserID.ToString(),
                        l.Avatar.SaveToJSON + "#:#:#:#" + l.SaveToJSON,
                        TimeSpan.FromHours(4));

                using (var db = new Mysql())
                {
                    db.Player.Add(
                        new Player
                        {
                            PlayerId = l.Avatar.UserID,
                            Avatar = l.Avatar.SaveToJSON,
                            GameObjects = l.SaveToJSON
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
        }

        /*public void CreateAlliance(Alliance a)
        {
            try
            {
                if (Constants.UseCacheServer) //Redis As Cache Server
                    Redis.Clans.StringSet(a.m_vAllianceId.ToString(), a.SaveToJSON(), TimeSpan.FromHours(4));

                using (Mysql db = new Mysql())
                {
                    db.Clan.Add(
                        new Clan()
                        {
                            ClanId = a.m_vAllianceId,
                            LastUpdateTime = DateTime.Now,
                            Data = a.SaveToJSON()
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
        }*/

        public Level GetAccount(long playerId)
        {
            try
            {
                Level account = null;
                if (Constants.UseCacheServer)
                {
                    var _Data = Redis.Players.StringGet(playerId.ToString()).ToString();

                    if (!string.IsNullOrEmpty(_Data) && _Data.Contains("#:#:#:#"))
                    {
                        var _Datas = _Data.Split(new string[1] {"#:#:#:#"}, StringSplitOptions.None);

                        if (!string.IsNullOrEmpty(_Datas[0]) && !string.IsNullOrEmpty(_Datas[1]))
                        {
                            account = new Level();
                            account.Avatar.LoadFromJSON(_Datas[0]);
                            account.LoadFromJSON(_Datas[1]);
                        }
                    }
                    else
                    {
                        using (var db = new Mysql())
                        {
                            var p = db.Player.Find(playerId);

                            if (p != null)
                            {
                                account = new Level();
                                account.Avatar.LoadFromJSON(p.Avatar);
                                account.LoadFromJSON(p.GameObjects);
                                Redis.Players.StringSet(playerId.ToString(), p.Avatar + "#:#:#:#" + p.GameObjects,
                                    TimeSpan.FromHours(4));
                            }
                        }
                        ;
                    }
                }
                else
                {
                    using (var db = new Mysql())
                    {
                        var p = db.Player.Find(playerId);

                        if (p != null)
                        {
                            account = new Level();
                            account.Avatar.LoadFromJSON(p.Avatar);
                            account.LoadFromJSON(p.GameObjects);
                        }
                    }
                }
                return account;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /*public Alliance GetAlliance(long allianceId)
        {
            try
            {
                Alliance alliance = null;
                if (Constants.UseCacheServer)
                {
                    string _Data = Redis.Clans.StringGet(allianceId.ToString()).ToString();


                    if (!string.IsNullOrEmpty(_Data))
                    {
                        alliance = new Alliance();
                        alliance.LoadFromJSON(_Data);
                    }
                    else
                    {
                        using (Mysql db = new Mysql())
                        {
                            Clan p = db.Clan.Find(allianceId);
                            if (p != null)
                            {
                                alliance = new Alliance();
                                alliance.LoadFromJSON(p.Data);
                                Redis.Clans.StringSet(allianceId.ToString(), p.Data, TimeSpan.FromHours(4));
                            }
                        }
                    }
                }
                else
                {
                    using (Mysql db = new Mysql())
                    {
                        Clan p = db.Clan.Find(allianceId);
                        if (p != null)
                        {
                            alliance = new Alliance();
                            alliance.LoadFromJSON(p.Data);
                        }
                    }
                }
                return alliance;
            }
            catch (Exception)
            {
                return null;
            }
        }
        */
        public List<long> GetAllPlayerIds()
        {
            var ids = new List<long>();
            using (var db = new Mysql())
            {
                ids.AddRange(db.Player.Select(p => p.PlayerId));
            }
            return ids;
        }

        public List<long> GetAllClanIds()
        {
            var ids = new List<long>();
            using (var db = new Mysql())
            {
                ids.AddRange(db.Clan.Select(p => p.ClanId));
            }
            return ids;
        }

        public long GetMaxAllianceId()
        {
            const string SQL = "SELECT coalesce(MAX(ClanId), 0) FROM Clan";
            var Seed = -1;

            using (var Conn = new MySqlConnection(Mysql))
            {
                Conn.Open();

                using (var CMD = new MySqlCommand(SQL, Conn))
                {
                    CMD.Prepare();
                    Seed = Convert.ToInt32(CMD.ExecuteScalar());
                }
            }

            return Seed;
        }

        public long GetMaxPlayerId()
        {
            try
            {
                const string SQL = "SELECT coalesce(MAX(PlayerId), 0) FROM Player";
                var Seed = -1;

                var builder = new MySqlConnectionStringBuilder
                {
                    Server = Utils.ParseConfigString("MysqlIPAddress"),
                    UserID = Utils.ParseConfigString("MysqlUsername"),
                    Port = (uint) Utils.ParseConfigInt("MysqlPort"),
                    Pooling = false,
                    Database = Utils.ParseConfigString("MysqlDatabase"),
                    MinimumPoolSize = 1
                };

                if (!string.IsNullOrWhiteSpace(Utils.ParseConfigString("MysqlPassword")))
                    builder.Password = Utils.ParseConfigString("MysqlPassword");

                Mysql = builder.ToString();

                using (var Conn = new MySqlConnection(Mysql))
                {
                    Conn.Open();

                    using (var CMD = new MySqlCommand(SQL, Conn))
                    {
                        CMD.Prepare();
                        Seed = Convert.ToInt32(CMD.ExecuteScalar());
                    }
                }

                return Seed;
            }
            catch (Exception ex)
            {
                Say();
                Error("An exception occured when reconnecting to the MySQL Server.");
                Error("Please check your database configuration!");
                Error(ex.Message);
                Console.ReadKey();
                UCSControl.UCSRestart();
            }
            return 0;
        }

        /*public void RemoveAlliance(Alliance alliance)
        {
            try
            {
                long id = alliance.m_vAllianceId;
                using (Mysql db = new Mysql())
                {
                    db.Clan.Remove(db.Clan.Find((int)id));
                    db.SaveChanges();
                }
                ObjectManager.RemoveInMemoryAlliance(id);
            }
            catch (Exception)
            {
            }
        }*/

        /*public async Task Save(Alliance alliance)
        {
            try
            {
                if (Constants.UseCacheServer)
                    Redis.Clans.StringSet(alliance.m_vAllianceId.ToString(), alliance.SaveToJSON(), TimeSpan.FromHours(4));

                using (Mysql context = new Mysql())
                {
                    Clan c = await context.Clan.FindAsync((int)alliance.m_vAllianceId);
                    if (c != null)
                    {
                        c.LastUpdateTime = DateTime.Now;
                        c.Data = alliance.SaveToJSON();
                        context.Entry(c).State = EntityState.Modified;
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
            }
        }*/

        public async Task Save(Level avatar)
        {
            try
            {
                if (Constants.UseCacheServer)
                    Redis.Players.StringSet(avatar.Avatar.UserID.ToString(),
                        avatar.Avatar.SaveToJSON + "#:#:#:#" + avatar.SaveToJSON, TimeSpan.FromHours(4));

                using (var context = new Mysql())
                {
                    var p = await context.Player.FindAsync(avatar.Avatar.UserID);
                    if (p != null)
                    {
                        p.Avatar = avatar.Avatar.SaveToJSON;
                        p.GameObjects = avatar.SaveToJSON;
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
            }
        }

        public async Task Save(List<Level> avatars, Save Save = Logic.Enums.Save.Mysql)
        {
            try
            {
                switch (Save)
                {
                    case Save.Redis:
                    {
                        foreach (var pl in avatars)
                            Redis.Players.StringSet(pl.Avatar.UserID.ToString(),
                                pl.Avatar.SaveToJSON + "#:#:#:#" + pl.SaveToJSON, TimeSpan.FromHours(4));
                        break;
                    }

                    case Save.Mysql:
                    {
                        using (var context = new Mysql())
                        {
                            foreach (var pl in avatars)
                            {
                                var p = context.Player.Find(pl.Avatar.UserID);
                                if (p != null)
                                {
                                    p.Avatar = pl.Avatar.SaveToJSON;
                                    p.GameObjects = pl.SaveToJSON;
                                }
                            }
                            await context.SaveChangesAsync();
                        }
                        break;
                    }
                    case Save.Both:
                    {
                        this.Save(avatars, Save.Mysql);
                        this.Save(avatars, Save.Redis);
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /*public async Task Save(List<Alliance> alliances, Save Save = Logic.Enums.Save.Mysql)
        {
            try
            {
                switch (Save)
                {

                    case Save.Redis:
                        {
                            foreach (Alliance alliance in alliances)
                            {
                                Redis.Clans.StringSet(alliance.m_vAllianceId.ToString(), alliance.SaveToJSON(),
                                    TimeSpan.FromHours(4));
                            }
                            break;
                        }
                    case Save.Mysql:
                        {
                            using (Mysql context = new Mysql())
                            {
                                foreach (Alliance alliance in alliances)
                                {
                                    Clan c = context.Clan.Find((int)alliance.m_vAllianceId);
                                    if (c != null)
                                    {
                                        c.LastUpdateTime = DateTime.Now;
                                        c.Data = alliance.SaveToJSON();
                                    }

                                }
                                await context.SaveChangesAsync();
                            }
                            break;
                        }
                    case Save.Both:
                        {
                            this.Save(alliances, Save.Mysql);
                            this.Save(alliances, Save.Redis);
                            break;
                        }
                }
            }
            catch (Exception)
            {
            }
        }*/
    }
}