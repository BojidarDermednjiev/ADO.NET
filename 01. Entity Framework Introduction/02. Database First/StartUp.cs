namespace SoftUni
{
    using Microsoft.Data.SqlClient;
    using Data;
    using System.Text;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public class StartUp
    {
        static async Task Main(string[] args)
        {
            await using SqlConnection connection = new SqlConnection(Config.Connection_String);

            await connection.OpenAsync();

            Console.WriteLine("successfull");

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
                sb.AppendLine($"{villainName} - {minionsCount}");
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
            {
                return $"No villain with ID {villainId} exists in the database.";
            }

            string villainName = (string)villainNameObj;
            SqlCommand getAllMinionsCommand = new SqlCommand(SQL_Queries.GetAllMinionsByVillainId, connection);
            getAllMinionsCommand.Parameters.AddWithValue("@Id", villainId);
            SqlDataReader minionsReader = await getAllMinionsCommand.ExecuteReaderAsync();
            sb.AppendLine($"Villain: {villainName}");
            if (!minionsReader.HasRows)
            {
                sb.AppendLine("(no minions)");
            }
            else
            {
                while (minionsReader.Read())
                {
                    long rowNum = (long)minionsReader["RowNum"];
                    string minionName = (string)minionsReader["Name"];
                    int minionAge = (int)minionsReader["Age"];
                    sb.AppendLine($"{rowNum}. {minionName} {minionAge}");
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
                await SetMinionToBeServanOfVillainAsync(connection , transaction, minionID, villainID);
                sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await transaction.RollbackAsync();
            }
            return sb.ToString().TrimEnd();   
        }
        private static async Task<int> GetTownOrAddByNameAsync(SqlConnection connection, SqlTransaction transaction, StringBuilder sb, string townName)
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
                sb.AppendLine($"Town {townName} was added to the database.");
            }

            return (int)townIdObj;
        }
        private static async Task<int> GetVillainOrAddByNameAsync(SqlConnection sqlConnection, SqlTransaction transaction, StringBuilder sb, string villainName)
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
                sb.AppendLine($"Villain {villainName} was added to the database.");
            }
            return (int)villainIdObj;
        }
        private static async Task<int> GetMinionsOrAddByNameAsync(SqlConnection connection, SqlTransaction transaction, string minionName, int minionAge, int townId)
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
        private static async Task SetMinionToBeServanOfVillainAsync(SqlConnection connection, SqlTransaction transaction, int minionID, int villainID)
        {
            SqlCommand setMinionsVillainCmd = new SqlCommand(SQL_Queries.InsertMinionsVillains, connection, transaction);
            setMinionsVillainCmd.Parameters.AddWithValue("@minionId", minionID);
            setMinionsVillainCmd.Parameters.AddWithValue("@villainId", villainID);
            await setMinionsVillainCmd.ExecuteNonQueryAsync();
        }
    }
}

