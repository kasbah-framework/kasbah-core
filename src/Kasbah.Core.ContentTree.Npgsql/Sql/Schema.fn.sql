create or replace function ensure_structure()
    returns void as
$$
begin

    create table if not exists node (
        id uuid not null default md5(random()::text || clock_timestamp()::text)::uuid unique,
        parent_id uuid references node ( id ),
        alias varchar(256) not null,
        type varchar(256) not null default 'Kasbah.Core.ContentBroker.Models.EmptyItem, Kasbah.Core.ContentBroker',

        constraint uq_alias unique ( parent_id, alias )
    );

    create table if not exists node_version (
        id uuid not null default md5(random()::text || clock_timestamp()::text)::uuid unique,

        node_id uuid not null references node ( id ),

        created_at timestamp not null default (now() at time zone 'utc'),
        modified_at timestamp not null default (now() at time zone 'utc'),

        "data" jsonb
    );

    if not exists (
        select column_name,data_type
        from information_schema.columns
        where table_name = 'node' and column_name = 'active_version_id'
    )
    then
        alter table node add active_version_id uuid references node_version ( id ) on delete cascade;
    end if;

end;
$$ language plpgsql;
