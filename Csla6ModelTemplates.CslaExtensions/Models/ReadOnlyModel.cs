using Csla;
using Csla.Core;
using Csla6ModelTemplates.Dal.Contracts;
using System.Reflection;

namespace Csla6ModelTemplates.CslaExtensions.Models
{
    /// <summary>
    /// Wrapper for read-only model to hide server side properties.
    /// </summary>
    /// <typeparam name="T">The type of the business object.</typeparam>
    [Serializable]
    public abstract class ReadOnlyModel<T> : ReadOnlyBase<T>, IReadOnlyModel
        where T: ReadOnlyBase<T>
    {
        /// <summary>
        /// Converts the business object to data transfer object.
        /// </summary>
        /// <typeparam name="D">The class of the data transfer object.</typeparam>
        /// <returns>The data transfer object.</returns>
        public D ToDto<D>() where D : class
        {
            Type type = typeof(D);
            D dto = Activator.CreateInstance(type) as D;

            List<IPropertyInfo> cslaProperties = FieldManager.GetRegisteredProperties();
            List<PropertyInfo> dtoProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(fi => !fi.Name.StartsWith("__"))
                .ToList();

            foreach (var dtoProperty in dtoProperties)
            {
                var cslaProperty = cslaProperties.Find(pi => pi.Name == dtoProperty.Name);
                if (cslaProperty != null)
                {
                    if (cslaProperty.Type.GetInterface(nameof(IReadOnlyList)) != null)
                    {
                        Type childType = dtoProperty.PropertyType.GenericTypeArguments[0];
                        IReadOnlyList cslaBase = GetProperty(cslaProperty) as IReadOnlyList;
                        object value = cslaProperty.Type
                            .GetMethod("ToDto")
                            .MakeGenericMethod(childType)
                            .Invoke(cslaBase, null);
                        dtoProperty.SetValue(dto, value);
                    }
                    else if (cslaProperty.Type.GetInterface(nameof(IReadOnlyModel)) != null)
                    {
                        Type childType = dtoProperty.PropertyType;
                        IReadOnlyModel cslaBase = GetProperty(cslaProperty) as IReadOnlyModel;
                        object value = cslaProperty.Type
                            .GetMethod("ToDto")
                            .MakeGenericMethod(childType)
                            .Invoke(cslaBase, null);
                        dtoProperty.SetValue(dto, value);
                    }
                    else
                        dtoProperty.SetValue(dto, GetProperty(cslaProperty));
                }
            }

            return dto;
        }

        /// <summary>
        /// Converts the business object to paginated data transfer object.
        /// </summary>
        /// <typeparam name="D">The class of the data transfer object.</typeparam>
        /// <returns>The data transfer object.</returns>
        public IPaginatedList<D> ToPaginatedDto<D>() where D : class
        {
            Type type = typeof(PaginatedList<D>);
            PaginatedList<D> dto = Activator.CreateInstance(type) as PaginatedList<D>;

            List<IPropertyInfo> cslaProperties = FieldManager.GetRegisteredProperties();
            List<PropertyInfo> dtoProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(fi => !fi.Name.StartsWith("__"))
                .ToList();

            foreach (var dtoProperty in dtoProperties)
            {
                var cslaProperty = cslaProperties.Find(pi => pi.Name == dtoProperty.Name);
                if (cslaProperty != null)
                {
                    if (cslaProperty.Type.GetInterface(nameof(IReadOnlyList)) != null)
                    {
                        Type childType = dtoProperty.PropertyType.GenericTypeArguments[0];
                        IReadOnlyList cslaBase = GetProperty(cslaProperty) as IReadOnlyList;
                        object value = cslaProperty.Type
                            .GetMethod("ToDto")
                            .MakeGenericMethod(childType)
                            .Invoke(cslaBase, null);
                        dtoProperty.SetValue(dto, value);
                    }
                    else if (cslaProperty.Type.GetInterface(nameof(IReadOnlyModel)) != null)
                    {
                        Type childType = dtoProperty.PropertyType;
                        IReadOnlyModel cslaBase = GetProperty(cslaProperty) as IReadOnlyModel;
                        object value = cslaProperty.Type
                            .GetMethod("ToDto")
                            .MakeGenericMethod(childType)
                            .Invoke(cslaBase, null);
                        dtoProperty.SetValue(dto, value);
                    }
                    else
                        dtoProperty.SetValue(dto, GetProperty(cslaProperty));
                }
            }

            return dto;
        }
    }
}
