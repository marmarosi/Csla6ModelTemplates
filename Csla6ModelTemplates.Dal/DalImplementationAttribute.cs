namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Indicates that the class contains data access implementations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DalImplementationAttribute : Attribute
    { }
}
