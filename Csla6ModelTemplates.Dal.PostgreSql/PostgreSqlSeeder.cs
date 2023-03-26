using Csla6ModelTemplates.Dal.PostgreSql.Entities;

namespace Csla6ModelTemplates.Dal.PostgreSql
{
    /// <summary>
    /// Database seeder.
    /// </summary>
    public static class PostgreSqlSeeder
    {
        private static Random random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Initializes the database schema and fills it with demo data.
        /// </summary>
        /// <param name="isDevelopment">The database context.</param>
        /// <param name="isDevelopment">Indicates whether the app is running in development mode.</param>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void Run(
            PostgreSqlContext context,
            bool isDevelopment,
            string contentRootPath
            )
        {
            if (isDevelopment)
                context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            if (!isDevelopment)
                return;

            #region Team data

            for (int i = 0; i < 50; i++)
            {
                int serialNumber = i + 1;
                Team team = new Team
                {
                    TeamCode = $"T-{serialNumber.ToString("0000")}",
                    TeamName = $"Team entry number {serialNumber}",
                };
                context.Teams.Add(team);
                context.SaveChanges();

                int count = random.Next(1, 5);
                for (int j = 0; j < count; j++)
                {
                    int index = j + 1;
                    context.Players.Add(new Player
                    {
                        TeamKey = team.TeamKey,
                        PlayerCode = $"P-{serialNumber.ToString("0000")}-{index}",
                        PlayerName = $"Item entry number {serialNumber}.{index}",
                    });
                }
                context.SaveChanges();
            }

            #endregion

            #region Folder data

            CreateFolderLevel(context, 1, null, null, null);

            #endregion

            #region Group data

            List<long> groupKeys = new List<long>();
            for (int i = 0; i < 20; i++)
            {
                int serialNumber = i + 1;
                Group group = new Group
                {
                    GroupCode = $"G-{serialNumber.ToString("00")}",
                    GroupName = $"Group No. {serialNumber}",
                };
                context.Groups.Add(group);
                context.SaveChanges();
                groupKeys.Add(group.GroupKey.Value);
            }

            #endregion

            #region Person data

            List<long> personKeys = new List<long>();
            for (int i = 0; i < 20; i++)
            {
                int serialNumber = i + 1;
                Person person = new Person
                {
                    PersonCode = $"XY-{serialNumber.ToString("00")}",
                    PersonName = $"Person #{serialNumber}",
                };
                context.Persons.Add(person);
                context.SaveChanges();
                personKeys.Add(person.PersonKey.Value);
            }

            #endregion

            #region GroupPerson data

            foreach (long groupKey in groupKeys)
            {
                int count = random.Next(1, 5);
                List<long> tempKeys = personKeys.GetRange(0, 20);
                for (int j = 0; j < count; j++)
                {
                    int index = random.Next(1, 20 - j);
                    long personKey = tempKeys[index];
                    context.GroupPersons.Add(new GroupPerson
                    {
                        GroupKey = groupKey,
                        PersonKey = personKey
                    });
                    tempKeys.Remove(personKey);
                }
            }
            context.SaveChanges();

            #endregion
        }

        #region Folder helpers

        private static void CreateFolderLevel(
            PostgreSqlContext context,
            int level,
            long? parentKey,
            long? rootKey,
            string parentPath
            )
        {
            int count = level == 1 ? 3 : random.Next(1, 5);
            for (int i = 0; i < count; i++)
            {
                int folderOrder = i + 1;
                Folder folder = CreateFolder(
                    parentKey,
                    rootKey,
                    folderOrder,
                    parentPath
                    );
                context.Folders.Add(folder);
                context.SaveChanges();

                if (level == 1)
                {
                    folder.RootKey = folder.FolderKey;
                    context.SaveChanges();
                }

                if (level < 4)
                {
                    string path = parentPath == null
                        ? folderOrder.ToString()
                        : $"{parentPath}.{folderOrder}";
                    CreateFolderLevel(
                        context,
                        level + 1,                      // level
                        folder.FolderKey,               // parentKey
                        rootKey ?? folder.FolderKey,    // teamKey
                        path                            // parentPath
                        );
                }
            }
        }

        private static Folder CreateFolder(
            long? parentKey,
            long? rootKey,
            int? folderOrder,
            string parentPath
            )
        {
            return new Folder
            {
                ParentKey = parentKey,
                RootKey = rootKey,
                FolderOrder = folderOrder,
                FolderName = parentPath == null
                    ? $"Folder entry number {folderOrder}"
                    : $"Folder entry number {parentPath}.{folderOrder}"
            };
        }

        #endregion
    }
}
