select
	n.id as "Id",
	n.parent_id as "ParentId",
	n.alias as "Alias",
	n.current_version_id as "CurrentVersionId",
    case (select count(1) from node where parent_id = n.id)
        when 0 then false
        else true
    end as "HasChildren"
from
	node n
where
	(:id is not null and n.parent_id = :id)
    or (:id is null and n.parent_id is null)
order by
	n.alias;