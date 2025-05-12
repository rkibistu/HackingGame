ARG UBUNTU_VERSION

FROM ubuntu:${UBUNTU_VERSION}

ARG HTTP_PORT_DEFAULT
ARG HTTPS_PORT_DEFAULT
ARG HTTP_WEBSERVER_PORT
ARG WEBSERVER_NAME
ARG WEBSERVER_CONTAINER_NAME
ARG CERT_FILENAME
ARG CERT_KEY_FILENAME
ARG DEBIAN_FRONTEND=noninteractive
ARG WEBSERVER_PUBLIC_IP

RUN apt-get update && \
    apt-get install -y apache2 modsecurity-crs git libapache2-mod-security2 dos2unix gettext && rm -rf /var/lib/apt/lists/*


ENV HTTP_PORT_DEFAULT=${HTTP_PORT_DEFAULT} \
    HTTPS_PORT_DEFAULT=${HTTPS_PORT_DEFAULT} \
    HTTP_WEBSERVER_PORT=${HTTP_WEBSERVER_PORT} \
    WEBSERVER_NAME=${WEBSERVER_NAME} \
    WEBSERVER_CONTAINER_NAME=${WEBSERVER_CONTAINER_NAME} \
    CERT_FILENAME=${CERT_FILENAME} \
    CERT_KEY_FILENAME=${CERT_KEY_FILENAME} \
    WEBSERVER_PUBLIC_IP=${WEBSERVER_PUBLIC_IP} 

RUN a2enmod proxy proxy_http ssl rewrite

COPY ./config/cybergame.conf.template /tmp/cybergame.conf.template

RUN envsubst < /tmp/cybergame.conf.template > /etc/apache2/sites-available/000-default.conf

COPY script.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/script.sh

COPY ./certs/${CERT_FILENAME} /etc/ssl/certs/server.crt
COPY ./certs/${CERT_KEY_FILENAME} /etc/ssl/private/server.key

RUN echo "CERT_KEY_FILENAME = ${CERT_KEY_FILENAME}"

RUN dos2unix /usr/local/bin/script.sh

RUN /usr/local/bin/script.sh

RUN echo "ServerName localhost" >> /etc/apache2/apache2.conf


EXPOSE ${HTTP_PORT_DEFAULT} ${HTTPS_PORT_DEFAULT}

CMD ["apache2ctl", "-D", "FOREGROUND"]
