FROM kasbah-solr


COPY docker-entrypoint.sh /all-entrypoint.sh
RUN chmod +x /all-entrypoint.sh

ENTRYPOINT ["/all-entrypoint.sh"]
