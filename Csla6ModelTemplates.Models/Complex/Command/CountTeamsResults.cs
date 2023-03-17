using Csla;
using Csla6ModelTemplates.Contracts.Complex.Command;
using Csla6ModelTemplates.CslaExtensions.Models;

namespace Csla6ModelTemplates.Models.Complex.Command
{
    /// <summary>
    /// Represents a read-only count teams result collection.
    /// </summary>
    [Serializable]
    public class CountTeamsResults : ReadOnlyList<CountTeamsResults, CountTeamsResult>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(CountTeamsList),
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
            List<CountTeamsResultDao> list,
            [Inject] IChildDataPortal<CountTeamsResult> itemPortal
            )
        {
            // Load values from persistent storage.
            foreach (var item in list)
                Items.Add(itemPortal.FetchChild(item));
        }

        #endregion
    }
}
