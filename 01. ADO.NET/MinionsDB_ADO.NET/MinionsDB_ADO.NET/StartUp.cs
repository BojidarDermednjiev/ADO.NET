
namespace MinionsDB_ADO.NET
{
    using System.Text;
    using Microsoft.Data.SqlClient;
    using UtilitiesOrMessages;


    public class StartUp
    {
        static async Task Main(string[] args)
        {
            Config config = new Config();

            await using SqlConnection connection = new SqlConnection(config.ReadConnectionString());

            await connection.OpenAsync();

            //To do... Choice one of all methods :)

            await connection.CloseAsync();
        }
        static async Task<string> GetAllVillainsWithTheirMinionsAsync(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand sqlCommand = new SqlCommand(SQL_Queries.GetAllVillainsAndCountOfTheirMinions, connection);
            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
            while (reader.Read())
            {
                string villainName = (string)reader["Name"];
                int minionsCount = (int)reader["MinionsCount"];
                sb.AppendLine(string.Format(OutputMessages.VillainsWithTheirMinionsOutput, villainName, minionsCount));
            }

            return sb.ToString().TrimEnd();
        }
        static async Task<string> GetVillainWithAllMinionsByIdAsync(SqlConnection connection, int villainId)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand getVillainNameCommand = new SqlCommand(SQL_Queries.GetVillainNameById, connection);
            getVillainNameCommand.Parameters.AddWithValue("@Id", villainId);
            object? villainNameObj = await getVillainNameCommand.ExecuteScalarAsync();
            if (villainNameObj == default)
                return string.Format(OutputMessages.NotFoundVillainByIdOutput, villainId);

            string villainName = (string)villainNameObj;
            SqlCommand getAllMinionsCommand = new SqlCommand(SQL_Queries.GetAllMinionsByVillainId, connection);
            getAllMinionsCommand.Parameters.AddWithValue("@Id", villainId);
            SqlDataReader minionsReader = await getAllMinionsCommand.ExecuteReaderAsync();
            sb.AppendLine($"Villain: {villainName}");
            if (!minionsReader.HasRows)
                sb.AppendLine(string.Format(OutputMessages.DoesNotHaveMinionFound));
            else
            {
                while (minionsReader.Read())
                {
                    long rowNum = (long)minionsReader["RowNum"];
                    string minionName = (string)minionsReader["Name"];
                    int minionAge = (int)minionsReader["Age"];
                    sb.AppendLine(string.Format(OutputMessages.MinionInfoOutput, rowNum, minionName, minionAge));
                }
            }

            return sb.ToString().TrimEnd();
        }
        static async Task<string> AddNewMinionAsync(SqlConnection connection, string minionInfo, string villainInfo)
        {
            StringBuilder sb = new StringBuilder();
            string[] minionsArgs = minionInfo.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            string minionName = minionsArgs.First();
            int minionAge = int.Parse(minionsArgs[1]);
            string townName = minionsArgs.Last();
            string[] villainArgs = villainInfo.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            string villainName = villainArgs.First();

            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                int townID = await GetTownOrAddByNameAsync(connection, transaction, sb, townName);
                int villainID = await GetVillainOrAddByNameAsync(connection, transaction, sb, villainName);
                int minionID = await GetMinionsOrAddByNameAsync(connection, transaction, minionName, minionAge, townID);
                await SetMinionToBeServanOfVillainAsync(connection, transaction, minionID, villainID);
                sb.AppendLine(string.Format(OutputMessages.SuccessfullyAddedMinionOutput, minionName, villainName));
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await transaction.RollbackAsync();
            }

            return sb.ToString().TrimEnd();
        }
        private static async Task<int> GetTownOrAddByNameAsync(SqlConnection connection, SqlTransaction transaction,
            StringBuilder sb, string townName)
        {
            SqlCommand getTownIdCmd = new SqlCommand(SQL_Queries.FindTownInDB, connection, transaction);
            getTownIdCmd.Parameters.AddWithValue("@townName", townName);
            object? townIdObj = await getTownIdCmd.ExecuteScalarAsync();

            if (townIdObj == null)
            {
                SqlCommand addNewTownCmd = new SqlCommand(SQL_Queries.InsertTown, connection, transaction);
                addNewTownCmd.Parameters.AddWithValue("@townName", townName);
                await addNewTownCmd.ExecuteNonQueryAsync();
                townIdObj = await getTownIdCmd.ExecuteScalarAsync();
                sb.AppendLine(string.Format(OutputMessages.SuccessfullyAddedTownToDatabaseOutput, townName));
            }

            return (int)townIdObj;
        }
        private static async Task<int> GetVillainOrAddByNameAsync(SqlConnection sqlConnection,
            SqlTransaction transaction, StringBuilder sb, string villainName)
        {
            SqlCommand getTownIdCmd = new SqlCommand(SQL_Queries.GetVillainName, sqlConnection, transaction);
            getTownIdCmd.Parameters.AddWithValue("@Name", villainName);
            object? villainIdObj = await getTownIdCmd.ExecuteScalarAsync();

            if (villainIdObj == null)
            {
                SqlCommand addNewVillainCmd = new SqlCommand(SQL_Queries.InsertVillain, sqlConnection, transaction);
                addNewVillainCmd.Parameters.AddWithValue("@villainName", villainName);
                await addNewVillainCmd.ExecuteNonQueryAsync();
                villainIdObj = await getTownIdCmd.ExecuteScalarAsync();
                sb.AppendLine(string.Format(OutputMessages.SuccessfullyAddedVillainToDatabaseOutput, villainName));
            }

            return (int)villainIdObj;
        }
        private static async Task<int> GetMinionsOrAddByNameAsync(SqlConnection connection, SqlTransaction transaction,
            string minionName, int minionAge, int townId)
        {
            SqlCommand getMinionIdCmd = new SqlCommand(SQL_Queries.GetMinionName, connection, transaction);
            getMinionIdCmd.Parameters.AddWithValue("@name", minionName);
            getMinionIdCmd.Parameters.AddWithValue("@age", minionAge);
            getMinionIdCmd.Parameters.AddWithValue("@townId", townId);
            await getMinionIdCmd.ExecuteNonQueryAsync();
            SqlCommand addminionCmd = new SqlCommand(SQL_Queries.GetMinionName, connection, transaction);
            addminionCmd.Parameters.AddWithValue("@Name", minionName);
            int minionID = (int)await getMinionIdCmd.ExecuteScalarAsync();
            return minionID;
        }
        private static async Task SetMinionToBeServanOfVillainAsync(SqlConnection connection,
            SqlTransaction transaction, int minionID, int villainID)
        {
            SqlCommand setMinionsVillainCmd =
                new SqlCommand(SQL_Queries.InsertMinionsVillains, connection, transaction);
            setMinionsVillainCmd.Parameters.AddWithValue("@minionId", minionID);
            setMinionsVillainCmd.Parameters.AddWithValue("@villainId", villainID);
            await setMinionsVillainCmd.ExecuteNonQueryAsync();
        }
        private static async Task<string> GetTownAndChangeNameWithUpperCaseAsync(SqlConnection connection, string country)
        {
            StringBuilder sb = new StringBuilder();

            int affectedRows = UpdateTowns(connection, country);

            if (affectedRows == 0)
                sb.AppendLine(string.Format(OutputMessages.DoesNotFoundTownNameInDatabaseOutput));
            else
            {

                SqlDataReader reader = GetAllTowns(connection, country);

                sb.AppendLine(string.Format(OutputMessages.AffectedRowsOutput, affectedRows));

                ICollection<string> towns = new List<string>();

                while (reader.Read())
                    towns.Add((string)reader["Name"]);

                sb.AppendLine(string.Format(OutputMessages.CatchAllUpdatedTownsInDatabaseWithUpperCaseOutput, string.Join(", ", towns)));
            }

            return sb.ToString().TrimEnd();
        }
        static int UpdateTowns(SqlConnection connection, string name)
        {
            SqlCommand cmd = new SqlCommand(SQL_Queries.UpdateTownNameQuery, connection);
            cmd.Parameters.AddWithValue(@"countryName", name);
            return cmd.ExecuteNonQuery();
        }
        static SqlDataReader GetAllTowns(SqlConnection connection, string name)
        {
            SqlCommand cmd = new SqlCommand(SQL_Queries.FindCountryQuery, connection);
            cmd.Parameters.AddWithValue(@"countryName", name);
            return cmd.ExecuteReader();
        }

        private static async Task<string> RemoveVillainByIdFromDatabaseAsync(SqlConnection connection, int villainId)
        {
            StringBuilder sb = new StringBuilder();
            SqlTransaction transaction = connection.BeginTransaction();
            string nameOfVillains = await FindVillainFromDatabaseAsync(connection, transaction, villainId);
            int removedMinions = await RemoveMinionFromDatabaseAsync(connection, transaction, villainId);
            int removedVillain = await RemoveVillainFromDatabaseAsync(connection, transaction, villainId);
            if (removedVillain == 0)
                sb.AppendLine(string.Format(OutputMessages.DoesNotFoundVillainFromDatabaseOutput));
            else
            {
                sb.AppendLine(string.Format(OutputMessages.RemoveVillainFromDatabaseOutput, nameOfVillains));
                sb.AppendLine(string.Format(OutputMessages.ReleasedMinionsCountOutput, removedMinions));
            }
            return sb.ToString().TrimEnd();
        }
        private static async Task<string> FindVillainFromDatabaseAsync(SqlConnection connection, SqlTransaction transaction, int villainId)
        {
            SqlCommand cmd = new SqlCommand(SQL_Queries.CatchAllVillainFromDatabase, connection, transaction);
            cmd.Parameters.AddWithValue(@"villainId", villainId);
            object nameOfVillain = await cmd.ExecuteScalarAsync();
            return (string)nameOfVillain;
        }
        private static async Task<int> RemoveVillainFromDatabaseAsync(SqlConnection connection, SqlTransaction transaction, int villainId)
        {
            SqlCommand cmd = new SqlCommand(SQL_Queries.RemoveVillainFromDatabase, connection, transaction);
            return await CmdExecuteNonQueryAsync(cmd, villainId);
        }
        private static async Task<int> RemoveMinionFromDatabaseAsync(SqlConnection connection, SqlTransaction transaction, int villainId)
        {
            SqlCommand cmd = new SqlCommand(SQL_Queries.RemoveMinionVillainFromDatabase, connection, transaction);
            return await CmdExecuteNonQueryAsync(cmd, villainId);
        }
        private static async Task<int> CmdExecuteNonQueryAsync(SqlCommand cmd, int villainId)
        {
            cmd.Parameters.AddWithValue(@"villainId", villainId);
            return await cmd.ExecuteNonQueryAsync();
        }
        private static async Task<ICollection<string>> RangeMinionsAsync(SqlConnection connection)
        {
            List<string> minions = new List<string>();
            SqlCommand cmd = new SqlCommand(SQL_Queries.CatchAllMinions, connection);
            var readMinionsFromData = await cmd.ExecuteReaderAsync();
            while (readMinionsFromData.Read())
                minions.Add((string)readMinionsFromData["Name"]);
            for (int currentMinion = 0; currentMinion < minions.Count; currentMinion++)
            {
                if (currentMinion % 2 == 1)
                {
                    string minion = minions[minions.Count - 1];
                    minions.RemoveAt(minions.Count - 1);
                    minions.Insert(currentMinion, minion);
                }
            }
            return minions;
        }
        private static /*async Task<string> UpdateMinionAgeAsync*/ string UpdateMinionAge(SqlConnection connection, IEnumerable<int> minionIds)
        {
            //StringBuilder sb = new StringBuilder();
            //SqlTransaction transaction = connection.BeginTransaction();
            //foreach (var id in minionIds)
            //{
            //    SqlCommand? minionUpdate = new SqlCommand(SQL_Queries.IncreaseMinionAge, connection, transaction);
            //    minionUpdate.Parameters.AddWithValue(@"Id", id);
            //    await minionUpdate.ExecuteNonQueryAsync();
            //}
            //SqlCommand? cmd = new SqlCommand(SQL_Queries.NameAndAgeMinion, connection, transaction);
            //SqlDataReader? reader = await cmd.ExecuteReaderAsync();
            //while (reader.Read())
            //    sb.AppendLine(string.Format(OutputMessages.MinionNameAndAgeInfoOutput, (string)reader["Name"], (int)reader["Age"]));

            //return sb.ToString();
            StringBuilder sb = new StringBuilder();
            SqlTransaction transaction = connection.BeginTransaction();
            foreach (var id in minionIds)
            {
                SqlCommand? minionUpdate = new SqlCommand(SQL_Queries.IncreaseMinionAge, connection, transaction);
                minionUpdate.Parameters.AddWithValue(@"Id", id);
                minionUpdate.ExecuteNonQuery();
            }
            SqlCommand? cmd = new SqlCommand(SQL_Queries.NameAndAgeMinion, connection, transaction);
            SqlDataReader? reader = cmd.ExecuteReader();
            while (reader.Read())
                sb.AppendLine(string.Format(OutputMessages.MinionNameAndAgeInfoOutput, (string)reader["Name"], (int)reader["Age"]));

            return sb.ToString().TrimEnd();
        }
        public static async Task<string> IncreaseAgeStoredProcedureAsync(SqlConnection connection, int id)
        {
            StringBuilder stringBuilder = new StringBuilder();
            SqlTransaction transaction = connection.BeginTransaction();
            SqlCommand increaseProc = new SqlCommand(SQL_Queries.usp_GetOlder, connection, transaction);
            increaseProc.Parameters.AddWithValue(@"minionId", id);
            await increaseProc.ExecuteNonQueryAsync();
            var minion = new SqlCommand(SQL_Queries.GetMinionById, connection, transaction);
            minion.Parameters.AddWithValue(@"Id", id);
            SqlDataReader? reader = await minion.ExecuteReaderAsync();
            while (reader.Read())
                stringBuilder.AppendLine(string.Format(OutputMessages.MinionFullInfo, (string)reader["Name"], (int)reader["Age"]));

            return stringBuilder.ToString().TrimEnd();
        }

    }

}
