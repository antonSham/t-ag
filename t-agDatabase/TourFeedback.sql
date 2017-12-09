CREATE TABLE [dbo].[TourFeedback]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [TourId] INT NOT NULL, 
    [Feedback] TEXT NOT NULL, 
    CONSTRAINT [FK_TourFeedback_ToTour] FOREIGN KEY ([TourId]) REFERENCES [Tour]([Id]),
)
