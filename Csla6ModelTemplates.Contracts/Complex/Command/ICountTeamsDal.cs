namespace Csla6ModelTemplates.Contracts.Complex.Command
{
    /// <summary>
    /// Defines the data access functions of the count teams by player count command.
    /// </summary>
    public interface ICountTeamsDal
    {
        List<CountTeamsListItemDao> Execute(CountTeamsCriteria criteria);
    }
}
