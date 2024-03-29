using System;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;

namespace migracao_rebranding
{
    public class QuemSomosCidade : Entidade
    {
        public QuemSomosCidade(IConfigurationRoot configurationRoot, string filePathToExport) : base(configurationRoot, filePathToExport)
        {
            ColumnsWithoutId =
                @"box_quem_somos_cidade_description, 
                box_quem_somos_cidade_video, 
                box_quem_somos_cidade_anchor_id, 
                box_nossos_numeros_text, 
                box_sobre_cidade_anchor_id, 
                box_sobre_cidade_cards, 
                quem_somos_id, 
                cidade_id, 
                created_at, 
                updated_at, 
                box_quem_somos_cidade_summary_name, 
                box_quem_somos_cidade_saiba_mais_summary_name";
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
            if (columnName == "quem_somos_id")
            {
                return ",(select max(id) from quem_somos)";
            }

            return base.PrepareCommonColumnValues(columnName, fields);
        }
    }
}