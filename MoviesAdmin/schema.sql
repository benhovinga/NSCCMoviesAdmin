CREATE TABLE [AudienceRating] (
    [AudienceRatingId] int NOT NULL IDENTITY,
    [Title] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_AudienceRating] PRIMARY KEY ([AudienceRatingId])
);
GO


CREATE TABLE [Genre] (
    [GenreId] int NOT NULL IDENTITY,
    [Title] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Genre] PRIMARY KEY ([GenreId])
);
GO


CREATE TABLE [UserSnapshot] (
    [UserId] nvarchar(50) NOT NULL,
    [Email] nvarchar(150) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    CONSTRAINT [PK_UserSnapshot] PRIMARY KEY ([UserId])
);
GO


CREATE TABLE [Movie] (
    [MovieId] int NOT NULL IDENTITY,
    [Title] nvarchar(150) NOT NULL,
    [Synopsis] nvarchar(max) NOT NULL,
    [GenreId] int NOT NULL,
    [AudienceRatingId] int NOT NULL,
    [Runtime] int NOT NULL,
    [ReleaseDate] date NOT NULL,
    [Director] nvarchar(max) NOT NULL,
    [Producer] nvarchar(max) NOT NULL,
    [Distributor] nvarchar(max) NOT NULL,
    [OriginalLanguage] nvarchar(max) NOT NULL,
    [PosterUrl] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Movie] PRIMARY KEY ([MovieId]),
    CONSTRAINT [FK_Movie_AudienceRating_AudienceRatingId] FOREIGN KEY ([AudienceRatingId]) REFERENCES [AudienceRating] ([AudienceRatingId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Movie_Genre_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [Genre] ([GenreId]) ON DELETE CASCADE
);
GO


CREATE TABLE [CriticReview] (
    [CriticReviewId] int NOT NULL IDENTITY,
    [MovieId] int NOT NULL,
    [Review] nvarchar(1000) NOT NULL,
    [Rating] int NOT NULL,
    [IsPublished] bit NOT NULL,
    [CreatedById] nvarchar(max) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_CriticReview] PRIMARY KEY ([CriticReviewId]),
    CONSTRAINT [FK_CriticReview_Movie_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movie] ([MovieId]) ON DELETE CASCADE
);
GO


CREATE UNIQUE INDEX [IX_AudienceRating_Title] ON [AudienceRating] ([Title]);
GO


CREATE INDEX [IX_CriticReview_MovieId] ON [CriticReview] ([MovieId]);
GO


CREATE UNIQUE INDEX [IX_Genre_Title] ON [Genre] ([Title]);
GO


CREATE INDEX [IX_Movie_AudienceRatingId] ON [Movie] ([AudienceRatingId]);
GO


CREATE INDEX [IX_Movie_GenreId] ON [Movie] ([GenreId]);
GO


