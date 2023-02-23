using System;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;

namespace migracao_rebranding
{
    public class QuemSomos : Entidade
    {
        public QuemSomos(IConfigurationRoot configurationRoot, string filePathToExport) : base(configurationRoot, filePathToExport)
        {
            ColumnsWithoutId =
                @"title, 
                banner, 
                box_quem_somos_description, 
                box_quem_somos_image, 
                box_quem_somos_summary_name, 
                box_proposito_valores_description, 
                box_proposito_valores_image, 
                box_proposito_valores_summary_name, 
                box_destaques_title, 
                box_destaques_description, 
                box_destaques_cards, 
                box_destaques_summary_name, 
                box_nossos_numeros_title, 
                box_nossos_numeros_description, 
                box_nossos_numeros_text, 
                box_nossos_numeros_summary_name, 
                box_composicao_societaria_title, 
                box_composicao_societaria_background, 
                box_composicao_societaria_summary_name, 
                box_composicao_societaria_text, 
                box_nosso_futuro_title, 
                box_nosso_futuro_image, 
                box_nosso_futuro_summary_name, 
                box_nosso_futuro_description, 
                box_valores_summary_name, 
                box_valores_backgroundImage, 
                box_valores_valores, 
                box_quem_somos_button_id, 
                box_proposito_valores_button_id, 
                box_nossos_numeros_button_id, 
                created_at, 
                updated_at, 
                box_composicao_societaria_background_mobile";
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