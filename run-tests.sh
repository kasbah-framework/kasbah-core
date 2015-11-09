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
        dnx -p test/Kasbah.Core.Tests/project.json test
        dnx -p test/Kasbah.Core.Events.Tests/project.json test
        dnx -p test/Kasbah.Core.Events.Redis.Tests/project.json test
        dnx -p test/Kasbah.Core.ContentTree.Tests/project.json test
        dnx -p test/Kasbah.Core.ContentTree.Npgsql.Tests/project.json test
        dnx -p test/Kasbah.Core.Index.Tests/project.json test
        dnx -p test/Kasbah.Core.Index.Solr.Tests/project.json test
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
