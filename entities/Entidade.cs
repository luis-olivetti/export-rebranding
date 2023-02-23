using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace migracao_rebranding
{
    public class Entidade
    {
        internal AppDB Db { get; set; }
        private string FilePath { get; set; }
        internal string TableName { get; }
        internal string ColumnsWithoutId { get; set; }

        public Entidade(IConfigurationRoot configurationRoot, string tableName)
        {
            Db = new AppDB(configurationRoot["ConnectionStrings:Default"]);
            TableName = tableName;
            PrepareFolderToScripts(tableName);
        }

        private void PrepareFolderToScripts(string tableName)
        {
            const string folder = "./generated-scripts";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            FilePath = Path.Combine(folder, tableName + ".sql");
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }

        public void WriteOnFile(string sql)
        {
            using StreamWriter file = new(FilePath, append: true);
            file.WriteLine(string.Empty);
            file.WriteLine(sql);
        }

        public string[] GetColumnsNameWithoutIdForValueSection()
        {
            return ColumnsWithoutId
                .Replace(Environment.NewLine, string.Empty)
                .Replace(" ", string.Empty)
                .Split(',');
        }

        public List<string> GetColumnsNameToSelectWithQuotationMark()
        {
            var columnsNameToSelectWithQuotationMark = new List<string>();
            foreach (var item in GetColumnsNameWithoutIdForValueSection())
            {
                columnsNameToSelectWithQuotationMark.Add($"`{item}`");
            }
            return columnsNameToSelectWithQuotationMark;
        }

        public string EscapeJson(object json)
        {
            if (json == null)
            {
                return "null";
            }

            return $"'{json.ToString().Replace(@"\", @"\\").Replace(@"'", @"''")}'";
        }

        protected virtual string PrepareCommonColumnValues(string columnName, IDictionary<string, object> fields)
        {
            if (columnName == "created_at")
            {
                return ",now()";
            }

            if (columnName == "updated_at")
            {
                return ",null";
            }

            if (columnName.Contains("button_id"))
            {
                return $",(select id from buttons where title like '[{fields[columnName]}]%')";
            }

            return $",{EscapeJson(fields[columnName])}";
        }
    }
}