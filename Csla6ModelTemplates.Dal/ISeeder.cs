namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Defines the functionality of a database seeder.
    /// </summary>
    public interface ISeeder
    {
        void SeedProductionData(string contentRootPath);
        void SeedDevelopmentData(string contentRootPath);
    }
}
