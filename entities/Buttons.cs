using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace migracao_rebranding
{
    public class Buttons : Entidade
    {
        public Buttons(IConfigurationRoot configurationRoot, string filePathToExport) : base(configurationRoot, filePathToExport)
        {
            ColumnsWithoutId = "title, font, color, url, is_blank, description, created_at, updated_at";
        }
        public bool Execute()
        {
            try
            {
                string sql = $"select id, {string.Join(',', GetColumnsNameToSelectWithQuotationMark())} from {TableName} order by id";

                foreach (var row in Db.Connection.Query<dynamic>(sql))
                {
                    string sqlValues = string.Empty;
                    var fields = row as IDictionary<string, object>;

                    foreach (var colName in GetColumnsNameWithoutIdForValueSection())
                    {
                        sqlValues += Environment.NewLine;
                        sqlValues += PrepareCommonColumnValues(colName, fields);
                    }

                    sqlValues = sqlValues.Remove(0, 2);

                    string sqlInsert = $"insert into {TableName} ({string.Join(',', GetColumnsNameToSelectWithQuotationMark())}) values ({sqlValues});";

                    WriteOnFile(sqlInsert);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        protected override string PrepareCommonColumnValues(string columnName, IDictionary<string, object> fields)
        {
            if (columnName == "is_blank")
            {
                return $",{fields[columnName]}".ToLower();
            }

            if (columnName == "url")
            {
                return $",'{ReplaceUrlToProduction(fields[columnName].ToString())}'";
            }

            if (columnName == "title")
            {
                return $",'[{fields["id"]}]{fields[columnName]}'";
            }

            return base.PrepareCommonColumnValues(columnName, fields);
        }

        private string ReplaceUrlToProduction(string url)
        {
            return url.Replace("//qa.brkambiental.com.br", "//brkambiental.com.br");
        }
    }
}