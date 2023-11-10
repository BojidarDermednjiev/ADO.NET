namespace MinionsDB_ADO.NET
{
    public class SQL_Queries
    {
        public const string GetAllVillainsAndCountOfTheirMinions = @"
              SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                FROM Villains AS v 
                JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
            GROUP BY v.Id, v.Name 
              HAVING COUNT(mv.VillainId) > 3 
            ORDER BY COUNT(mv.VillainId)";
        public const string GetVillainNameById = @"SELECT Name FROM Villains WHERE Id = @Id";
        public const string GetAllMinionsByVillainId = @"
                                  SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";
        public const string FindTownInDB = @"SELECT Id FROM Towns WHERE Name = @townName";
        public const string InsertTown = @"INSERT INTO Towns (Name) 
                                                VALUES (@townName)";
        public const string GetVillainName = @"SELECT Id FROM Villains WHERE Name = @Name";
        public const string InsertVillain = @"INSERT INTO Villains (Name, EvilnessFactorId)
                                                   VALUES (@villainName, 4)";
        public const string GetMinionName = @"SELECT Id FROM Minions WHERE Name = @Name";
        public const string InsertMinionsVillains = @"INSERT INTO MinionsVillains (MinionId, VillainId) 
                                                           VALUES (@minionId, @villainId)";
        public const string FindCountryQuery = @"
                           SELECT [t].Name 
                             FROM [Towns] as [t]
                             JOIN [Countries] AS [c] ON [c].[Id] = [t].[CountryCode]
                            WHERE [c].[Name] = @countryName";
        public const string UpdateTownNameQuery = @"
                          UPDATE [Towns]
                             SET [Name] = UPPER([Name])
                           WHERE [CountryCode] = (SELECT [c].[Id] FROM [Countries] AS [c] WHERE [c].[Name] = @countryName)";

        public const string CatchAllVillainFromDatabase = @"
                        SELECT Name 
                          FROM Villains 
                         WHERE Id = @villainId";

        public const string RemoveMinionVillainFromDatabase = @"
                        DELETE 
                          FROM MinionsVillains 
                         WHERE VillainId = @villainId";

        public const string RemoveVillainFromDatabase = @"
                        DELETE 
                          FROM Villains
                         WHERE Id = @villainId";

        public const string CatchAllMinions = @"
            SELECT [m].[Name] 
              FROM [Minions] AS [m]";

        public const string IncreaseMinionAge = @"
                 UPDATE [Minions]
                    SET [Name] = UPPER(LEFT([Name], 1)) + SUBSTRING([Name], 2, LEN([Name])), [Age] += 1
                  WHERE [Id] = @Id";

        public const string NameAndAgeMinion = @"
                    SELECT [m].[Name], [m].[Age]
                      FROM [Minions] AS [m]
                ";

        public const string GetMinionById = @"
                 SELECT [m].[Name], [m].[Age] 
                   FROM [Minions] AS [m]
                  WHERE [m].[Id] = @Id";

        public const string usp_GetOlder = @"EXEC usp_GetOlderMinion @id = @minionId";
    }
}
