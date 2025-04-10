USE DapperDatabase;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Parent' AND xtype = 'U')
BEGIN
    CREATE TABLE Parent (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Child' AND xtype = 'U')
BEGIN
    CREATE TABLE Child (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(50) NOT NULL,
        ParentId INT FOREIGN KEY REFERENCES Parent(Id)
    );
END;
