CREATE TABLE task_item
(
    id INTEGER PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    description VARCHAR(1000) NULL
);

CREATE TABLE task_status
(
    id INTEGER PRIMARY KEY,
    status VARCHAR(50) NOT NULL
);

CREATE TABLE task_history
(
    id INTEGER PRIMARY KEY,
    task_id INTEGER NOT NULL,
    task_status_id INTEGER NOT NULL,
    status_dttm VARCHAR(30) NOT NULL,
    FOREIGN KEY(task_id) REFERENCES task_item(id),
    FOREIGN KEY(task_status_id) REFERENCES task_status(id)
);



