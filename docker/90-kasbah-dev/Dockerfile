FROM kasbah-solr

USER root

ADD schema.sql /docker-entrypoint-initdb.d/

ADD docker-entrypoint.sh /entrypoint.sh

ENTRYPOINT ["sh", "/entrypoint.sh"]

