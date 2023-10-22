using Csla;

namespace Csla6ModelTemplates.CslaExtensions
{
    /// <summary>
    /// Wrapper for read-only collections to hide server side properties.
    /// </summary>
    /// <typeparam name="T">The type of the read-only collection.</typeparam>
    /// <typeparam name="C">The type of the read-only objects in the collection.</typeparam>
    [Serializable]
    public abstract class ReadOnlyList<T, C> : ReadOnlyListBase<T, C>, IReadOnlyList
        where T : ReadOnlyListBase<T, C>
        where C : ReadOnlyBase<C>
    {
        /// <summary>
        /// Converts the read-only collection to data transfer object list.
        /// </summary>
        /// <typeparam name="D">The class of the data transfer objects.</typeparam>
        /// <returns>The list of the data transfer objects.</returns>
        public IList<D> ToDto<D>() where D : class
        {
            Type type = typeof(List<D>);
            IList<D> instance = Activator.CreateInstance(type) as IList<D>;

            foreach (C item in Items)
            {
                D child = item.GetType()
                    .GetMethod("ToDto")
                    .MakeGenericMethod(typeof(D))
                    .Invoke(item, null) as D;
                instance.Add(child);
            }
            return instance;
        }
    }
}
