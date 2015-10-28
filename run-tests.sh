case "$1" in
    core)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Tests/project.json test"
        ;;
    events)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Events.Tests/project.json test"
        ;;
    content-tree)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.ContentTree.Tests/project.json test"
        ;;
    content-tree-ngpsql)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.ContentTree.Npgsql.Tests/project.json test"
        ;;
    *)
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Tests/project.json test" & PID_1=$!
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Events.Tests/project.json test" & PID_2=$!
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.ContentTree.Tests/project.json test" & PID_3=$!
        nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.ContentTree.Npgsql.Tests/project.json test" & PID_4=$!
        wait $PID_1
        wait $PID_2
        wait $PID_3
        wait $PID_4
        ;;
esac