namespace Csla6ModelTemplates.Contracts.Tree.View
{
    /// <summary>
    /// Represents the criteria of the read-only folder tree object.
    /// </summary>
    [Serializable]
    public class FolderTreeParams
    {
        public string RootId { get; set; }

        public FolderTreeParams(
            string rootId
            )
        {
            RootId = rootId;
        }

        public FolderTreeCriteria Decode()
        {
            return new FolderTreeCriteria
            {
                RootKey = KeyHash.Decode(ID.Folder, RootId) ?? 0
            };
        }
    }

    /// <summary>
    /// Represents the criteria of the read-only folder tree object.
    /// </summary>
    [Serializable]
    public class FolderTreeCriteria
    {
        public long RootKey { get; set; }
    }
}
