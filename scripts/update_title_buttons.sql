update buttons
set title = substring(title, position(']' in title)+1, length(title));