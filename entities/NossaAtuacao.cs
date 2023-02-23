using System;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;

namespace migracao_rebranding
{
    public class NossaAtuacao : Entidade
    {
        public NossaAtuacao(IConfigurationRoot configurationRoot, string filePathToExport) : base(configurationRoot, filePathToExport)
        {
            ColumnsWithoutId =
                @"
                title,
                banner,
                box_o_que_fazemos_summary_name,
                box_o_que_fazemos_left_description,
                box_o_que_fazemos_rigth_description,
                box_onde_estamos_summary_name,
                box_onde_estamos_description,
                box_onde_estamos_background_image,
                box_onde_estamos_image,
                box_atuacao_socioambiental_summary_name,
                box_atuacao_socioambiental_description,
                box_atuacao_socioambiental_background_image,
                box_atuacao_socioambiental_image,
                box_atuacao_socioambiental_button_id,
                box_compromisso_summary_name,
                box_compromisso_description,
                box_compromisso_background_image,
                box_compromisso_image,
                box_cards,
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
    }
}