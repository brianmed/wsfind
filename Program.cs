using System;
using System.Collections.Generic;
using System.Data.OleDb;

using wsfind.Bll;

namespace wsfind
{
    class Program
    {
        public static Options Options = new Options();

        public static string ConnectionString = "Provider=Search.CollatorDSO;Extended Properties='Application=Windows';";

        static void Main(string[] args)
        {
            Options.ParseOptions(args);
            
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                connection.Open();

                DisplayRows(connection);

                connection.Close();
            }
        }

        static void DisplayRows(OleDbConnection connection)
        {
            string queryInUse = $"SELECT System.ItemPathDisplay FROM SystemIndex {WhereClause()}";

            LogVerbose(queryInUse);

            OleDbDataAdapter adapter = new OleDbDataAdapter(queryInUse, connection);
            OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(adapter);

            using (OleDbCommand commandInUse = adapter.SelectCommand)
            {
                OleDbDataReader reader = commandInUse.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetValue(0));
                }
            }
        }

        static string WhereClause()
        {
            List<string> whereClause = new List<string>();

            foreach (KeyValuePair<string, string> contains in Options.ContainsProperties)
            {
                whereClause.Add($"CONTAINS({contains.Key}, {Quote(contains.Value)})");
            }

            foreach (KeyValuePair<string, string> freetext in Options.FreetextProperties)
            {
                whereClause.Add($"FREETEXT({freetext.Key}, {Quote(freetext.Value)})");
            }

            if (Options.Scope != null) {
                whereClause.Add($"SCOPE = {Quote(Options.Scope)}"); // TODO: Not sure if we need Quote
            }

            if (Options.Directory != null) {
                whereClause.Add($"DIRECTORY = {Quote(Options.Directory)}"); // TODO: Not sure if we need Quote
            }

            return whereClause.Count == 0 ? "" : $" WHERE {String.Join(" AND ", whereClause)}";
        }

        static string Quote(string unquoted)
        {
            OleDbCommandBuilder quoteBuilder = new OleDbCommandBuilder();

            quoteBuilder.QuotePrefix = "'";
            quoteBuilder.QuoteSuffix = "'";

            return quoteBuilder.QuoteIdentifier(unquoted);
        }

        static void LogVerbose(string logLine)
        {
            if (Options.Verbose) {
                System.Console.Error.WriteLine(logLine);
            }
        }
    }
}
