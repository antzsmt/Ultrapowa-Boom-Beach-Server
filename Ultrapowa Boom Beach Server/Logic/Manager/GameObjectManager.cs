using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UCS.Core;
using UCS.Files;

namespace UCS.Logic.Manager
{
    internal class GameObjectManager
    {
        private readonly ComponentManager m_vComponentManager;
        private readonly List<GameObject> m_vGameObjectRemoveList;
        private readonly List<List<GameObject>> m_vGameObjects;
        private readonly List<int> m_vGameObjectsIndex;
        private readonly Level m_vLevel;

        public GameObjectManager(Level l)
        {
            m_vLevel = l;
            m_vGameObjects = new List<List<GameObject>>();
            m_vGameObjectRemoveList = new List<GameObject>();
            m_vGameObjectsIndex = new List<int>();
            for (var i = 0; i < 7; i++)
            {
                m_vGameObjects.Add(new List<GameObject>());
                m_vGameObjectsIndex.Add(0);
            }
            m_vComponentManager = new ComponentManager(m_vLevel);
            //m_vObstacleManager      = new ObstacleManager(m_vLevel);
        }
        //readonly ObstacleManager m_vObstacleManager;

        public void AddGameObject(GameObject go)
        {
            go.GlobalId = GenerateGameObjectGlobalId(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData();
            }
            m_vGameObjects[go.ClassId].Add(go);
        }

        public List<List<GameObject>> GetAllGameObjects()
        {
            return m_vGameObjects;
        }

        public ComponentManager GetComponentManager()
        {
            return m_vComponentManager;
        }

        //public ObstacleManager GetObstacleManager() => m_vObstacleManager;

        public GameObject GetGameObjectByID(int id)
        {
            var classId = GlobalID.GetType(id) - 500;
            if (m_vGameObjects.Capacity < classId)
                return null;
            return m_vGameObjects[classId].Find(g => g.GlobalId == id);
        }

        public List<GameObject> GetGameObjects(int id)
        {
            return m_vGameObjects[id];
        }

        public void Load(JObject jsonObject)
        {
            var jsonBuildings = (JArray) jsonObject["buildings"];
            foreach (JObject jsonBuilding in jsonBuildings)
            {
                 Buildings bd = CSVManager.Tables.Get(1).GetDataWithID(jsonBuilding["data"].ToObject<int>()) as Buildings;
                 System.Console.WriteLine(bd.GetGlobalID());
                 Building b = new Building(bd, m_vLevel);
                 AddGameObject(b);
                 b.Load(jsonBuilding);
            }

            var jsonTraps = (JArray) jsonObject["traps"];
            /*foreach (JObject jsonTrap in jsonTraps)
            {
                var td = (TrapData)CSVManager.DataTables.GetDataById(jsonTrap["data"].ToObject<int>());
                var t = new Trap(td, m_vLevel);
                AddGameObject(t);
                t.Load(jsonTrap);
            }*/

            var jsonDecos = (JArray) jsonObject["decos"];

            /*foreach (JObject jsonDeco in jsonDecos)
            {
                var dd = (DecoData)CSVManager.DataTables.GetDataById(jsonDeco["data"].ToObject<int>());
                var d = new Deco(dd, m_vLevel);
                AddGameObject(d);
                d.Load(jsonDeco);
            }*/

            var jsonObstacles = (JArray) jsonObject["obstacles"];
            /*foreach (JObject jsonObstacle in jsonObstacles)
            {
                var dd = (ObstacleData)CSVManager.DataTables.GetDataById(jsonObstacle["data"].ToObject<int>());
                var d = new Obstacle(dd, m_vLevel);
                AddGameObject(d);
                d.Load(jsonObstacle);
            }

            m_vObstacleManager.Load(jsonObject); */
        }

        public void RemoveGameObject(GameObject go)
        {
            m_vGameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData();
            }
            RemoveGameObjectReferences(go);
        }

        public void RemoveGameObjectReferences(GameObject go)
        {
            m_vComponentManager.RemoveGameObjectReferences(go);
        }

        public JObject Save()
        {
            var pl = m_vLevel.Avatar;
            var jsonData = new JObject();

            var JBuildings = new JArray();
            var c = 0;
            foreach (var go in new List<GameObject>(m_vGameObjects[0]))
            {
                var b = (Building) go;
                var j = new JObject();
                j.Add("data", b.GetBuildingData().GetGlobalID());
                b.Save(j);
                JBuildings.Add(j);
                c++;
            }
            jsonData.Add("buildings", JBuildings);

            var JTraps = new JArray();
            /*int u = 0;
            foreach (GameObject go in new List<GameObject>(m_vGameObjects[4]))
            {
                Trap t = (Trap)go;
                JObject j = new JObject();
                j.Add("data", t.GetTrapData().GetGlobalID());
                j.Add("id", 504000000 + u);
                t.Save(j);
                JTraps.Add(j);
                u++;
            }*/
            jsonData.Add("traps", JTraps);

            var JDecos = new JArray();
            /*int e = 0;
            foreach (GameObject go in new List<GameObject>(m_vGameObjects[6]))
            {
                Deco d = (Deco)go;
                JObject j = new JObject();
                j.Add("data", d.GetDecoData().GetGlobalID());
                j.Add("id", 506000000 + e);
                d.Save(j);
                JDecos.Add(j);
                e++;
            }*/
            jsonData.Add("decos", JDecos);

            /*JArray JObstacles = new JArray();
            int o = 0;
            foreach (GameObject go in new List<GameObject>(m_vGameObjects[3]))
            {
                Obstacle d = (Obstacle)go;
                JObject j = new JObject();
                j.Add("data", d.GetObstacleData().GetGlobalID());
                j.Add("id", 503000000 + o);
                d.Save(j);
                JObstacles.Add(j);
                o++;
            }
            jsonData.Add("obstacles", JObstacles);
            m_vObstacleManager.Save(jsonData);*/

            var JObstacles = new JArray();
            jsonData.Add("obstacles", JObstacles);

            var JResourceShips = new JArray();
            jsonData.Add("resource_ships", JResourceShips);

            jsonData.Add("secondsFromLastTreeRespawn", 12711);
            jsonData.Add("map_spawn_timer", 98077);
            jsonData.Add("deepsea_spawn_timer", 126013);
            jsonData.Add("map_unliberation_timer", 5667);
            jsonData.Add("upgrade_outpost_defenses", false);
            jsonData.Add("seed", 1298056622);

            return jsonData;
        }

        public void Tick()
        {
            m_vComponentManager.Tick();
            foreach (var l in m_vGameObjects)
            foreach (var go in l)
                go.Tick();
            foreach (var g in new List<GameObject>(m_vGameObjectRemoveList))
            {
                RemoveGameObjectTotally(g);
                m_vGameObjectRemoveList.Remove(g);
            }
        }

        private int GenerateGameObjectGlobalId(GameObject go)
        {
            var index = m_vGameObjectsIndex[go.ClassId];
            m_vGameObjectsIndex[go.ClassId]++;
            return GlobalID.CreateGlobalID(go.ClassId + 500, index);
        }

        private void RemoveGameObjectTotally(GameObject go)
        {
            m_vGameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData();
            }
            RemoveGameObjectReferences(go);
        }
    }
}