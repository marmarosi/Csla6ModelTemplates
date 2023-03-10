using Csla;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csla6ModelTemplates.CslaExtensions.Models
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
        where Dto: class
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

        #region Update (Key)

        ///// <summary>
        ///// Updates an editable collection from the data transfer objects.
        ///// </summary>
        ///// <typeparam name="D">The type of the data transfer objects.</typeparam>
        ///// <param name="list">The list of data transfer objects.</param>
        ///// <param name="keyName">The name of the key property.</param>
        //public async Task Update(
        //    List<Dto> list,
        //    string keyName
        //    )
        //{
        //    List<int> indeces = Enumerable.Range(0, list.Count).ToList();
        //    for (int i = Items.Count - 1; i > -1; i--)
        //    {
        //        C item = Items[i];
        //        long? keyValue = GetValue(item, keyName);
        //        Predicate<Dto> match = (Dto o) => GetValue(o, keyName) == keyValue;
        //        Dto dto = list.Find(match);

        //        if (dto == null)
        //            RemoveItem(i);
        //        else
        //        {
        //            await item.Update(dto);
        //            indeces.Remove(list.IndexOf(dto));
        //        }
        //    }
        //    foreach (int index in indeces)
        //        Items.Add(await (
        //            typeof(EditableModel<C, Dto>)
        //            .GetMethod("Create")
        //            .MakeGenericMethod(typeof(Dto))
        //            .Invoke(null, new object[] { this, list[index] })
        //            as Task<C>));
        //}

        //private long? GetValue(
        //    object something,
        //    string propertyName
        //    )
        //{
        //    return something.GetType()
        //        .GetProperty(propertyName)
        //        .GetValue(something) as long?;
        //}

        #endregion

        #region Update (ID)

        /// <summary>
        /// Updates an editable collection from the data transfer objects.
        /// </summary>
        /// <param name="list">The list of data transfer objects.</param>
        /// <param name="idName">The name of the identifier property.</param>
        public async Task Update(
            List<Dto> list,
            string idName,
            IDataPortal<C> portal
            )
        {
            List<int> indeces = Enumerable.Range(0, list.Count).ToList();
            for (int i = Items.Count - 1; i > -1; i--)
            {
                C item = Items[i];
                string idValue = GetValue(item, idName);
                bool match(Dto o) => GetValue(o, idName) == idValue;
                Dto dto = list.Find(match);

                if (dto == null)
                    RemoveItem(i);
                else
                {
                    await item.Update(dto);
                    indeces.Remove(list.IndexOf(dto));
                }
            }
            foreach (int index in indeces)
            {
                var t = typeof(EditableModel<C, Dto>);
                var m = t.GetMethod("Create");
                Items.Add(await (
                    typeof(EditableModel<C, Dto>)
                    .GetMethod("Create")
                    .Invoke(null, new object[] { this, list[index] })
                    as Task<C>));
            }
            //foreach (int index in indeces)
            //    Items.Add(
            //        typeof(EditableModel<C, Dto>)
            //        .GetMethod("Create")
            //        .Invoke(null, new object[] { this, list[index] })
            //        as C
            //        );
        }

        private string GetValue(
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
