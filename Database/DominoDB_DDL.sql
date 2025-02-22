USE [DOMINO_DB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Dmn_Auth_D](
	[AuthID] [nvarchar](20) NOT NULL,
	[MenuID] [varchar](20) NOT NULL,
	[MenuAuth] [bigint] NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_Auth_D] PRIMARY KEY CLUSTERED 
(
	[AuthID] ASC,
	[MenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_Auth_M](
	[AuthID] [nvarchar](20) NOT NULL,
	[AuthName] [nvarchar](100) NOT NULL,
	[UseYN] [char](1) NOT NULL,
	[SeqNum] [int] NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_Auth_M] PRIMARY KEY CLUSTERED 
(
	[AuthID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_CommonCode_D](
	[CDCode] [varchar](30) NOT NULL,
	[CDCode_Dtl] [nvarchar](50) NOT NULL,
	[CDCode_Name] [nvarchar](200) NOT NULL,
	[UseYN] [char](1) NOT NULL,
	[SeqNum] [int] NULL,
	[Code_Value1] [nvarchar](200) NULL,
	[Code_Value2] [nvarchar](200) NULL,
	[Code_value3] [nvarchar](200) NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateUser] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_CommonCode_M](
	[CDCode] [varchar](30) NOT NULL,
	[CDName] [nvarchar](200) NOT NULL,
	[UseYN] [char](1) NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateUser] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

INSERT [Domino_DB].[dbo].[Dmn_CommonCode_M] ([CDCode], [CDName], [UseYN], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'101', N'포장타입', N'Y', GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_M] ([CDCode], [CDName], [UseYN], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'102', N'서버타입', N'Y', GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_M] ([CDCode], [CDName], [UseYN], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'시리얼고정생성키', N'Y', GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_M] ([CDCode], [CDName], [UseYN], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'190', N'시리얼타입', N'Y', GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_M] ([CDCode], [CDName], [UseYN], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'191', N'리소스타입', N'Y', GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_M] ([CDCode], [CDName], [UseYN], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'제품상태', N'Y', GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_M] ([CDCode], [CDName], [UseYN], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'193', N'사용유무', N'Y', GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_M] ([CDCode], [CDName], [UseYN], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'201', N'작업지시상태', N'Y', GETDATE(), N'System', NULL, NULL);

INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'101', N'001', N'EA', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'101', N'002', N'BX1', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'101', N'003', N'BX2', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'101', N'004', N'BX3', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'101', N'005', N'BX4', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'101', N'006', N'BX5', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'101', N'007', N'BX6', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'101', N'008', N'BX7', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'101', N'009', N'BX8', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'101', N'010', N'BX9', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'102', N'001', N'DSM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'102', N'002', N'STANDALONE', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'102', N'003', N'Kiedas', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'102', N'004', N'ERP', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'102', N'005', N'Movilitas', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);


INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'1', N'ProductCode', N'Y', NULL, NULL, NULL,  GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'2', N'Lot', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'3', N'LineNo', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'4', N'FIX', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'5', N'FIX_ORDER', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);

INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'101', N'MFD_YY', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'102', N'MFD_YYMM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'103', N'MFD_YYMMDD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'104', N'MFD_YYYY', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'105', N'MFD_YYYYMM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'106', N'MFD_YYYYMMDD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'107', N'MFD_MM', N'Y', NULL, NULL, NULL,  GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'108', N'MFD_MMDD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'109', N'MFD_DD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'201', N'EXP_YY', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'202', N'EXP_YYMM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'203', N'EXP_YYMMDD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'204', N'EXP_YYYY', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'205', N'EXP_YYYYMM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'206', N'EXP_YYYYMMDD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'207', N'EXP_MM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'208', N'EXP_MMDD', N'Y', NULL, NULL, NULL,  GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'209', N'EXP_DD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'901', N'LEGACY', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'902', N'Pd_YY', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'903', N'Pd_YYMM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'904', N'Pd_YYMMDD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'905', N'Pd_YYYY', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'906', N'Pd_YYYYMM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'907', N'Pd_YYYYMMDD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'908', N'Order', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'909', N'Exp_YYMMDD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'910', N'LineNo', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'911', N'FIX', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'912', N'ORDER_YY', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'913', N'ORDER_YYMM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'914', N'ORDER_YYMMDD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'915', N'ORDER_YYYY', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'916', N'ORDER_YYYYMM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'917', N'ORDER_YYYYMMDD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'918', N'ORDER_M_HEXA', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'919', N'ORDER_DD', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'920', N'ORDER_Seq', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'921', N'ORDER_YYYY_Code', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'922', N'Packing_Code', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'923', N'Packing_Code_2', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'924', N'Sub_Lot', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'925', N'ERP_ORDER_NO', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'926', N'GTIN_3', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'103', N'927', N'MFD_YY', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);


--시리얼타입
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'190', N'N', N'None', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'190', N'R', N'ReceivedSerialNumber', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'190', N'S', N'ScanSerialNumber', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'190', N'C', N'CreateSerialNumber', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'190', N'D', N'DSM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'190', N'M', N'Movilitas', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'190', N'I', N'Interface', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'190', N'H', N'HelpCode', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);

--리소스 타입
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'191', N'L', N'LocalCreate', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'191', N'D', N'DSM', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'191', N'F', N'File', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'191', N'M', N'Movilitas', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'191', N'E', N'ERP', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);

--제품 상태
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'PA', N'Pass', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'RE', N'Reject', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'NU', N'Notused', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'OW', N'OverWeight', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'UW', N'UnderWeight', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'SN', N'Sample_Normal', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'ST', N'Sample_Test', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'SS', N'Sample_Storage', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'SC', N'Sample_QC', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'SA', N'Sample_QA', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'NF', N'NotForSale', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'DE', N'Destroy', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'192', N'CC', N'Cancel', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);

--사용 유무
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'193', N'Y', N'Yes', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'193', N'N', N'No', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);

--작업지시 상태
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'201', N'NM', N'Normal', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'201', N'TS', N'Test', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'201', N'MA', N'Manual', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'201', N'TP', N'TestPrint', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'201', N'MP', N'ManualPrint', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);
INSERT [Domino_DB].[dbo].[Dmn_CommonCode_D] ([CDCode], [CDCode_Dtl], [CDCode_Name], [UseYN], [Code_Value1], [Code_Value2], [Code_Value3], [InsertDate], [InsertUser], [UpdateDate], [UpdateUser]) VALUES (N'201', N'RE', N'Repack', N'Y', NULL, NULL, NULL, GETDATE(), N'System', NULL, NULL);


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_JobOrder_AG](
	[OrderNo] [varchar](50) NOT NULL,
	[SeqNo] [varchar](4) NOT NULL,
	[JobDetailType] [varchar](3) NOT NULL,
	[JobStatus] [char](2) NOT NULL,
	[Cnt_Good] [int] NULL,
	[Cnt_Error] [int] NULL,
	[Cnt_Sample] [int] NULL,
	[Cnt_Destroy] [int] NULL,
	[Cnt_Child] [int] NULL,
	[Cnt_Work] [int] NULL,
	[Cnt_Parent] [int] NULL,
	[Cnt_SNLast] [int] NULL,
	[Cnt_SNPrintLast] [int] NULL,
	[Cnt_SN_Movil] [int] NULL,
	[Cnt_SN_DSM] [int] NULL,
	[Cnt_SN_Lot_O] [int] NULL,
	[Cnt_SN_Lot_X] [int] NULL,
	[Cnt_Status1] [int] NULL,
	[Cnt_Status2] [int] NULL,
	[Cnt_Status3] [int] NULL,
	[Cnt_Status4] [int] NULL,
	[Cnt_Status5] [int] NULL,
	[UserDefineData1] [nvarchar](50) NULL,
	[UserDefineData2] [nvarchar](50) NULL,
	[PrinterVariable1] [nvarchar](100) NULL,
	[PrinterVariable2] [nvarchar](100) NULL,
	[PrinterVariable3] [nvarchar](100) NULL,
	[PrinterVariable4] [nvarchar](100) NULL,
	[PrinterVariable5] [nvarchar](100) NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[StartDate] [datetime] NULL,
	[StartUser] [nvarchar](50) NULL,
	[CompleteDate] [datetime] NULL,
	[CompleteUser] [nvarchar](50) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_JobOrder_AG] PRIMARY KEY CLUSTERED 
(
	[OrderNo] ASC,
	[SeqNo] ASC,
	[JobDetailType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dmn_JobOrder_M]    Script Date: 1/22/2021 3:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_JobOrder_M](
	[OrderNo] [varchar](50) NOT NULL,
	[SeqNo] [varchar](4) NOT NULL,
	[LineID] [nvarchar](20) NULL,
	[CorCode] [nvarchar](20) NULL,
	[PlantCode] [nvarchar](20) NULL,
	[ErpOrderNo] [nvarchar](50) NULL,
	[OrderType] [varchar](2) NOT NULL,
	[MfdDate] [datetime] NULL,
	[ExpDate] [datetime] NULL,
	[ProdCode] [nvarchar](50) NOT NULL,
	[LotNo] [varchar](20) NOT NULL,
	[LotNo_Sub] [varchar](20) NULL,
	[UseYN] [char](1) NOT NULL,
	[Cnt_JobPlan] [int] NOT NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[AssignUser] [nvarchar](50) NULL,
	[AssignDate] [datetime] NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_JobOrder_M] PRIMARY KEY CLUSTERED 
(
	[OrderNo] ASC,
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dmn_JobOrder_PM]    Script Date: 1/22/2021 3:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_JobOrder_PM](
	[OrderNo] [varchar](50) NOT NULL,
	[SeqNo] [varchar](4) NOT NULL,
	[JobDetailType] [varchar](3) NOT NULL,
	[JobStatus] [char](2) NOT NULL,
	[Cnt_Good] [int] NULL,
	[Cnt_Error] [int] NULL,
	[Cnt_Sample] [int] NULL,
	[Cnt_Destroy] [int] NULL,
	[Cnt_SNLast] [int] NULL,
	[Cnt_SNPrintLast] [int] NULL,
	[Cnt_SN_Movil] [int] NULL,
	[Cnt_SN_DSM] [int] NULL,
	[Cnt_SN_Lot_O] [int] NULL,
	[Cnt_SN_Lot_X] [int] NULL,
	[Cnt_Status1] [int] NULL,
	[Cnt_Status2] [int] NULL,
	[Cnt_Status3] [int] NULL,
	[Cnt_Status4] [int] NULL,
	[Cnt_Status5] [int] NULL,
	[UserDefineData1] [nvarchar](50) NULL,
	[UserDefineData2] [nvarchar](50) NULL,
	[PrinterVariable1] [nvarchar](100) NULL,
	[PrinterVariable2] [nvarchar](100) NULL,
	[PrinterVariable3] [nvarchar](100) NULL,
	[PrinterVariable4] [nvarchar](100) NULL,
	[PrinterVariable5] [nvarchar](100) NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[StartDate] [datetime] NULL,
	[StartUser] [nvarchar](50) NULL,
	[CompleteDate] [datetime] NULL,
	[CompleteUser] [nvarchar](50) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_JobOrder_PM] PRIMARY KEY CLUSTERED 
(
	[OrderNo] ASC,
	[SeqNo] ASC,
	[JobDetailType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dmn_Product_AG]    Script Date: 1/22/2021 3:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_Product_AG](
	[ProdCode] [varchar](50) NOT NULL,
	[JobDetailType] [varchar](3) NOT NULL,
	[ResourceType] [char](1) NOT NULL,
	[BarcodeType] [varchar](20) NULL,
	[BarcodeDataFormat] [varchar](20) NULL,
	[SnType] [char](1) NULL,
	[SnExpressionID] [nvarchar](20) NULL,
	[DesignID] [nvarchar](50) NULL,
	[ContentCount] [int] NULL,
	[PackingCount] [int] NULL,
	[Prefix_SSCC] [varchar](20) NULL,
	[PrinterName] [nvarchar](50) NULL,
	[Capacity] [nvarchar](20) NULL,
	[LabelPrintCount] [int] NULL,
	[GS1ExtensionCode] [char](1) NULL,
	[ProdStdCodeChild] [varchar](30) NULL,
	[Condition] [nvarchar](20) NULL,
	[MachineUseYN] [char](1) NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_Product_AG] PRIMARY KEY CLUSTERED 
(
	[ProdCode] ASC,
	[JobDetailType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dmn_Product_M]    Script Date: 1/22/2021 3:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_Product_M](
	[ProdCode] [nvarchar](50) NOT NULL,
	[ProdStdCode] [varchar](30) NOT NULL,
	[ProdName] [nvarchar](200) NOT NULL,
	[ProdName2] [nvarchar](200) NULL,
	[AGLevel] [int] NULL,
	[Remark] [nvarchar](200) NULL,
	[Exp_Day] [int] NULL,
	[UseYN] [char](1) NOT NULL,
	[Delay_Print] [int] NULL,
	[Delay_Shot1] [int] NULL,
	[Delay_Shot2] [int] NULL,
	[Delay_NG] [int] NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_Product_M] PRIMARY KEY CLUSTERED 
(
	[ProdCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dmn_Product_PM]    Script Date: 1/22/2021 3:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_Product_PM](
	[ProdCode] [nvarchar](50) NOT NULL,
	[JobDetailType] [varchar](3) NOT NULL,
	[ResourceType] [char](1) NOT NULL,
	[BarcodeType] [varchar](20) NULL,
	[BarcodeDataFormat] [varchar](20) NULL,
	[SnType] [char](1) NULL,
	[SnExpressionID] [nvarchar](20) NULL,
	[DesignID] [nvarchar](50) NULL,
	[Capacity] [nvarchar](20) NULL,
	[LIC] [nvarchar](20) NULL,
	[PCN] [nvarchar](20) NULL,
	[Condition] [nvarchar](20) NULL,
	[MachineUseYN] [char](1) NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_Product_PM] PRIMARY KEY CLUSTERED 
(
	[ProdCode] ASC,
	[JobDetailType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dmn_ReadBarcode]    Script Date: 1/22/2021 3:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_ReadBarcode](
	[ProdStdCode] [varchar](30) NOT NULL,
	[SerialNum] [varchar](50) NOT NULL,
	[OrderNo] [varchar](50) NOT NULL,
	[SeqNo] [varchar](4) NOT NULL,
	[JobDetailType] [varchar](3) NULL,
	[FullBarcode_Read] [nvarchar](300) NULL,
	[AI_FullBarcode_Read] [varchar](300) NULL,
	[FullBarcode_Parent] [nvarchar](300) NULL,
	[AI_FullBarcode_Parent] [varchar](300) NULL,
	[ParentProdStdCode] [varchar](30) NULL,
	[ParentSerialNum] [varchar](20) NULL,
	[Status] [varchar](2) NOT NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[FilePath] [nvarchar](512) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_ReadBarcode] PRIMARY KEY CLUSTERED 
(
	[ProdStdCode] ASC,
	[SerialNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dmn_Serial_Expression]    Script Date: 1/22/2021 3:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_Serial_Expression](
	[SnExpressionID] [nvarchar](20) NOT NULL,
	[SnExpressionStr] [varchar](300) NOT NULL,
	[UseYN] [char](1) NULL,
	[SeqNum] [int] NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_SerialExpression] PRIMARY KEY CLUSTERED 
(
	[SnExpressionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dmn_SerialPool]    Script Date: 1/22/2021 3:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_SerialPool](
	[ProdStdCode] [varchar](30) NOT NULL,
	[SerialNum] [varchar](50) NOT NULL,
	[JobDetailType] [varchar](3) NOT NULL,
	[OrderNo] [varchar](50) NULL,
	[SeqNo] [varchar](4) NULL,
	[ResourceType] [char](1) NOT NULL,
	[BarcodeDataFormat] [varchar](20) NULL,
	[BarcodeType] [varchar](20) NULL,
	[SerialType] [char](1) NOT NULL,
	[Idx_Group] [bigint] NULL,
	[Idx_Insert] [bigint] NULL,
	[UseDate] [datetime] NULL,
	[InspectedDate] [datetime] NULL,
	[UseYN] [char](1) NOT NULL,
	[Status] [varchar](2) NULL,
	[FileName] [nvarchar](512) NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
	[AssignDate] [datetime] NULL,
 CONSTRAINT [PK_Dmn_SerialPool] PRIMARY KEY CLUSTERED 
(
	[ProdStdCode] ASC,
	[SerialNum] ASC,
	[JobDetailType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dmn_User]    Script Date: 1/22/2021 3:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_User](
	[UserID] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[UserPW] [nvarchar](200) NOT NULL,
	[LastLogIn] [datetime] NULL,
	[FailLogInCount] [int] NOT NULL,
	[LockYN] [char](1) NOT NULL,
	[UseYN] [char](1) NOT NULL,
	[PrePW1] [nvarchar](200) NULL,
	[PrePW2] [nvarchar](200) NULL,
	[PrePW3] [nvarchar](200) NULL,
	[LastPWUpdate] [datetime] NULL,
	[AuthID] [nvarchar](20) NULL,
	[DeptCode] [nvarchar](20) NULL,
	[Email] [varchar](100) NULL,
	[PhoneNum] [varchar](20) NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dmn_VisionResult]    Script Date: 1/22/2021 3:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dmn_VisionResult](
	[OrderNo] [varchar](50) NOT NULL,
	[SeqNo] [varchar](4) NOT NULL,
	[JobDetailType] [varchar](3) NOT NULL,
	[DecodedBarcode] [nvarchar](300) NOT NULL,
	[Idx_Insert] [bigint] IDENTITY(1,1) NOT NULL,
	[Read_OCR] [nvarchar](512) NULL,
	[Grade_Barcode] [char](1) NULL,
	[FilePath] [nvarchar](512) NULL,
	[CameraIndex] [int] NULL,
	[Status] [varchar](2) NULL,
	[Reserved1] [nvarchar](90) NULL,
	[Reserved2] [nvarchar](90) NULL,
	[Reserved3] [nvarchar](90) NULL,
	[Reserved4] [nvarchar](90) NULL,
	[Reserved5] [nvarchar](90) NULL,
	[InsertDate] [datetime] NOT NULL,
	[InsertUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dmn_VisionResult] PRIMARY KEY CLUSTERED 
(
	[OrderNo] ASC,
	[SeqNo] ASC,
	[JobDetailType] ASC,
	[DecodedBarcode] ASC,
	[Idx_Insert] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

