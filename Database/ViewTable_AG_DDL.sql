USE [DOMINO_DB]
GO

/****** Object:  UserDefinedFunction [dbo].[fn_CommonDetailName]    Script Date: 2021-06-18 오후 8:43:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fn_CommonDetailName] ( @CdCode VARCHAR(30), @CdDetailCode VARCHAR(50)) 
RETURNS varchar(200)
AS
BEGIN
DECLARE @returnVal VARCHAR(200);


SET		@returnVal =(
SELECT	D.CDCode_Name 
FROM	dbo.Dmn_CommonCode_M AS M
			Inner Join Dmn_CommonCode_D AS D
			ON M.CDCode = D.CDCode
WHERE	M.CDCode = @CdCode
AND		D.CDCode_Dtl = @CdDetailCode);
 

RETURN @returnVal;
END
;

GO

USE [DOMINO_DB]
GO

/****** Object:  StoredProcedure [dbo].[AGTable]    Script Date: 2024-03-27 오후 6:09:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AGTable]
	@OrderNo	varchar(10),
	@SeqNo		varchar(4)
AS
BEGIN

DECLARE	@NewOrderNo	varchar(10) = @OrderNo
DECLARE	@NewSeqNo		varchar(4) = @SeqNo

SELECT JobDetailType, ProdStdCode, Weight INTO #JOBDETAIL FROM
(
		SELECT DISTINCT JobDetailType, ProdStdCode, CAST (CASE JobDetailType WHEN 'EA' THEN 0 ELSE RIGHT(JobDetailType,1) END AS int) [Weight] FROM [DOMINO_DB].[dbo].[Dmn_ReadBarcode] WHERE OrderNo = @NewOrderNo AND SeqNo = @NewSeqNo
		UNION
		SELECT DISTINCT JobDetailType,ProdStdCode, CAST (CASE JobDetailType WHEN 'EA' THEN 0 ELSE RIGHT(JobDetailType,1) END AS int) [Weight] FROM [DOMINO_DB].[dbo].[Dmn_SerialPool] WHERE OrderNo = @NewOrderNo AND SeqNo = @NewSeqNo
) AS JOBDETAIL 
DECLARE @LASTJOBDETAILTYPE NVARCHAR(10) = (SELECT TOP 1 JobDetailType FROM #JOBDETAIL ORDER BY Weight DESC)
SELECT
		Child.ProdStdCode,
		Child.SerialNum,
		Child.JobDetailType,
		Child.FullBarcode_Read,
		Child.AI_FullBarcode_Read,

		Child.ParentProdStdCode,
		Child.ParentSerialNum,
		(SELECT JobDetailType FROM #JOBDETAIL WHERE ProdStdCode = Child.ParentProdStdCode) ParentJobDetailType,
		Child.FullBarcode_Parent,
		Child.AI_FullBarcode_Parent,

		Parent.ParentProdStdCode AS GParentProdStdCode,
		Parent.ParentSerialNum AS GParentSerialNum ,
		(SELECT JobDetailType FROM #JOBDETAIL WHERE ProdStdCode = Parent.ParentProdStdCode) GParentJobDetailType,
		Parent.FullBarcode_Parent AS FullBarcode_GParent,
		Parent.AI_FullBarcode_Parent AS AI_FullBarcode_GParent,

		GParent.ParentSerialNum AS GGParentProdStdCode,
		GParent.ParentSerialNum AS GGParentSerialNum ,
		(SELECT JobDetailType FROM #JOBDETAIL WHERE ProdStdCode = GParent.ProdStdCode) GGParentJobDetailType,
		GParent.FullBarcode_Parent AS FullBarcode_GGParent,
		GParent.AI_FullBarcode_Parent AS AI_FullBarcode_GGParent
		INTO #BASE
		FROM DOMINO_DB.dbo.Dmn_ReadBarcode AS Child
		Left Join DOMINO_DB.dbo.Dmn_ReadBarcode AS Parent On Child.ParentProdStdCode = Parent.ProdStdCode and Child.ParentSerialNum = Parent.SerialNum
		Left Join DOMINO_DB.dbo.Dmn_ReadBarcode AS GParent On Parent.ParentProdStdCode = GParent.ProdStdCode and Parent.ParentSerialNum = GParent.SerialNum
		WHERE Child.OrderNo = @NewOrderNo and Child.SeqNo = @NewSeqNo


SELECT
	JM.LineID,
	JM.CorCode,
	JM.PlantCode,
	JM.ErpOrderNo,
	JM.MfdDate,
	JM.ExpDate,
	JM.AssignDate AS JM_AssignDate,
	JM.ProdCode, 
	JM.OrderNo AS JobOrderNo,
	JM.SeqNo AS JobSeqNo,
	JM.LotNo,
	JM.LotNo_Sub,
	PM.ProdName, 
	PM.ProdName2, 
	PD.JobDetailType AS DetailType,
	PD.SnType AS PD_SnType, 
	PD.SnExpressionID, 
	PD.DesignID,
	PD.Prefix_SSCC, 
	PD.PrinterName, 
	PD.GS1ExtensionCode
	INTO #JOB
	FROM DOMINO_DB.dbo.Dmn_JobOrder_M AS JM
	Left Join DOMINO_DB.dbo.Dmn_JobOrder_AG AS JD On JD.OrderNo = JM.OrderNo and JD.SeqNo = JM.SeqNo
	Left Join DOMINO_DB.dbo.Dmn_Product_M AS PM On PM.ProdCode = JM.ProdCode
	Left Join DOMINO_DB.dbo.Dmn_Product_AG AS PD On PD.ProdCode = JM.ProdCode AND PD.JobDetailType = JD.JobDetailType 
	WHERE JM.OrderNo = @NewOrderNo and JM.SeqNo = @NewSeqNo

 SELECT * FROM
 (
	 SELECT * FROM
	 (
 			SELECT 
			RBEA.[ProdStdCode]  AS StandardCode,
			RBEA.[SerialNum],
			RBEA.[OrderNo],
			RBEA.[SeqNo],
			RBEA.[JobDetailType],
			RBEA.[FullBarcode_Read],
			(SELECT TOP 1 FullBarcode_Parent FROM #BASE where SerialNum = RBEA.SerialNum) AS FullBarcode_Parent,
			(SELECT TOP 1 FullBarcode_GParent FROM #BASE where SerialNum = RBEA.SerialNum) AS FullBarcode_GParent,
			(SELECT TOP 1 FullBarcode_GGParent FROM #BASE where SerialNum = RBEA.SerialNum) AS FullBarcode_GGParent,
			RBEA.[AI_FullBarcode_Read],
			(SELECT TOP 1 AI_FullBarcode_Parent FROM #BASE where SerialNum = RBEA.SerialNum) AS AI_FullBarcode_Parent,
			(SELECT TOP 1 AI_FullBarcode_GParent FROM #BASE where SerialNum = RBEA.SerialNum) AS AI_FullBarcode_GParent,
			(SELECT TOP 1 AI_FullBarcode_GGParent FROM #BASE where SerialNum = RBEA.SerialNum) AS AI_FullBarcode_GGParent,
			(SELECT TOP 1 ParentProdStdCode FROM #BASE where SerialNum = RBEA.SerialNum) AS StandardCode_Parent,
			(SELECT TOP 1 GParentProdStdCode FROM #BASE where SerialNum = RBEA.SerialNum) AS StandardCode_GParent,
			(SELECT TOP 1 GGParentProdStdCode FROM #BASE where SerialNum = RBEA.SerialNum) AS StandardCode_GGParent,
			(SELECT TOP 1 ParentSerialNum FROM #BASE where SerialNum = RBEA.SerialNum) AS SerialNumer_Parent,
			(SELECT TOP 1 GParentSerialNum FROM #BASE where SerialNum = RBEA.SerialNum) AS SerialNumer_GParent,
			(SELECT TOP 1 GGParentSerialNum FROM #BASE where SerialNum = RBEA.SerialNum) AS SerialNumer_GGParent,
			(SELECT TOP 1 ParentJobDetailType FROM #BASE where SerialNum = RBEA.SerialNum) AS JobDetailType_Parent,
			(SELECT TOP 1 GParentJobDetailType FROM #BASE where SerialNum = RBEA.SerialNum) AS JobDetailType_GParent,
			(SELECT TOP 1 GGParentJobDetailType FROM #BASE where SerialNum = RBEA.SerialNum) AS JobDetailType_GGParent,
			dbo.fn_CommonDetailName('192',RBEA.[Status]) AS RB_Status,
			RBEA.[FilePath],
			RBEA.[InsertUser] AS RB_InsertUser,
			RBEA.[InsertDate] AS RB_InsertDate,
			RBEA.[UpdateUser] AS RB_UpdateUser,
			RBEA.[UpdateDate] AS RB_UpdateDate,
			SPBX1.[ResourceType],
			SPBX1.[BarcodeDataFormat],
			SPBX1.[BarcodeType],
			SPBX1.[SerialType],
			SPBX1.[idx_Group],
			SPBX1.[idx_Insert],
			SPBX1.[UseDate],
			SPBX1.[InspectedDate],
			SPBX1.[UseYN] AS SP_UseYN,
			dbo.fn_CommonDetailName('192',SPBX1.[Status]) AS SP_Status,
			SPBX1.[FileName],
			SPBX1.[ConfirmedYN],
			SPBX1.[InsertUser] AS SP_InsertUser,
			SPBX1.[InsertDate] AS SP_InsertDate,
			SPBX1.[UpdateUser] AS SP_UpdateUser,
			SPBX1.[UpdateDate] AS SP_UpdateDate,
			SPBX1.[AssignDate]
			FROM DOMINO_DB.dbo.Dmn_ReadBarcode AS RBEA
			FULL JOIN DOMINO_DB.dbo.Dmn_SerialPool AS SPBX1 ON RBEA.ProdStdCode = SPBX1.ProdStdCode AND RBEA.SerialNum = SPBX1.SerialNum 
			UNION
			SELECT DISTINCT
			SP.[ProdStdCode],
			SP.[SerialNum],
			SP.[OrderNo],
			SP.[SeqNo],
			SP.[JobDetailType],
			RB.[FullBarcode_Parent],
			null [FullBarcode_Parent],
			null [FullBarcode_GParent],
			null [FullBarcode_GGParent],
			RB.[AI_FullBarcode_Parent],
			null [AI_FullBarcode_Parent],
			null [AI_FullBarcode_GParent],
			null [AI_FullBarcode_GGParent],
			null [ParentProdStdCode],
			null [GParentProdStdCode],
			null [GGParentProdStdCode],
			null ParentSerialNum,
			null GParentSerialNum,
			null GGParentSerialNum,
			null ParentJobDetailType,
			null GParentJobDetailType,
			null GGParentJobDetailType,
			null RB_Status,
			null [FilePath],
			null RB_InsertUser,
			null RB_InsertDate,
			null RB_UpdateUser,
			null RB_UpdateDate,
			SP.[ResourceType],
			SP.[BarcodeDataFormat],
			SP.[BarcodeType],
			SP.[SerialType],
			SP.[idx_Group],
			SP.[idx_Insert],
			SP.[UseDate],
			SP.[InspectedDate],
			SP.[UseYN],
			dbo.fn_CommonDetailName('192', SP.[Status]) AS SP_Status,
			SP.[FileName],
			SP.[ConfirmedYN],
			SP.[InsertUser] AS SP_InsertUser,
			SP.[InsertDate] AS SP_InsertDate,
			SP.[UpdateUser] AS SP_UpdateUser,
			SP.[UpdateDate] AS SP_UpdateDate,
			SP.[AssignDate]
			FROM DOMINO_DB.dbo.Dmn_SerialPool AS SP
			FULL JOIN DOMINO_DB.dbo.Dmn_ReadBarcode AS RB ON RB.ParentProdStdCode = SP.ProdStdCode AND RB.ParentSerialNum = SP.SerialNum 
			where SP.JobDetailType = @LASTJOBDETAILTYPE
	 ) AS SerialInfo
	 LEFT JOIN #JOB AS JobOrder ON JobOrder.JobOrderNo = SerialInfo.OrderNo and JobOrder.JobSeqNo = SerialInfo.SeqNo and JobOrder.DetailType = SerialInfo.JobDetailType
) AS AGTable
WHERE OrderNo = @NewOrderNo and SeqNo = @NewSeqNo

DROP TABLE #JOBDETAIL
DROP TABLE #BASE
DROP TABLE #JOB
End


GO


