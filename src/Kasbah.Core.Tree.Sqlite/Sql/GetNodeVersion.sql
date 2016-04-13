select
    nv.id as "Id",
    nv.node_id as "NodeId",
    nv.created_at as "Created",
    nv.modified_at as "Modified",
    nv."data" as "Data"
from
    node_version nv
where
    nv.node_id = @id
    and nv.id = @version;
