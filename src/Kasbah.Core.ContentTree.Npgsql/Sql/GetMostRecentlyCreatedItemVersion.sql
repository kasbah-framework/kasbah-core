select
    id as "Id",
    node_id as "NodeId",
    "timestamp" as "Timestamp",
    "data" as "Data"
from
    node_version
where
    node_id = :id
    and "timestamp" = (select max("timestamp") from node_version where node_id = :id);