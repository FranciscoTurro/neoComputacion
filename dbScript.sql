USE [neoCompDB]
GO

/****** Object: Table [dbo].[Category] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Category](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [name] [varchar](100) NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
    (
        [id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Inserting Categories
INSERT INTO [dbo].[Category] ([name])
VALUES ('programming'), ('web'), ('mobile'), ('PC'), ('AI'), ('security'), ('review')

GO

/****** Object: Table [dbo].[Post] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Post](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [title] [varchar](255) NOT NULL,
    [image] [varchar](max) NOT NULL,
    [content] [text] NOT NULL,
    [creation_date] [datetime] NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
    (
        [id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object: Table [dbo].[PostCategory] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PostCategory](
    [postId] [int] NOT NULL,
    [categoryId] [int] NOT NULL,
    CONSTRAINT [PK_PostCategory] PRIMARY KEY CLUSTERED 
    (
        [postId] ASC,
        [categoryId] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PostCategory] WITH CHECK 
    ADD CONSTRAINT [FK_PostCategory_Category] FOREIGN KEY([categoryId])
    REFERENCES [dbo].[Category] ([id])

GO

ALTER TABLE [dbo].[PostCategory] CHECK CONSTRAINT [FK_PostCategory_Category]

GO

ALTER TABLE [dbo].[PostCategory] WITH CHECK 
    ADD CONSTRAINT [FK_PostCategory_Post] FOREIGN KEY([postId])
    REFERENCES [dbo].[Post] ([id])

GO

USE [master]
GO
ALTER DATABASE [neoCompDB] SET READ_WRITE
GO
