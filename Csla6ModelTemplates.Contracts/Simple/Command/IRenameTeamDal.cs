namespace Csla6ModelTemplates.Contracts.Simple.Command
{
    /// <summary>
    /// Defines the data access functions of the rename team command.
    /// </summary>
    public interface IRenameTeamDal
    {
        void Execute(RenameTeamDao dao);
    }
}
