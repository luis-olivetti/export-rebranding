update pages
set template = REPLACE(template, 'campaign-page.', '')
where campaign = 1;