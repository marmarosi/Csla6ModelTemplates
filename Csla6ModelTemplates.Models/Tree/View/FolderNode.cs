using Csla;
using Csla.Data;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Tree.View;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Models.Tree.View
{
    /// <summary>
    /// Represents an item in a read-only folder tree.
    /// </summary>
    [Serializable]
    public class FolderNode : ReadOnlyModel<FolderNode>
    {
        #region Properties

        public static readonly PropertyInfo<long?> FolderKeyProperty = RegisterProperty<long?>(nameof(FolderKey));
        public long? FolderKey
        {
            get => GetProperty(FolderKeyProperty);
            private set => LoadProperty(FolderKeyProperty, value);
        }

        public static readonly PropertyInfo<long?> FolderIdProperty = RegisterProperty<long?>(nameof(FolderId), RelationshipTypes.PrivateField);
        public string FolderId
        {
            get => KeyHash.Encode(ID.Folder, FolderKey);
            private set => FolderKey = KeyHash.Decode(ID.Folder, value);
        }

        public static readonly PropertyInfo<long?> ParentKeyProperty = RegisterProperty<long?>(nameof(ParentKey));
        public long? ParentKey
        {
            get => GetProperty(ParentKeyProperty);
            private set => LoadProperty(ParentKeyProperty, value);
        }

        public static readonly PropertyInfo<long?> ParentIdProperty = RegisterProperty<long?>(nameof(ParentId), RelationshipTypes.PrivateField);
        public string ParentId
        {
            get => KeyHash.Encode(ID.Folder, ParentKey);
            private set => ParentKey = KeyHash.Decode(ID.Folder, value);
        }

        public static readonly PropertyInfo<string> FolderNameProperty = RegisterProperty<string>(nameof(FolderName));
        public string FolderName
        {
            get => GetProperty(FolderNameProperty);
            private set => LoadProperty(FolderNameProperty, value);
        }

        public static readonly PropertyInfo<int?> LevelProperty = RegisterProperty<int?>(nameof(Level));
        public int? Level
        {
            get => GetProperty(LevelProperty);
            private set => LoadProperty(LevelProperty, value);
        }

        public static readonly PropertyInfo<FolderNodes> ChildrenProperty = RegisterProperty<FolderNodes>(nameof(Children));
        public FolderNodes Children
        {
            get => GetProperty(ChildrenProperty);
            private set => LoadProperty(ChildrenProperty, value);
        }

        #endregion

        #region Business Rules

        //protected override void AddBusinessRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        new IsInRole(
        //            AuthorizationActions.ReadProperty,
        //            FolderNameProperty,
        //            "Manager"
        //            )
        //        );
        //}

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(FolderNode),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Factory Methods

        //internal static FolderNode Fetch(FolderNodeDao dao)
        //{
        //    return DataPortal.FetchChild<FolderNode>(dao);
        //}

        #endregion

        #region Data Access

        [FetchChild]
        private void Fetch(
            FolderNodeDao dao,
            [Inject] IChildDataPortal<FolderNodes> itemsPortal
            )
        {
            // Load values from persistent storage.
            DataMapper.Map(dao, this, "Children", "FolderOrder");
            Children = itemsPortal.FetchChild(dao.Children);
        }

        #endregion
    }
}
