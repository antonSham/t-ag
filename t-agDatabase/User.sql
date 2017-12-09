CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [type] VARCHAR(50) NOT NULL, 
    [loging] VARCHAR(50) NOT NULL, 
    [password] VARBINARY(50) NOT NULL
)
