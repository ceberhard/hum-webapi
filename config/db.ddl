
create table task_item
(
    id integer primary key asc not null,
    title varchar(100) not null,
    description varchar(1000) null
);

create table task_status
(
    id integer primary key asc not null,
    status varchar(50) not null
);

create table task_history
(
    id integer primary key asc not null,
    task_id integer not null,
    task_status_id integer not null,
    status_dttm varchar(30) not null,
    foreign key(task_id) references task_item(id),
    foreign key(task_status_id) references task_status(id)
);



