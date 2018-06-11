use Production;

drop table admin_users;

CREATE TABLE admin_users
(
	id int NOT NULL PRIMARY KEY IDENTITY,
	username varchar(200),
	password varchar(200),
	lastlogin DateTime,
	previouslogin DateTime
);

INSERT INTO admin_users (username, password)
VALUES ('IiT8Q2xxIsQLIpmHuCPTEg==', 'Epz3BQ04poAopSXrc0XkoQ6zKBmJC3gcuZjCzXMYK6kn2BvTFgX+tqpyvfGJe/d2AWo8suYUqkxn6WAxZK9VP7cgcdY/Zpo1U50rn1QWULk=');