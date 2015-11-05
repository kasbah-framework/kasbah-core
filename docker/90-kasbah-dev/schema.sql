create table node (
    id uuid not null default md5(random()::text || clock_timestamp()::text)::uuid unique,
    parent_id uuid references node ( id ),
    alias varchar(256) not null,
    type varchar(256) not null default 'Kasbah.Core.Models.EmptyItem, Kasbah.Core'
);

create table node_version (
    id uuid not null default md5(random()::text || clock_timestamp()::text)::uuid unique,

    node_id uuid not null references node ( id ),

    created_at timestamp not null default (now() at time zone 'utc'),
    modified_at timestamp not null default (now() at time zone 'utc'),

    "data" jsonb
);

create unique index uq_alias on node ( parent_id, alias );

alter table node add active_version_id uuid references node_version ( id );
