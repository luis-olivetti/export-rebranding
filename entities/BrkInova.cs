using System;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;

namespace migracao_rebranding
{
    public class BrkInova : Entidade
    {
        public BrkInova(IConfigurationRoot configurationRoot, string filePathToExport) : base(configurationRoot, filePathToExport)
        {
            ColumnsWithoutId =
                @"
                title,
                banner,
                short_description,
                box_inovacao_aberta_description,
                box_inovacao_aberta_items,
                box_inovacao_aberta_summary_name,
                box_datathon_description,
                box_datathon_image,
                box_datathon_anchor_id,
                box_datathon_summary_name,
                box_datathon_button_id,
                box_pitch_description,
                box_pitch_image,
                box_pitch_anchor_id,
                box_pitch_summary_name,
                box_pitch_button_id,
                box_parceiros_title,
                box_parceiros_cards,
                box_parceiros_summary_name,
                box_time_title,
                box_time_cards,
                box_time_summary_name,
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