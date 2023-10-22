using Csla;

namespace Csla6ModelTemplates.CslaExtensions
{
    /// <summary>
    /// Wrapper for editable collections to hide server side properties.
    /// </summary>
    /// <typeparam name="T">The type of the business collection.</typeparam>
    /// <typeparam name="C">The type of the business objects in the collection.</typeparam>
    [Serializable]
    public abstract class EditableList<T, C, Dto> : BusinessListBase<T, C>, IEditableList<Dto, C>
        where T : BusinessListBase<T, C>, IEditableList<Dto, C>
        where C : EditableModel<C, Dto>
        where Dto : class
    {
        #region ToDto

        /// <summary>
        /// Converts the business collection to data transfer object list.
        /// </summary>
        /// <returns>The list of the data transfer objects.</returns>
        public IList<Dto> ToDto()
        {
            Type type = typeof(List<Dto>);
            IList<Dto> instance = Activator.CreateInstance(type) as IList<Dto>;

            foreach (C item in Items)
            {
                Dto child = item.GetType()
                    .GetMethod("ToDto")
                    .Invoke(item, null) as Dto;
                instance.Add(child);
            }
            return instance;
        }

        #endregion

        #region SetValuesByKey

        /// <summary>
        /// Updates an editable collection from the data transfer objects.
        /// </summary>
        /// <param name="list">The list of data transfer objects.</param>
        /// <param name="keyName">The name of the key property.</param>
        /// <param name="childFactory">The data portal factory of the items.</param>
        public void SetValuesByKey(
            List<Dto> list,
            string keyName,
            IChildDataPortalFactory childFactory
            )
        {
            List<int> indeces = Enumerable.Range(0, list.Count).ToList();
            for (int i = Items.Count - 1; i > -1; i--)
            {
                C item = Items[i];
                long? keyValue = GetKeyValue(item, keyName);
                Predicate<Dto> match = (o) => GetKeyValue(o, keyName) == keyValue;
                Dto dto = list.Find(match);

                if (dto == null)
                    RemoveItem(i);
                else
                {
                    item.SetValuesOnBuild(dto, childFactory);
                    indeces.Remove(list.IndexOf(dto));
                }
            }
            if (indeces.Count > 0)
            {
                var portal = childFactory.GetPortal<C>();
                foreach (int index in indeces)
                {
                    C item = portal.CreateChild();
                    item.SetValuesOnBuild(list[index], childFactory);
                    Items.Add(item);
                }
            }
        }

        private long? GetKeyValue(
            object something,
            string propertyName
            )
        {
            return something.GetType()
                .GetProperty(propertyName)
                .GetValue(something) as long?;
        }

        #endregion

        #region SetValuesById

        /// <summary>
        /// Updates an editable collection from the data transfer objects.
        /// </summary>
        /// <param name="list">The list of data transfer objects.</param>
        /// <param name="idName">The name of the identifier property.</param>
        /// <param name="childFactory">The data portal factory of the items.</param>
        public void SetValuesById(
            List<Dto> list,
            string idName,
            IChildDataPortalFactory childFactory
            )
        {
            List<int> indeces = Enumerable.Range(0, list.Count).ToList();
            for (int i = Items.Count - 1; i > -1; i--)
            {
                C item = Items[i];
                string idValue = GeIdtValue(item, idName);
                bool match(Dto o) => GeIdtValue(o, idName) == idValue;
                Dto dto = list.Find(match);

                if (dto == null)
                    RemoveItem(i);
                else
                {
                    item.SetValuesOnBuild(dto, childFactory);
                    indeces.Remove(list.IndexOf(dto));
                }
            }
            if (indeces.Count > 0)
            {
                var portal = childFactory.GetPortal<C>();
                foreach (int index in indeces)
                {
                    C item = portal.CreateChild();
                    item.SetValuesOnBuild(list[index], childFactory);
                    Items.Add(item);
                }
            }
        }

        private string GeIdtValue(
            object something,
            string propertyName
            )
        {
            return something.GetType()
                .GetProperty(propertyName)
                .GetValue(something) as string;
        }

        #endregion
    }
}
