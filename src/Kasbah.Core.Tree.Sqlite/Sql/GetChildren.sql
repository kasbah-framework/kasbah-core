select
	n.id as "Id",
	n.parent_id as "Parent",
	n.alias as "Alias",
	n.active_version_id as "ActiveVersion",
    case (select count(1) from node where parent_id = n.id)
        when 0 then false
        else true
    end as "HasChildren",
    n.type as "Type"
from
	node n
where
	(@id is not null and n.parent_id = @id)
    or (@id is null and n.parent_id is null)
order by
	n.alias;