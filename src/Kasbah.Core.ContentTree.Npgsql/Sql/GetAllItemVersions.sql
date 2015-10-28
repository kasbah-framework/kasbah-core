select
    id as "Id",
    node_id as "NodeId",
    "timestamp" as "Timestamp",
    "data" as "Data"
from
    node_version
where
    node_id = :id
order by
    "timestamp"