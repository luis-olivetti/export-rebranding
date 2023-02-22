using System;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;

namespace migracao_rebranding
{
    public class NossaAtuacaoCidade : Entidade
    {
        public NossaAtuacaoCidade(IConfigurationRoot configurationRoot, string filePathToExport) : base(configurationRoot, filePathToExport)
        {
            ColumnsWithoutId =
                @"
                title,
                banner,
                box_o_que_fazemos_summary_name,
                box_o_que_fazemos_left_description,
                box_o_que_fazemos_rigth_description,
                box_nossa_atuacao_em_video,
                box_nossa_atuacao_em_description,
                box_onde_estamos_summary_name,
                box_onde_estamos_description,
                box_onde_estamos_image,
                box_nossa_operacao_summary_name,
                box_nossa_operacao_description,
                box_nossa_operacao_cards,
                box_abastecimento_summary_name,
                box_abastecimento_left_description,
                box_abastecimento_rigth_description,
                box_abastecimento_cards,
                box_relatorio_qualidade_left_description,
                box_relatorio_qualidade_rigth_description,
                relatorio_mensal_file,
                relatorio_anual_files,
                box_tratamento_esgoto_summary_name,
                box_tratamento_esgoto_left_description,
                box_tratamento_esgoto_rigth_description,
                box_estacoes_tratamento_esgoto_left_description,
                box_estacoes_tratamento_esgoto_rigth_description,
                box_estacoes_tratamento_esgoto_cards_estacoes,
                box_estacoes_tratamento_esgoto_cards_niveis,
                box_saiba_mais_sobre_cidade,
                box_saiba_mais_sobre_cidade_cards,
                box_atuacao_socioambiental_summary_name,
                box_atuacao_socioambiental_description,
                box_atuacao_socioambiental_image,
                at_socioamb_button_id,
                box_compromisso_summary_name,
                box_compromisso_description,
                box_compromisso_image,
                box_cards,
                cidade_id,
                created_at,
                updated_at,
                box_nossa_operacao_background_image,
                box_nossa_operacao_background_image_mobile,
                box_nossa_atuacao_summary_name
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

                        if (colName.Contains("_button_id"))
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