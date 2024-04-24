CREATE TABLE [dbo].[Notes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](4000) NULL,
	[Desc] [varchar](max) NULL,
	[FilePath] [varchar](max) NULL,
	[Status] [bit] NULL,
	[CreatedBy] [varchar](500) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [varchar](500) NULL,
	[UpdatedDate] [datetime2](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


