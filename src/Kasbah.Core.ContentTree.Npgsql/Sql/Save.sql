insert into node_version ( id, node_id, "data" )
values ( :id, :nodeId, :data )
on conflict ( id ) do
    update set
        "data" = excluded.data,
        modified_at = current_timestamp;