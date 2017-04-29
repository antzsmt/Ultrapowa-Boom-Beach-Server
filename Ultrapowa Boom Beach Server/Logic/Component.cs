using Newtonsoft.Json.Linq;

namespace UCS.Logic
{
    internal class Component
    {
        private readonly GameObject m_vParentGameObject;
        internal bool IsEnabled;

        public Component()
        {
        }

        public Component(GameObject go)
        {
            IsEnabled = true;
            m_vParentGameObject = go;
        }

        public virtual int Type => -1;

        public GameObject GetParent()
        {
            return m_vParentGameObject;
        }

        public virtual void Load(JObject jsonObject)
        {
        }

        public virtual JObject Save(JObject jsonObject)
        {
            return jsonObject;
        }

        public void SetEnabled(bool status)
        {
            IsEnabled = status;
        }

        public virtual void Tick()
        {
        }
    }
}