update menu_nodes
set url = REPLACE(url, '//qa.brkambiental.com.br', '//www.brkambiental.com.br')
where url like '%//qa.brkambiental.com.br%';