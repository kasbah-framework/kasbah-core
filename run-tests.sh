# this assumes that you have the required services running under docker as detailed in docker/README.md
# running on the default docker-machine

if [ -z "$DB" ]; then
    IP=$(docker-machine ip default)
    DB="Server=$IP;Port=50001;Database=postgres;User Id=postgres;Password="
fi
if [ -z "$SOLR" ]; then
    SOLR="http://$IP:50003"
fi
if [ -z "$REDIS" ]; then
    REDIS="$IP:50002"
fi

case "$1" in
    core)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Tests/project.json test"
        ;;
    events)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Events.Tests/project.json test"
        ;;
    events-redis)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Events.Redis.Tests/project.json test"
        ;;
    content-tree)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.ContentTree.Tests/project.json test"
        ;;
    content-tree-npgsql)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.ContentTree.Npgsql.Tests/project.json test"
        ;;
    index)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Index.Tests/project.json test"
        ;;
    index-solr)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Index.Solr.Tests/project.json test"
        ;;
    no-watch)
        dnx -p test/Kasbah.Core.Tests/project.json test -parallel none
        dnx -p test/Kasbah.Core.Events.Tests/project.json test -parallel none
        dnx -p test/Kasbah.Core.Events.Redis.Tests/project.json test -parallel none
        dnx -p test/Kasbah.Core.ContentTree.Tests/project.json test -parallel none
        dnx -p test/Kasbah.Core.ContentTree.Npgsql.Tests/project.json test -parallel none
        dnx -p test/Kasbah.Core.Index.Tests/project.json test -parallel none
        dnx -p test/Kasbah.Core.Index.Solr.Tests/project.json test -parallel none
        ;;
    *)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Tests/project.json test" & PID_1=$!
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Events.Tests/project.json test" & PID_2=$!
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Events.Redis.Tests/project.json test" & PID_3=$!
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.ContentTree.Tests/project.json test" & PID_4=$!
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.ContentTree.Npgsql.Tests/project.json test" & PID_5=$!
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Index.Tests/project.json test" & PID_6=$!
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Index.Solr.Tests/project.json test" & PID_7=$!
        wait $PID_1
        wait $PID_2
        wait $PID_3
        # wait $PID_4
        wait $PID_5
        wait $PID_6
        wait $PID_7
        ;;
esac
