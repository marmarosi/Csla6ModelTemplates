using System.Net;
using System.Runtime.Serialization;

namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// This class is the base exception that is thrown when a
    /// non-fatal application error occurs in the application.
    /// </summary>
    [Serializable]
    public class BackendException : ApplicationException
    {
        #region Variables

        //IPrincipal _user = null;

        #endregion Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BackendException"/> class.
        /// </summary>
        public BackendException()
            : base()
        {
            Init(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackendException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public BackendException(
            string message
            )
            : base(message)
        {
            Init(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackendException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference, the current exception
        /// is raised in a catch block that handles the inner exception.</param>
        public BackendException(
            string message,
            Exception inner
            )
            : base(message, inner)
        {
            Init(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackendException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected BackendException(
            SerializationInfo info,
            StreamingContext context
            )
            : base(info, context)
        {
            Init(HttpStatusCode.BadRequest);
        }

        private void Init(
            HttpStatusCode statusCode
            )
        {
            //_user = HttpContext.Current.User as IPrincipal;
            StatusCode = (int)statusCode;
        }

        #endregion Constructors

        #region Properties
        /*
        /// <summary>
        /// Gets the current user.
        /// </summary>
        public IPrincipal User
        {
            get { return _user; }
        }
        */
        public int StatusCode { get; set; }

        #endregion Properties

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
        public override void GetObjectData(
            SerializationInfo info,
            StreamingContext context
            )
        {
            base.GetObjectData(info, context);
        }

        #endregion Methods
    }
}
