namespace MinionsDB_ADO.NET
{
    internal class Config
    {
        private const string Connection_String = @"../../../Connect_String.txt";

            public string ReadConnectionString()
            {
                using StreamReader configReader = new StreamReader(Connection_String);
                return configReader.ReadLine();
            }
    }
}
