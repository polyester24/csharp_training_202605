TRUNCATE TABLE
    employee,
    department
RESTART IDENTITY CASCADE;

INSERT INTO department (name) VALUES ('総務部');
INSERT INTO department (name) VALUES ('経理部');
INSERT INTO department (name) VALUES ('人事部');
INSERT INTO department (name) VALUES ('開発部');
INSERT INTO department (name) VALUES ('営業部');

INSERT INTO employee (name, dept_id, email)
VALUES ('田中太郎', 2, 'tanakatarou@csharp.com');

INSERT INTO employee (name, dept_id, email)
VALUES ('鈴木三郎', 1, 'suzukisaburou@csharp.com');

INSERT INTO employee (name, dept_id, email)
VALUES ('佐藤花子', 4, 'sastouhanako@csharp.com');

INSERT INTO employee (name, dept_id, email)
VALUES ('中田彩子', 5, 'nakataayako@csharp.com');

INSERT INTO employee (name, dept_id, email)
VALUES ('加藤圭太', 3, 'katoukeita@csharp.com');

INSERT INTO employee (name, dept_id, email)
VALUES ('松本良太', 4, 'matumotoryouta@csharp.com');

INSERT INTO employee (name, dept_id, email)
VALUES ('山下孝輔', 5, 'yamasitakousuke@csharp.com');

INSERT INTO employee (name, dept_id, email)
VALUES ('渡辺大輔', 4, 'watanabedaisuke@csharp.com');