CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    passwordhash VARCHAR(255) NOT NULL,
    rolecode VARCHAR(100) not null
);

create table organizations(
	Id SERIAL primary key,
	Name VARCHAR(100) NOT null
);


CREATE table departments (
    Id SERIAL PRIMARY KEY,
    OrgId INT,
    Name VARCHAR(100) NOT null,
    FOREIGN KEY (OrgId) REFERENCES organizations(Id) on delete set null
);

CREATE table positions (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

CREATE TABLE employees (
    Id SERIAL PRIMARY KEY,
    UserId INT,
    Surname VARCHAR(100) NOT NULL,
    MiddleName VARCHAR(100) NOT NULL,
    GivenName VARCHAR(100) NOT NULL,
    DeptId INT,
    PosId INT,
    FOREIGN KEY (UserId) REFERENCES Users(Id) on delete set null,
    FOREIGN KEY (PosId) REFERENCES positions(Id) on delete set null,
    FOREIGN KEY (DeptId) REFERENCES Departments(Id) on delete cascade
);


CREATE TABLE hierarchy (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    type VARCHAR(50) not null,
    ParentId INT,
    DeptId INT NOT NULL,
    FOREIGN KEY (ParentId) REFERENCES hierarchy(Id) ON DELETE CASCADE,
    FOREIGN KEY (DeptId) REFERENCES Departments(Id) ON DELETE CASCADE
);


