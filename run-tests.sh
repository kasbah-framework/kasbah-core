nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Tests/project.json test" & PID_1=$!
nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.Events.Tests/project.json test" & PID_2=$!
nodemon -e cs,sql,json --exec "dnx -p test/Kasbah.Core.ContentTree.Tests/project.json test" & PID_3=$!
wait $PID_1
wait $PID_2
wait $PID_3