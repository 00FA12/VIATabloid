DROP SCHEMA IF EXISTS viatabloid;

CREATE SCHEMA IF NOT EXISTS viatabloid;
SET SCHEMA 'viatabloid';
SET search_path = viatabloid;

CREATE TABLE tabloid(
    id SERIAL PRIMARY KEY
);

CREATE TABLE department(
    id SERIAL PRIMARY KEY,
    name VARCHAR(50),
    tabloidId INT REFERENCES tabloid(id)
);

CREATE TABLE story(
    id SERIAL PRIMARY KEY,
    title VARCHAR(100),
    body text,
    departmentId INT REFERENCES department(id)
);

INSERT INTO tabloid (id) VALUES (1);