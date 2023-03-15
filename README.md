# export-rebranding

Console application to export SQL files.

Example to appsettings:
```
{
    "ConnectionStrings": {
        "Default": "Server=10.10.10.123;User ID=root;Password=123456;Database=test"
    }
}
```

<h3>How to run?</h3>

`dotnet run`

<h3>Execution order</h3>

1. delete_values_of_rebranding_tables.sql
1. buttons.sql
1. brk_inova.sql
1. compliances.sql
1. conteudos.sql
1. conteudo_pages.sql
1. home_pages.sql
1. imobiliaria_page.sql
1. need_helps.sql
1. news_header.sql
1. nossa_atuacao.sql
1. nossa_atuacao_cidade.sql
1. opening_pages.sql
1. privacy_policy.sql
1. quem_somos.sql
1. quem_somos_cidade.sql
1. search_keys.sql
1. trabalhe_conosco.sql
1. sliders.sql
1. update_campaing_pages.sql
1. update_menu_nodes.sql
1. update_title_buttons.sql

<h3>How to run locally?</h3>

it is recommended to recreate the local database.
```
mysql -u homestead -p
drop database homestead;
create database homestead;
exit;
```

Restore database with backup
```
mysql -u root -p homestead < producao.sql
```

Execute migrations and seeders
```
php artisan migrate
php artisan brk:seed
```

<h3>Technologies</h3>

- .NET 5
- Dapper
- MySQL Connector
