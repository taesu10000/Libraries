USE [Domino_AuditTrail]
IF OBJECT_ID (N'__MigrationHistory', N'U') IS NOT NULL
BEGIN
 DROP TABLE __MigrationHistory
END 

IF COL_LENGTH('Dmn_Log', 'ErrorCode') IS NULL
BEGIN
    ALTER TABLE Dmn_Log ADD ErrorCode nvarchar(50) null
END
GO
USE [Domino_DB]
IF OBJECT_ID (N'__MigrationHistory', N'U') IS NOT NULL
BEGIN
 DROP TABLE __MigrationHistory
END 

IF COL_LENGTH('Dmn_Product_M', 'Delay_Print2') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_M ADD Delay_Print2 int null
END
GO

IF COL_LENGTH('Dmn_Product_M', 'Delay_NG2') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_M ADD Delay_NG2 int null
END
GO

IF COL_LENGTH('Dmn_Product_M', 'Delay_Shot3') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_M ADD Delay_Shot3 int null
END
GO

IF COL_LENGTH('Dmn_Product_M', 'Delay_Shot4') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_M ADD Delay_Shot4 int null
END
GO

IF COL_LENGTH('Dmn_Product_PM', 'DesignID2') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_PM ADD DesignID2 nvarchar(50) null
END
GO

IF COL_LENGTH('Dmn_Product_AG', 'DesignID2') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_AG ADD DesignID2 nvarchar(50) null
END
GO

IF COL_LENGTH('Dmn_Product_AG', 'PrinterName2') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_AG ADD PrinterName2 nvarchar(50) null
END
GO

IF COL_LENGTH('Dmn_Product_PM', 'UsePrinterGroup1') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_PM ADD UsePrinterGroup1 bit null
END
GO

IF COL_LENGTH('Dmn_Product_PM', 'UsePrinterGroup2') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_PM ADD UsePrinterGroup2 bit null
END
GO

IF COL_LENGTH('Dmn_Product_PM', 'DesignID') < 100 --nvarchar은 1글자당 length 2
BEGIN
    ALTER TABLE Dmn_Product_PM ALTER COLUMN DesignID nvarchar(50) null
END
GO

IF COL_LENGTH('Dmn_Product_PM', 'DesignID2') < 100
BEGIN
    ALTER TABLE Dmn_Product_PM ALTER COLUMN DesignID2 nvarchar(50) null
END
GO

IF COL_LENGTH('Dmn_Product_AG', 'DesignID2') < 100
BEGIN
    ALTER TABLE Dmn_Product_AG ALTER COLUMN DesignID2 nvarchar(50) null
END
GO

IF COL_LENGTH('Dmn_SerialPool', 'ConfirmedYN') IS NULL
BEGIN
    ALTER TABLE Dmn_SerialPool ADD ConfirmedYN nvarchar(1) null
END
GO

IF OBJECT_ID (N'Dmn_HelpCodePool_M', N'U') IS NULL 
BEGIN
	CREATE TABLE [dbo].[Dmn_HelpCodePool_M](
		[OrderNo] NVARCHAR(50) NOT NULL,
		[SeqNo] NVARCHAR(4) NOT NULL,
		[HelpCode] NVARCHAR(20) NOT NULL,
		[ProdStdCode] NVARCHAR(14) NULL,
		[SerialNum] NVARCHAR(20) NULL,
		[UseYN] NVARCHAR(1) NOT NULL,
		[Status] NVARCHAR(2) NOT NULL,
		[idx_Insert] BIGINT NULL,
		[FilePath] NVARCHAR(512) NULL,
		[InsertUser] NVARCHAR(50) NOT NULL,
		[InsertDate] DATETIME NULL,
		[UpdateUser] NVARCHAR(50) NULL,
		[UpdateDate] DATETIME NULL,
	 CONSTRAINT [PK_dbo.Dmn_HelpCodePool_M] PRIMARY KEY CLUSTERED 
	(
		[OrderNo] ASC,
		[SeqNo] ASC,
		[HelpCode] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF OBJECT_ID (N'Dmn_HelpCodePool_D', N'U') IS NULL 
BEGIN
	CREATE TABLE [dbo].[Dmn_HelpCodePool_D](
		[ChildProdStdCode] NVARCHAR(14) NOT NULL,
		[ChildSerialNum] NVARCHAR(20) NOT NULL,
		[HelpCode] NVARCHAR(20) NOT NULL,
		[FullChildBarcode_Read] NVARCHAR(300) NOT NULL,
		[OrderNo] NVARCHAR(50) NOT NULL,
		[SeqNo] NVARCHAR(4) NOT NULL,
		[History] NVARCHAR(MAX) NULL,
		[InsertUser] NVARCHAR(50) NOT NULL,
		[InsertDate] DATETIME NULL,
		[UpdateUser] NVARCHAR(50) NULL,
		[UpdateDate] DATETIME NULL,
	 CONSTRAINT [PK_dbo.Dmn_HelpCodePool_D] PRIMARY KEY CLUSTERED 
	(
		[ChildProdStdCode] ASC,
		[ChildSerialNum] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF COL_LENGTH('Dmn_Product_AG', 'Price') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_AG ADD Price nvarchar(10) null
END
GO

IF COL_LENGTH('Dmn_Product_PM', 'Price') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_PM ADD Price nvarchar(10) null
END
GO

IF COL_LENGTH('Dmn_Product_PM', 'PharmaCode') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_PM ADD PharmaCode nvarchar(50) null
END
GO

IF COL_LENGTH('Dmn_VisionResult', 'PharmaCode') IS NULL
BEGIN
    ALTER TABLE Dmn_VisionResult ADD PharmaCode nvarchar(50) null
END
GO

IF COL_LENGTH('Dmn_Product_AG', 'ProdStdCode') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_AG ADD ProdStdCode nvarchar(30) null
END
GO

IF COL_LENGTH('Dmn_Product_M', 'MedicineType') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_M ADD MedicineType int null
END
GO

IF COL_LENGTH('Dmn_Product_PM', 'ExtractSerialStart') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_PM ADD ExtractSerialStart int null
END
GO

IF COL_LENGTH('Dmn_Product_PM', 'ExtractSerialEnd') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_PM ADD ExtractSerialEnd int null
END
GO

IF COL_LENGTH('Dmn_HelpCodePool_M', 'SerialNum') < 180
BEGIN
    ALTER TABLE Dmn_HelpCodePool_M ALTER COLUMN SerialNum nvarchar(90) null
END
GO

IF COL_LENGTH('Dmn_VisionResult', 'DecodedBarcode') < 2000
BEGIN
    ALTER TABLE Dmn_VisionResult ALTER COLUMN DecodedBarcode nvarchar(1000) null
END
GO

IF COL_LENGTH('Dmn_VisionResult', 'Weight') IS NULL
BEGIN
    ALTER TABLE Dmn_VisionResult ADD Weight nvarchar(30) null
END
GO

IF COL_LENGTH('Dmn_Product_PM', 'MinimumWeight') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_PM ADD MinimumWeight decimal(13,3) null
END
GO

IF COL_LENGTH('Dmn_Product_PM', 'MaximumWeight') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_PM ADD MaximumWeight decimal(13,3) null
END
GO

IF COL_LENGTH('Dmn_Product_AG', 'MinimumWeight') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_AG ADD MinimumWeight decimal(13,3) null
END
GO

IF COL_LENGTH('Dmn_Product_AG', 'MaximumWeight') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_AG ADD MaximumWeight decimal(13,3) null
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='IX_ReadBarcode_ParentProdStdCode_ParentSerialNum' AND object_id = OBJECT_ID('dbo.Dmn_ReadBarcode'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ReadBarcode_ParentProdStdCode_ParentSerialNum]
    ON [dbo].[Dmn_ReadBarcode] ([ParentProdStdCode], [ParentSerialNum])
    INCLUDE ([ProdStdCode], [SerialNum], [OrderNo], [SeqNo], [JobDetailType], [FullBarcode_Read], [FullBarcode_Parent], [AI_FullBarcode_Read], [AI_FullBarcode_Parent], [Status], [Reserved1], [Reserved2], [Reserved3], [Reserved4], [Reserved5], [FilePath], [InsertUser], [InsertDate], [UpdateUser], [UpdateDate])
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='IX_OrderNo_SeqNo_JobDetailType_BarcodeDataFormat' AND object_id = OBJECT_ID('dbo.Dmn_SerialPool'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_OrderNo_SeqNo_JobDetailType_BarcodeDataFormat] ON [dbo].[Dmn_SerialPool]
(
	[OrderNo] ASC,
	[SeqNo] ASC,
	[JobDetailType] ASC,
	[BarcodeDataFormat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Dmn_CustomBarcodeFormat]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Dmn_CustomBarcodeFormat](
		  [CustomBarcodeFormatID] [nvarchar](20) NOT NULL,
		  [UseSeparatorYN] [nvarchar](1) NULL,
		  [Separator] [nvarchar](5) NULL,
		  [CustomBarcodeFormatStr] [nvarchar](300) NULL,
		  [Reserved1] [nvarchar](90) NULL,
		  [Reserved2] [nvarchar](90) NULL,
		  [Reserved3] [nvarchar](90) NULL,
		  [Reserved4] [nvarchar](90) NULL,
		  [Reserved5] [nvarchar](90) NULL,
		  [InsertUser] [nvarchar](50) NOT NULL,
		  [InsertDate] [datetime] NOT NULL,
		  [UpdateUser] [nvarchar](50) NULL,
	      [UpdateDate] [datetime] NULL,
     CONSTRAINT [PK_dbo.Dmn_CustomBarcodeFormat] PRIMARY KEY CLUSTERED 
    (
        [CustomBarcodeFormatID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF COL_LENGTH('Dmn_JobOrder_M', 'DateOfTest') IS NULL
BEGIN
    ALTER TABLE Dmn_JobOrder_M ADD DateOfTest datetime null
END
GO

IF COL_LENGTH('Dmn_SerialPool', 'ChildCount') IS NULL
BEGIN
    ALTER TABLE Dmn_SerialPool ADD ChildCount int null
END
GO