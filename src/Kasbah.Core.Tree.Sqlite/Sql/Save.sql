delete from node_version where id = @id;
insert into node_version ( id, node_id, "data", created_at, modified_at )
values ( @id, @node, @data, datetime(), datetime() );
select * from node_version where id = @id;