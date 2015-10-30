#!/bin/bash
set -e

if [ "$1" = 'solr' ]; then
    chown -R solr:solr /opt/solr
    /opt/solr/bin/solr -f
fi

exec "$@"
