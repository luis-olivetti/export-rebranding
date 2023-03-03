using System;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;

namespace migracao_rebranding
{
    public class TrabalheConosco : Entidade
    {
        public TrabalheConosco(IConfigurationRoot configurationRoot, string filePathToExport) : base(configurationRoot, filePathToExport)
        {
            ColumnsWithoutId =
                @"
                title, 
                banner, 
                created_at, 
                updated_at, 
                box_trabalhe_conosco_anchor_id,
                box_trabalhe_conosco_summary_name, 
                box_trabalhe_conosco_description,
                box_trabalhe_conosco_video, 
                box_trabalhe_conosco_button_id, 
                box_liderenca_brk_anchor_id,
                box_liderenca_brk_summary_name, 
                box_liderenca_brk_description, 
                box_liderenca_brk_image,
                box_ciclo_desenvolvimento_anchor_id, 
                box_ciclo_desenvolvimento_summary_name,
                box_ciclo_desenvolvimento_description_left, 
                box_ciclo_desenvolvimento_description_right,
                box_title_atracao_desenvolvimento_talentos, 
                box_subtitle_atracao_desenvolvimento_talentos,
                box_nossos_programas_anchor_id, 
                box_nossos_programas_description,
                box_nossos_programas_image, 
                box_nossos_programas_description_baixo,
                box_nossos_programas_button_id, 
                box_cultura_bem_estar_description,
                box_onde_estamos_description, 
                box_onde_estamos_image, box_onde_estamos_button_id,
                box_nossas_vagas_description, 
                box_desenvolvimento_talentos_cards,
                box_nossos_programas_cards, 
                box_cultura_bem_estar_cards, 
                box_vagas_cards
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