USE [neoComp-db]
GO

/****** Object:  Table [dbo].[Post]    Script Date: 6/6/2023 12:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Post](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [title] [varchar](255) NOT NULL,
    [image] [varchar](255) NOT NULL,
    [content] [text] NOT NULL,
    CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
    (
        [id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [master]
GO
ALTER DATABASE [neoComp-db] SET READ_WRITE
GO
