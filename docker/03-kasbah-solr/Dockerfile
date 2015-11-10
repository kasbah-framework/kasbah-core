# taken from... https://raw.githubusercontent.com/makuk66/docker-solr/master/5.3/Dockerfile

FROM    kasbah-redis
# MAINTAINER  Martijn Koster "mak-docker@greenhills.co.uk"

#### BEGIN java:openjdk-8-jre

# taken from... https://raw.githubusercontent.com/docker-library/java/master/openjdk-8-jre/Dockerfile

# FROM buildpack-deps:jessie-curl

RUN echo 'deb http://httpredir.debian.org/debian jessie-backports main' > /etc/apt/sources.list.d/jessie-backports.list

# Default to UTF-8 file.encoding
ENV LANG C.UTF-8

ENV JAVA_VERSION 8u66
ENV JAVA_DEBIAN_VERSION 8u66-b01-1~bpo8+1

# see https://bugs.debian.org/775775
# and https://github.com/docker-library/java/issues/19#issuecomment-70546872
ENV CA_CERTIFICATES_JAVA_VERSION 20140324

RUN set -x \
  && apt-get update \
  && apt-get install -y \
    unzip lsof wget \
    openjdk-8-jre-headless \
    ca-certificates-java="$CA_CERTIFICATES_JAVA_VERSION" \
  && rm -rf /var/lib/apt/lists/*

# see CA_CERTIFICATES_JAVA_VERSION notes above
RUN /var/lib/dpkg/info/ca-certificates-java.postinst configure

# see https://bugs.debian.org/793210
# and https://github.com/docker-library/java/issues/46#issuecomment-119026586
RUN apt-get update && apt-get install -y --no-install-recommends libfontconfig1 && rm -rf /var/lib/apt/lists/*

# If you're reading this and have any feedback on how this image could be
#   improved, please open an issue or a pull request so we can discuss it!



#### END java:openjdk-8-jre

ENV SOLR_USER solr
ENV SOLR_UID 8983

RUN groupadd -r $SOLR_USER && \
  useradd -r -u $SOLR_UID -g $SOLR_USER $SOLR_USER

ENV SOLR_KEY CFCE5FBB920C3C745CEEE084C38FF5EC3FCFDB3E
RUN gpg --keyserver ha.pool.sks-keyservers.net --recv-keys "$SOLR_KEY"

ENV SOLR_VERSION 5.3.1
ENV SOLR_SHA256 34ddcac071226acd6974a392af7671f687990aa1f9eb4b181d533ca6dca6f42d

RUN mkdir -p /opt/solr && \
  wget -nv --output-document=/opt/solr.tgz http://archive.apache.org/dist/lucene/solr/$SOLR_VERSION/solr-$SOLR_VERSION.tgz && \
  wget -nv --output-document=/opt/solr.tgz.asc http://archive.apache.org/dist/lucene/solr/$SOLR_VERSION/solr-$SOLR_VERSION.tgz.asc && \
  gpg --verify /opt/solr.tgz.asc && \
  echo "$SOLR_SHA256 */opt/solr.tgz" | sha256sum -c - && \
  tar -C /opt/solr --extract --file /opt/solr.tgz --strip-components=1 && \
  rm /opt/solr.tgz* && \
  mkdir -p /opt/solr/server/solr/lib && \
  chown -R $SOLR_USER:$SOLR_USER /opt/solr

# https://issues.apache.org/jira/browse/SOLR-8107
RUN sed --in-place -e 's/^    "$JAVA" "${SOLR_START_OPTS\[@\]}" $SOLR_ADDL_ARGS -jar start.jar "${SOLR_JETTY_CONFIG\[@\]}"/    exec "$JAVA" "${SOLR_START_OPTS[@]}" $SOLR_ADDL_ARGS -jar start.jar "${SOLR_JETTY_CONFIG[@]}"/' /opt/solr/bin/solr

USER root
COPY docker-entrypoint.sh /solr-entrypoint.sh
RUN chmod +x /solr-entrypoint.sh

RUN rm -rf /opt/solr/docs
RUN rm -rf /usr/share/docs

EXPOSE 8983
WORKDIR /opt/solr
USER $SOLR_USER
CMD ["/opt/solr/bin/solr", "-f"]

