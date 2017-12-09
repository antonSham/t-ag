CREATE TABLE [dbo].[OrderParticipant]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [OrderId] INT NOT NULL, 
    [ParticipantId] INT NOT NULL, 
    CONSTRAINT [FK_OrderParticipant_ToOrder] FOREIGN KEY ([OrderId]) REFERENCES [Order]([Id]), 
    CONSTRAINT [FK_OrderParticipant_ToParticipant] FOREIGN KEY ([ParticipantId]) REFERENCES [Participant]([Id])
)
