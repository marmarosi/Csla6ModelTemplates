namespace Csla6ModelTemplates.CslaExtensions.Models
{
    /// <summary>
    /// Defines the helper functions of read-only models.
    /// </summary>
    public interface IReadOnlyModel
    {
        T ToDto<T>() where T : class;
    }
}
