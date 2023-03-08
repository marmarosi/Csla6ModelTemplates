using System.Reflection;

namespace Csla6ModelTemplates.Dal
{
    public class DalIndex
    {
        public readonly Dictionary<Type, Type> DalTypes = new Dictionary<Type, Type>();

        public DalIndex(
            Assembly dalAssembly
            )
        {
            LookUpDalTypes(dalAssembly);
        }

        private void LookUpDalTypes(
            Assembly dalAssembly
            )
        {
            ArgumentNullException.ThrowIfNull(dalAssembly);

            foreach (Type type in dalAssembly.GetTypes())
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
