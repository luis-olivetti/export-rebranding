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
        public bool QueryButtons()
        {
            try
            {
                string sql = $"select id, {string.Join(',', GetColumnsNameToSelectWithQuotationMark())} from buttons order by id";

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

                        if (colName == "is_blank")
                        {
                            sqlValues += $",{fields[colName]}";
                            continue;
                        }

                        if (colName == "url")
                        {
                            sqlValues += $",'{ReplaceUrlToProduction(fields[colName].ToString())}'";
                            continue;
                        }

                        if (colName == "title")
                        {
                            sqlValues += $",'[{fields["id"]}]{fields[colName]}'";
                            continue;
                        }

                        sqlValues += $",'{fields[colName]}'";
                    }

                    sqlValues = sqlValues.Remove(0, 2);

                    string sqlInsert = $"insert into buttons ({string.Join(',', GetColumnsNameToSelectWithQuotationMark())}) values ({sqlValues});";

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

        private string ReplaceUrlToProduction(string url)
        {
            return url.Replace("//qa.brkambiental.com.br", "//brkambiental.com.br");
        }
    }
}