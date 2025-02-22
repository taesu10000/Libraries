USE [Domino_AuditTrail]
IF OBJECT_ID (N'__MigrationHistory', N'U') IS NOT NULL
BEGIN
 DROP TABLE __MigrationHistory
END 

IF COL_LENGTH('Dmn_Log_DSM', 'ErrorCode') IS NULL
BEGIN
    ALTER TABLE Dmn_Log_DSM ADD ErrorCode nvarchar(50) null
END
GO
USE [DSM]
IF OBJECT_ID (N'__MigrationHistory', N'U') IS NOT NULL
BEGIN
 DROP TABLE __MigrationHistory
END 
GO
IF COL_LENGTH('Dmn_SerialPool', 'AssignDate') IS NULL
BEGIN
    ALTER TABLE Dmn_SerialPool ADD AssignDate datetime null
END
IF COL_LENGTH('Dmn_Product_D', 'Price') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_D ADD Price nvarchar(10) null
END
GO
IF COL_LENGTH('Dmn_Product_M', 'MedicineType') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_M ADD MedicineType int null
END
IF COL_LENGTH('Dmn_Product_M', 'CompanyPrefix') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_M ADD CompanyPrefix varchar(20) null
END
GO
IF COL_LENGTH('Dmn_Product_D', 'ProdStdCode') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_D ADD ProdStdCode nvarchar(30) null
END
GO
IF COL_LENGTH('Dmn_Product_M', 'NDCType') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_M ADD NDCType nvarchar(20) null
END
GO
IF COL_LENGTH('Dmn_Product_M', 'NDCValue') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_M ADD NDCValue nvarchar(50) null
END
GO
IF COL_LENGTH('Dmn_Product_D', 'MinimumWeight') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_D ADD MinimumWeight decimal(13,3) null
END
GO
IF COL_LENGTH('Dmn_Product_D', 'MaximumWeight') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_D ADD MaximumWeight decimal(13,3) null
END
GO

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE 
        TABLE_NAME = 'Dmn_Serialpool' AND
        TABLE_SCHEMA = 'dbo' AND
        COLUMN_NAME = 'Idx_Group' COLLATE Latin1_General_BIN
)
BEGIN
	EXEC sp_rename 'DSM.dbo.Dmn_Serialpool.Idx_Group', 'idx_Group', 'COLUMN';
END
IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE 
        TABLE_NAME = 'Dmn_Serialpool' AND
        TABLE_SCHEMA = 'dbo' AND
        COLUMN_NAME = 'Idx_Insert' COLLATE Latin1_General_BIN
)
BEGIN
	EXEC sp_rename 'DSM.dbo.Dmn_Serialpool.Idx_Insert', 'idx_Insert', 'COLUMN';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Dmn_CustomBarcodeFormat]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Dmn_CustomBarcodeFormat](
          [PlantCode] [nvarchar](20) NOT NULL,
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
        [PlantCode] ASC,
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
IF COL_LENGTH('Dmn_Product_M', 'InterfaceDetail') IS NULL
BEGIN
    ALTER TABLE Dmn_Product_M ADD InterfaceDetail nvarchar(50) null
END
GO
