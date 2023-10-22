using Csla;
using Csla.Core;
using Csla.Data;
using Csla.Rules;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Junction.Edit;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.CslaExtensions.Validations;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Resources;

namespace Csla6ModelTemplates.Models.Junction.Edit
{
    /// <summary>
    /// Represents an editable group-person object.
    /// </summary>
    [Serializable]
    [ValidationResourceType(typeof(ValidationText), ObjectName = "GroupPerson")]
    public class GroupPerson : EditableModel<GroupPerson, GroupPersonDto>
    {
        #region Properties

        public static readonly PropertyInfo<long?> PersonKeyProperty = RegisterProperty<long?>(nameof(PersonKey));
        public long? PersonKey
        {
            get => GetProperty(PersonKeyProperty);
            private set => LoadProperty(PersonKeyProperty, value);
        }

        public static readonly PropertyInfo<long?> PersonIdProperty = RegisterProperty<long?>(nameof(PersonId), RelationshipTypes.PrivateField);
        public string PersonId
        {
            get => KeyHash.Encode(ID.Person, PersonKey);
            set => PersonKey = KeyHash.Decode(ID.Person, value);
        }

        public static readonly PropertyInfo<string> PersonNameProperty = RegisterProperty<string>(nameof(PersonName));
        public string PersonName
        {
            get => GetProperty(PersonNameProperty);
            private set => LoadProperty(PersonNameProperty, value);
        }

        #endregion

        #region Business Rules

        protected override void AddBusinessRules()
        {
            // Call base class implementation to add data annotation rules to BusinessRules.
            // NOTE: DataAnnotation rules is always added with Priority = 0.
            base.AddBusinessRules();

            //// Add validation rules.
            BusinessRules.AddRule(new UniquePersonIds(PersonIdProperty));

            //// Add authorization rules.
            //BusinessRules.AddRule(
            //    new IsInRole(
            //        AuthorizationActions.WriteProperty,
            //        PersonNameProperty,
            //        "Manager"
            //        )
            //    );
        }

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(GroupPerson),
        //        new IsInRole(
        //            AuthorizationActions.EditObject,
        //            "Manager"
        //            )
        //        );
        //}

        private sealed class UniquePersonIds : BusinessRule
        {
            // Add additional parameters to your rule to the constructor.
            public UniquePersonIds(
                IPropertyInfo primaryProperty
                )
              : base(primaryProperty)
            {
                // If you are  going to add InputProperties make sure to
                // uncomment line below as InputProperties is NULL by default.
                //if (InputProperties == null) InputProperties = new List<IPropertyInfo>();

                // Add additional constructor code here.

                // Marke rule for IsAsync if Execute contains asyncronous code IsAsync = true; 
            }

            protected override void Execute(
                IRuleContext context
                )
            {
                GroupPerson target = (GroupPerson)context.Target;
                if (target.Parent == null)
                    return;

                Group group = (Group)target.Parent.Parent;
                var count = group.Persons.Count(gp => gp.PersonId == target.PersonId);
                if (count > 1)
                    context.AddErrorResult(ValidationText.GroupPerson_PersonId_NotUnique);
            }
        }

        #endregion

        #region Business Methods

        /// <summary>
        /// Updates an editable model and its children from the data transfer object.
        /// </summary>
        /// <param name="dto">The data transfer object.</param>
        /// <param name="childFactory">The child data portal factory.</param>
        public override void SetValuesOnBuild(
            GroupPersonDto dto,
            IChildDataPortalFactory childFactory
            )
        {
            DataMapper.Map(dto, this);
            BusinessRules.CheckRules();
        }

        #endregion

        #region Data Access

        [CreateChild]
        private void Create()
        {
            // Set values from data transfer object.
            //LoadProperty(PersonNameProperty, "");
            //BusinessRules.CheckRules();
        }

        [FetchChild]
        private void Fetch(
            GroupPersonDao dao
            )
        {
            // Load values from persistent storage.
            using (BypassPropertyChecks)
                DataMapper.Map(dao, this, "GroupKey");
        }

        [InsertChild]
        private void Insert(
            Group parent,
            [Inject] IGroupPersonDal dal
            )
        {
            // Insert values into persistent storage.
            using (BypassPropertyChecks)
            {
                var dao = Copy.PropertiesFrom(this).ToNew<GroupPersonDao>();
                dao.GroupKey = parent.GroupKey;
                dal.Insert(dao);

                // Set new data.
                PersonKey = dao.PersonKey;
            }
            //FieldManager.UpdateChildren(this);
        }

        [UpdateChild]
        private void Update(
            Group parent,
            [Inject] IGroupPersonDal dal
            )
        {
            // Update values in persistent storage.
            throw new NotImplementedException();
        }

        [DeleteSelfChild]
        private void Child_DeleteSelf(
            Group parent,
            [Inject] IGroupPersonDal dal
            )
        {
            // Delete values from persistent storage.

            //Items.Clear();
            //FieldManager.UpdateChildren(this);

            GroupPersonDao dao = Copy.PropertiesFrom(this).Omit("PersonId").ToNew<GroupPersonDao>();
            dao.GroupKey = parent.GroupKey;
            dal.Delete(dao);
        }

        #endregion
    }
}
