using Csla;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.CslaExtensions.Models
{
    /// <summary>
    /// Represents a key-name option in a read-only choice object.
    /// </summary>
    [Serializable]
    public class IdNameOption : ReadOnlyModel<IdNameOption>
    {
        #region Business Methods

        private readonly string _hashid;

        public static readonly PropertyInfo<string> IdProperty = RegisterProperty<string>(c => c.Id, RelationshipTypes.PrivateField);
        private long? Key = null;
        public string Id
        {
            get { return GetProperty(IdProperty, KeyHash.Encode(_hashid, Key)); }
            private set { Key = KeyHash.Decode(_hashid, value); }
        }

        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
        public string Name
        {
            get { return GetProperty(NameProperty); }
            private set { LoadProperty(NameProperty, value); }
        }

        #endregion

        #region Data Access

        [FetchChild]
        private void Fetch(
            IdNameOptionDao dao
            )
        {
            Key = dao.Key;
            Name = dao.Name;
        }

        #endregion
    }
}
