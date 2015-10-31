select
    id as "Id",
    node_id as "NodeId",
    created_at as "Created",
    modified_at as "Modified",
    "data" as "Data"
from
    node_version
where
    node_id = :id
order by
    created_at