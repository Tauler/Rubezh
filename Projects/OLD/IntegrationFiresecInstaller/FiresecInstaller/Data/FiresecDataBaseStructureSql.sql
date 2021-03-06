USE [Firesec]
GO
/****** Объект:  Table [dbo].[Journal]    Дата сценария: 09/14/2011 16:57:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Journal](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceTime] [datetime] NULL,
	[SystemTime] [datetime] NULL,
	[ZoneName] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[DeviceName] [nvarchar](max) NULL,
	[PanelName] [nvarchar](max) NULL,
	[DeviceDatabaseId] [nvarchar](max) NULL,
	[PanelDatabaseId] [nvarchar](max) NULL,
	[UserName] [nvarchar](max) NULL,
	[Detalization] [nvarchar](max) NULL,
	[StateType] [int] NULL,
	[SubSystemType] [int] NULL,
 CONSTRAINT [PK_Journal] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
