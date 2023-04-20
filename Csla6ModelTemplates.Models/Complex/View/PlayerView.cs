using Csla;
using Csla.Data;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Complex.View;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Models.Complex.View
{
    /// <summary>
    /// Represents an item in a read-only player collection.
    /// </summary>
    [Serializable]
    public class PlayerView : ReadOnlyModel<PlayerView>
    {
        #region Properties

        public static readonly PropertyInfo<long?> PlayerKeyProperty = RegisterProperty<long?>(nameof(PlayerKey));
        public long? PlayerKey
        {
            get => GetProperty(PlayerKeyProperty);
            private set => LoadProperty(PlayerKeyProperty, value);
        }

        public static readonly PropertyInfo<long?> PlayerIdProperty = RegisterProperty<long?>(nameof(PlayerId), RelationshipTypes.PrivateField);
        public string PlayerId
        {
            get => KeyHash.Encode(ID.Player, PlayerKey);
            private set => PlayerKey = KeyHash.Decode(ID.Player, value);
        }

        public static readonly PropertyInfo<string> PlayerCodeProperty = RegisterProperty<string>(nameof(PlayerCode));
        public string PlayerCode
        {
            get => GetProperty(PlayerCodeProperty);
            private set => LoadProperty(PlayerCodeProperty, value);
        }

        public static readonly PropertyInfo<string> PlayerNameProperty = RegisterProperty<string>(nameof(PlayerName));
        public string PlayerName
        {
            get => GetProperty(PlayerNameProperty);
            private set => LoadProperty(PlayerNameProperty, value);
        }

        #endregion

        #region Business Rules

        //protected override void AddBusinessRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        new IsInRole(
        //            AuthorizationActions.ReadProperty,
        //            PlayerNameProperty,
        //            "Manager"
        //            )
        //        );
        //}

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(PlayerView),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Data Access

        [FetchChild]
        private void Fetch(
            PlayerViewDao dao
            )
        {
            // Load values from persistent storage.
            DataMapper.Map(dao, this);
        }

        #endregion
    }
}
