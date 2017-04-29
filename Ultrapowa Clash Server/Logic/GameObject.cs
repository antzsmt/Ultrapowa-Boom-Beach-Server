using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UCS.Logic.Enums;
using System;
using UCS.Files;

namespace UCS.Logic
{
    internal class GameObject
    {
        public GameObject(Data data, Level level)
        {
            m_vLevel      = level;
            Data          = data;
            m_vComponents = new List<Component>();
            for (int i = 0; i < 11; i++)
            {
                m_vComponents.Add(new Component());
            }
        }

        readonly List<Component> m_vComponents;
        internal readonly Data Data;
        readonly Level m_vLevel;

        public virtual int ClassId => -1;

        public int GlobalId { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public void AddComponent(Component c)
        {
            if (m_vComponents[c.Type].Type != -1)
            {
            }
            else
            {
                m_vLevel.GetComponentManager().AddComponent(c);
                m_vComponents[c.Type] = c;
            }
        }

        public Component GetComponent(int index, bool test)
        {
            Component result = null;
            if (!test || m_vComponents[index].IsEnabled)
            {
                result = m_vComponents[index];
            }
            return result;
        }

        public Level Avatar => m_vLevel;

        public Vector GetPosition() => new Vector(X, Y);

        public virtual bool IsHero() => false;

        public int TownHallLevel() => Avatar.Avatar.TownHallLevel;

        public int LayoutID() => Avatar.Avatar.m_vActiveLayout;

        public void Load(JObject jsonObject)
        {
            X = jsonObject["x"].ToObject<int>();
            Y = jsonObject["y"].ToObject<int>();

            foreach(Component c in m_vComponents)
            {
                c.Load(jsonObject);
            }
        }    

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("x", X);
            jsonObject.Add("y", Y);

            foreach(Component c in m_vComponents)
            {
                c.Save(jsonObject);
            }

            return jsonObject;
        }

        public void SetPositionXY(int newX, int newY, int Layout)
        {
            X = newX;
            Y = newY;
        }

        public virtual void Tick()
        {
            foreach(Component comp in m_vComponents)
            {
                if (comp.IsEnabled)
                    comp.Tick();
            }
        }
    }
}