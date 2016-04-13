select
    nv.id as "Id",
    nv.node_id as "NodeId",
    nv.created_at as "Created",
    nv.modified_at as "Modified",
    nv."data" as "Data"
from
    node_version nv
    inner join node n on nv.node_id = n.id and nv.id = n.active_version_id
where
    n.id = @id;
