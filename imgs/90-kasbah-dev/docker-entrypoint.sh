/opt/solr/bin/solr -f & PID_SOLR=$!
/redis-entrypoint.sh redis-server & PID_REDIS=$!
/docker-entrypoint.sh postgres & PID_PGSQL=$!

wait $PID_REDIS && wait $PID_SOLR && wait $PID_PGSQL
