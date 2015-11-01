set -e

docker build -t kasbah-pgsql 01-kasbah-pgsql
docker build -t kasbah-redis 02-kasbah-redis
docker build -t kasbah-solr 03-kasbah-solr

docker build -t kasbah-dev 90-kasbah-dev
