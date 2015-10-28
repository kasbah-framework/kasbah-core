insert into node ( id, parent_id, alias )
values ( :id, :parent_id, :alias )
on conflict ( id ) do
    update set
        parent_id = excluded.parent_id,
        alias = excluded.alias;