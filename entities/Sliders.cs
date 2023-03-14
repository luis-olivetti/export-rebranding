using System;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;

namespace migracao_rebranding
{
    public class Sliders : Entidade
    {
        public Sliders(IConfigurationRoot configurationRoot, string filePathToExport) : base(configurationRoot, filePathToExport)
        {
            ColumnsWithoutId =
                @"
                description, 
                image, 
                sequence, 
                button_id, 
                owner_type, 
                owner_id, 
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
            if (columnName == "owner_id")
            {
                var ownerType = fields["owner_type"].ToString();
                if (ownerType.Contains("Compliance"))
                {
                    return ",(select max(id) from compliances)";
                }
                else if (ownerType.Contains("HomePage"))
                {
                    return ",(select max(id) from home_pages)";
                }
            }

            return base.PrepareCommonColumnValues(columnName, fields);
        }
    }
}