namespace SoftUni.Data
{
    internal class SQL_Queries
    {
        public const string GetAllVillainsAndCountOfTheirMinions = @"

             SELECT
                    [v].[Name]
                    , COUNT([mv].[VillainId]) AS [MinionsCount]
               FROM [Villains] AS [v]
         INNER JOIN [MinionsVillains] AS [mv]
                 ON [v].[Id] = [mv].[VillainId]
           GROUP BY [v].[Id], [v].[Name]
             HAVING COUNT([mv].[VillainId]) > 3
           ORDER BY COUNT([mv].[VillainId])";

        public const string GetVillainNameById = @"
            SELECT 
                   [v].[Name]
              FROM [Villains] AS [v]
             WHERE [v].[Id] = @Id";


        public const string GetAllMinionsByVillainId = @"
                  SELECT ROW_NUMBER()
                    OVER (ORDER BY [m].[Name]) AS [RowNum]
                         , [m].[Name]
                         , [m].[Age]
                    FROM [MinionsVillains] AS [mv]
                    JOIN [Minions] AS [m]
                      ON [mv].[MinionId] = [m].[Id]
                   WHERE [mv].[VillainId] = @Id
                ORDER BY [m].[Name] ASC";

        public const string GetVillainName = @"
                    SELECT [v].[Id]
                      FROM [Villains] AS [v]
                     WHERE [v].[Name] = @Name";


        public const string GetMinionName = @"
                     SELECT [m].[Id] 
                       FROM [Minions] AS [m]
                      WHERE [m].[Name] = @Name";


        public const string InsertMinionsVillains = @"
                INSERT INTO [MinionsVillains] ([MinionId], [VillainId]) 
                     VALUES (@minionId, @villainId)";

        public const string InsertVillain = @"
            INSERT INTO [Villains] ([Name], [EvilnessFactorId]) 
                 VALUES (@villainName, 4) ";

        public const string InsertMinion = @"
                INSERT INTO [Minions] ([Name], [Age], [TownId]) 
                     VALUES (@name, @age, @townId)";

        public const string InsertTown = @"
                INSERT INTO [Towns] ([Name]) 
                     VALUES (@townName)";


        public const string FindTownInDB = @"
                SELECT [t].[Id]
                  FROM Towns AS [t]
                 WHERE [t].[Name] = @townName";
    }
}
