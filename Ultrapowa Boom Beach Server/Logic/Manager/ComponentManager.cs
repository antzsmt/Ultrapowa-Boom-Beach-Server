using System.Collections.Generic;
using System.Windows;
using UCS.Core;

namespace UCS.Logic.Manager
{
    internal class ComponentManager
    {
        public ComponentManager(Level l)
        {
            m_vComponents = new List<List<Component>>();
            for (var i = 0; i <= 10; i++)
                m_vComponents.Add(new List<Component>());
            m_vLevel = l;
        }

        readonly List<List<Component>> m_vComponents;

        readonly Level m_vLevel;

        public void AddComponent(Component c) => m_vComponents[c.Type].Add(c);

        public Component GetClosestComponent(int x, int y, ComponentFilter cf)
        {
            Component result = null;
            var componentType = cf.Type;
            var components = m_vComponents[componentType];
            var v = new Vector(x, y);
            double maxLengthSquared = 0;

            if (components.Count > 0)
                foreach (var c in components)
                    if (cf.TestComponent(c))
                    {
                        var go = c.GetParent();
                        var lengthSquared = (v - go.GetPosition()).LengthSquared;
                        if (lengthSquared < maxLengthSquared || result == null)
                        {
                            maxLengthSquared = lengthSquared;
                            result = c;
                        }
                    }
            return result;
        }

        public List<Component> GetComponents(int type) => m_vComponents[type];

        public void RemoveGameObjectReferences(GameObject go)
        {
            foreach (var components in m_vComponents)
            {
                var markedForRemoval = new List<Component>();
                foreach (var component in components)
                    if (component.GetParent() == go)
                        markedForRemoval.Add(component);
                foreach (var component in markedForRemoval)
                    components.Remove(component);
            }
        }

        public void Tick()
        {
        }
    }
}
