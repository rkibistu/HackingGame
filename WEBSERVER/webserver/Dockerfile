# Dockerfile for webserver
ARG WEBSERVER_VERSION

FROM python:${WEBSERVER_VERSION}

ARG WEBSERVER_PORT

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY . .

EXPOSE ${WEBSERVER_PORT}

CMD ["python", "/app/app.py"]
