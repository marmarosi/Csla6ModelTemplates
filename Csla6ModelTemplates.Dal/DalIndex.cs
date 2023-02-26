using System.Reflection;

namespace Csla6ModelTemplates.Dal
{
    public class DalIndex
    {
        protected Assembly DalAssembly;
        private Dictionary<Type, Type> DalTypes;

        protected DalIndex() { }

        public Dictionary<Type, Type> GetDalItems()
        {
            if (DalTypes == null)
            {
                DalTypes = new Dictionary<Type, Type>();
                LookUpDalTypes();
            }
            return DalTypes;
        }

        private void LookUpDalTypes()
        {
            ArgumentNullException.ThrowIfNull(DalAssembly, "DalAssembly");

            foreach (Type type in DalAssembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(DalImplementationAttribute), false).Length > 0)
                {
                    Type[] interfaces = type.GetInterfaces();
                    if (interfaces.Length == 1)
                    {
                        DalTypes.Add(interfaces[0], type);
                    }
                }
            }
        }
    }
}
