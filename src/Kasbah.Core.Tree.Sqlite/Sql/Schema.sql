CREATE TABLE node ( id text not null primary key, parent_id text, alias text not null, type text not null, active_version_id text );
CREATE TABLE node_version ( id text not null primary key, node_id text not null, created_at text not null, modified_at text not null, data text, foreign key ( node_id ) references node ( id ));
