insert into node_version ( id, node_id, "data" )
values ( :id, :node, :data::jsonb )
on conflict ( id ) do
    update set
        "data" = excluded.data,
        modified_at = now() at time zone 'utc'
returning
    id as "Id",
    node_id as "NodeId",
    created_at as "Created",
    modified_at as "Modified";