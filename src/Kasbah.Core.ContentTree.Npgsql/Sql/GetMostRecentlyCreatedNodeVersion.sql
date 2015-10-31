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
    and modified_at = (select max(modified_at) from node_version where node_id = :id);