sh /redis-entrypoint.sh &  PID_REDIS=$!
/opt/solr/bin/solr -f & PID_SOLR=$!
sh /pgsql-entrypoint.sh & PID_PGSQL=$!

wait $PID_REDIS && wait $PID_SOLR && wait $PID_PGSQL
