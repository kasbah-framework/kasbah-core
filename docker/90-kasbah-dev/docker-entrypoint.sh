ls -la /opt/solr/bin

/opt/solr/bin/solr -f & PID_SOLR=$!
# /solr-entrypoint.sh solr & PID_SOLR=$!
/redis-entrypoint.sh redis-server & PID_REDIS=$!
/pgsql-entrypoint.sh postgres & PID_PGSQL=$!

wait $PID_REDIS && wait $PID_SOLR && wait $PID_PGSQL
