namespace Csla6ModelTemplates.Contracts.Complex.Set
{
    /// <summary>
    /// Defines the editable team set item data.
    /// </summary>
    public class TeamSetItemData
    {
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public DateTime? Timestamp { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the editable team set item object.
    /// </summary>
    public class TeamSetItemDao : TeamSetItemData
    {
        public long? TeamKey { get; set; }
        public List<TeamSetPlayerDao> Players { get; set; }

        public TeamSetItemDao()
        {
            Players = new List<TeamSetPlayerDao>();
        }
    }

    /// <summary>
    /// Defines the data transfer object of the editable team set item object.
    /// </summary>
    public class TeamSetItemDto : TeamSetItemData
    {
        public string TeamId { get; set; }
        public List<TeamSetPlayerDto> Players { get; set; }

        public TeamSetItemDto()
        {
            Players = new List<TeamSetPlayerDto>();
        }

        public TeamSetItemDao ToDao()
        {
            return new TeamSetItemDao
            {
                TeamKey = KeyHash.Decode(ID.Team, TeamId),
                TeamCode = TeamCode,
                TeamName = TeamName,
                Players = PlayersToDao(),
                Timestamp = Timestamp
            };
        }

        protected List<TeamSetPlayerDao> PlayersToDao()
        {
            List<TeamSetPlayerDao> list = new List<TeamSetPlayerDao>();

            foreach (TeamSetPlayerDto player in Players)
                list.Add(player.ToDao());

            return list;
        }
    }
}
