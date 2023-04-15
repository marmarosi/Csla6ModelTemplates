using System.Reflection;

namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Represents a list of data access implementations.
    /// </summary>
    public class DalIndex
    {
        /// <summary>
        /// Gets the list of data access implementations in the specified assembly.
        /// </summary>
        public Dictionary<Type, Type> DalTypes { get; private set; }

        /// <summary>
        /// Creates a new instance of the list of data access implementations.
        /// </summary>
        /// <param name="dalAssembly">The assembly containing the data access implementations.</param>
        public DalIndex(
            Assembly dalAssembly
            )
        {
            DalTypes = new Dictionary<Type, Type>();
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
                    if (interfaces.Length >= 2)
                    {
                        DalTypes.Add(interfaces[1], type);
                    }
                }
            }
        }
    }
}
