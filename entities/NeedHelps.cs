using System;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;

namespace migracao_rebranding
{
    public class NeedHelps : Entidade
    {
        public NeedHelps(IConfigurationRoot configurationRoot, string filePathToExport) : base(configurationRoot, filePathToExport)
        {
            ColumnsWithoutId =
                @"
                description,
                background_image,
                mobile_background_image,
                button_id,
                created_at,
                updated_at
                ";
        }

        public bool Execute()
        {
            try
            {
                string sql = $"select {string.Join(',', GetColumnsNameToSelectWithQuotationMark())} from {TableName} order by id";

                foreach (var row in Db.Connection.Query<dynamic>(sql))
                {
                    string sqlValues = string.Empty;
                    var fields = row as IDictionary<string, object>;

                    foreach (var colName in GetColumnsNameWithoutIdForValueSection())
                    {
                        sqlValues += Environment.NewLine;

                        if (colName == "created_at")
                        {
                            sqlValues += ",now()";
                            continue;
                        }

                        if (colName == "updated_at")
                        {
                            sqlValues += ",null";
                            continue;
                        }

                        if (colName.Contains("_cards"))
                        {
                            sqlValues += $",'{EscapeJson(fields[colName])}'";
                            continue;
                        }

                        if (colName.Contains("button_id"))
                        {
                            sqlValues += $",(select id from buttons where title like '[{fields[colName]}]%')";
                            continue;
                        }

                        sqlValues += $",'{fields[colName]}'";
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
    }
}