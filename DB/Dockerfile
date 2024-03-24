FROM postgres:latest

# Environment variables
ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=password

COPY init.sql /docker-entrypoint-initdb.d/

EXPOSE 5432