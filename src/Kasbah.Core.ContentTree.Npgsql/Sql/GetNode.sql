select
	n.id as "Id",
	n.parent_id as "Parent",
	n.alias as "Alias",
	n.current_version_id as "CurrentVersionId",
    case (select count(1) from node where parent_id = n.id)
        when 0 then false
        else true
    end as "HasChildren",
    n.type as "Type"
from
	node n
where
	id = :id;