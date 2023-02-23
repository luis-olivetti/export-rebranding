using System;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;

namespace migracao_rebranding
{
    public class Compliances : Entidade
    {
        public Compliances(IConfigurationRoot configurationRoot, string filePathToExport) : base(configurationRoot, filePathToExport)
        {
            ColumnsWithoutId =
                @"
                title,
                banner,
                box_description_title,
                box_description_subtitle,
                box_description_text_left,
                box_description_text_right,
                box_description_image,
                box_description_sliders,
                box_confidencial_description,
                box_confidencial_image,
                box_integridade_description,
                box_integridade_list,
                box_reconhecimento_description,
                box_reconhecimento_cards,
                box_normativos_description,
                box_normativos_json,
                box_patrocinios_description,
                box_patrocinios_header_type,
                box_patrocinios_header_description,
                box_patrocinios_header_ano,
                box_patrocinios_json,
                box_transparencia_description,
                box_transparencia_select_description,
                box_transparencia_json,
                box_confidencial_button_id,
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