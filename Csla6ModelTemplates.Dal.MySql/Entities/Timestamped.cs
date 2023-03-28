namespace Csla6ModelTemplates.Dal.MySql.Entities
{
    /// <summary>
    /// Define a base class for entities with timestamp column.
    /// </summary>
    public abstract class Timestamped
    {
        public DateTimeOffset Timestamp { get; set; }
    }
}
