CREATE TABLE task_item
(
    id INTEGER PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    description VARCHAR(1000) NULL,
    CONSTRAINT task_item_c1 UNIQUE(title)
);

CREATE TABLE task_status
(
    id INTEGER PRIMARY KEY,
    status VARCHAR(50) NOT NULL,
    CONSTRAINT task_status_c1 UNIQUE(status)
);

CREATE TABLE task_history
(
    id INTEGER PRIMARY KEY,
    task_id INTEGER NOT NULL,
    task_status_id INTEGER NOT NULL,
    status_dttm VARCHAR(30) NOT NULL,
    FOREIGN KEY(task_id) REFERENCES task_item(id),
    FOREIGN KEY(task_status_id) REFERENCES task_status(id),
    CONSTRAINT task_history_c1 UNIQUE(task_id, task_status_id, status_dttm)
);



