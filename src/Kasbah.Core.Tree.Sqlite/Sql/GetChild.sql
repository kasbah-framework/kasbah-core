select
	n.id as "Id",
	n.parent_id as "Parent",
	n.alias as "Alias",
	n.active_version_id as "ActiveVersion",
    case (select count(1) from node where parent_id = n.id)
        when 0 then 0
        else 1
    end as "HasChildren",
    n.type as "Type"
from
	node n
where
	((@parent is not null and n.parent_id = @parent)
    or (@parent is null and n.parent_id is null))
    and n.alias = @alias;