namespace Csla6ModelTemplates.Contracts.Tree.View
{
    /// <summary>
    /// Defines the read-only folder node data.
    /// </summary>
    public class FolderNodeData
    {
        public int? FolderOrder { get; set; }
        public string FolderName { get; set; }
        public int? Level { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only folder node object.
    /// </summary>
    public class FolderNodeDao : FolderNodeData
    {
        public long? FolderKey { get; set; }
        public long? ParentKey { get; set; }
        public List<FolderNodeDao> Children { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only folder node object.
    /// </summary>
    public class FolderNodeDto : FolderNodeData
    {
        public string FolderId { get; set; }
        public string ParentId { get; set; }
        public List<FolderNodeDto> Children { get; set; }
    }
}
