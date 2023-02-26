using Csla6ModelTemplates.CslaExtensions.Validations;
using Csla6ModelTemplates.Dal;
using System.Runtime.Serialization;

namespace Csla6ModelTemplates.CslaExtensions
{
    /// <summary>
    /// Represents an exception thrown by a business object for failed validations.
    /// </summary>
    [Serializable]
    public class ValidationException : BackendException
    {
        #region Properties

        /// <summary>
        /// Gets information about the broken validation rules.
        /// </summary>
        public List<ValidationMessage> Messages { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="messages">Information v the failed validations.</param>
        public ValidationException(
            List<ValidationMessage> messages
            )
            : base()
        {
            Messages = messages;
        }

        #endregion

        #region Methods

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion Methods
    }
}
