select
	id as "Id",
	parent_id as "ParentId",
	alias as "Alias",
	current_version_id as "CurrentVersionId"
from
	node
where
	parent_id = :id
order by
	alias;