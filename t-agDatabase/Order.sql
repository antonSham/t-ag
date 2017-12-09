CREATE TABLE [dbo].[Order]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [TourId] INT NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [EmployeeId] INT NOT NULL, 
    [Amount] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Order_ToUser_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_Order_ToUser_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_Order_ToTour] FOREIGN KEY ([TourId]) REFERENCES [Tour]([Id])
)
