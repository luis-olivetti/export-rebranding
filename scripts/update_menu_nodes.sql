update menu_nodes
set url = REPLACE(url, '//qa.brkambiental.com.br', '//www.brkambiental.com.br')
where url like '%//qa.brkambiental.com.br%';

update menu_nodes
set deleted_at = now()
where menu_id = 2
and id in (1191, 9);

-- Fale conosco e filhos
INSERT INTO menu_nodes
(menu_id, parent_id, reference_id, reference_type, url, icon_font, `position`, title, css_class, target, has_child, created_at, updated_at, deleted_at, description)
VALUES(2, 0, 0, NULL, 'https://minhabrk.com.br/home/atendimento', '', 5, 'Fale Conosco', '', '_self', 1, now(), NULL, NULL, '');

INSERT INTO menu_nodes
(menu_id, parent_id, reference_id, reference_type, url, icon_font, `position`, title, css_class, target, has_child, created_at, updated_at, deleted_at, description)
VALUES(2, (select id from menu_nodes where menu_id = 2 and url = 'https://minhabrk.com.br/home/atendimento'), 0, NULL, 'https://minhabrk.com.br/home/info/canais/atendimento', NULL, 1, 'Atendimento', NULL, '_self', 0, now(), now(), NULL, NULL);

INSERT INTO menu_nodes
(menu_id, parent_id, reference_id, reference_type, url, icon_font, `position`, title, css_class, target, has_child, created_at, updated_at, deleted_at, description)
VALUES(2, (select id from menu_nodes where menu_id = 2 and url = 'https://minhabrk.com.br/home/atendimento'), 0, NULL, 'https://www.brkambiental.com.br/contato', NULL, 7, 'Contato', NULL, '_self', 0, now(), now(), NULL, NULL);

UPDATE menu_nodes
set parent_id = (select id from menu_nodes where menu_id = 2 and url = 'https://minhabrk.com.br/home/atendimento')
where id = 549 and menu_id = 2;


INSERT INTO brk_cms_qa.menu_nodes
(menu_id, parent_id, reference_id, reference_type, url, icon_font, `position`, title, css_class, target, has_child, created_at, updated_at, deleted_at, description)
VALUES(2, 0, 0, NULL, 'https://www.brkambiental.com.br/conteudos', NULL, 4, 'Conteúdos', NULL, '_self', 0, now(), now(), NULL, NULL);

INSERT INTO brk_cms_qa.menu_nodes
(menu_id, parent_id, reference_id, reference_type, url, icon_font, `position`, title, css_class, target, has_child, created_at, updated_at, deleted_at, description)
VALUES(2, 0, 0, NULL, 'https://www.brkambiental.com.br/inovacao/brk-inova', NULL, 3, 'Inovação', NULL, '_self', 0, now(), now(), NULL, NULL);

UPDATE menu_nodes
set url = 'https://www.brkambiental.com.br/nossa-atuacao'
where id = 111 and menu_id = 2;