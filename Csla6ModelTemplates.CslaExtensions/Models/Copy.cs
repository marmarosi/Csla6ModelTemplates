using System.Reflection;

namespace Csla6ModelTemplates.CslaExtensions.Models
{
    public class Copy
    {
        private readonly object source;
        private readonly PropertyInfo[] sourceProperties;
        private readonly List<string> whiteList;
        private readonly List<string> blackList;

        public Copy(
            object source
            )
        {
            this.source = source;
            sourceProperties = source.GetType().GetProperties();
            whiteList = new List<string>();
            blackList = new List<string>();
        }

        public static Copy PropertiesFrom(
            object source
            )
        {
            return new Copy(source);
        }

        public Copy Keep(
            params string[] propertyNames
            )
        {
            whiteList.AddRange(propertyNames);
            return this;
        }

        public Copy Omit(
            params string[] propertyNames
            )
        {
            blackList.AddRange(propertyNames);
            return this;
        }

        public T ToNew<T>() where T: class
        {
            var target = Activator.CreateInstance<T>();
            return (T)ToPropertiesOf(target);
        }

        public object ToPropertiesOf(
            object target
            )
        {
            var targetProperties = target.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                if (blackList.Count > 0 && blackList.Contains(sourceProperty.Name))
                    continue;

                if (whiteList.Count > 0 && !whiteList.Contains(sourceProperty.Name))
                    continue;

                foreach (var targetProperty in targetProperties)
                {
                    if (sourceProperty.Name == targetProperty.Name /* &&
                        sourceProperty.PropertyType == targetProperty.PropertyType */
                        )
                    {
                        targetProperty.SetValue(target, sourceProperty.GetValue(source));
                        break;
                    }
                }
            }
            return target;
        }
    }
}
