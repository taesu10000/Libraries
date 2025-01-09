USE [Domino_DB]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_CommonDetailName]    Script Date: 1/22/2021 3:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  UserDefinedFunction [dbo].[fn_CommonDetailName]    Script Date: 2020-12-12 오후 3:30:53 ******/

IF OBJECT_ID('dbo.Dmn_View_PMData', 'U') IS NOT NULL 
  DROP TABLE dbo.Dmn_View_PMData; 
GO

IF OBJECT_ID('dbo.Dmn_View_AGData', 'U') IS NOT NULL 
  DROP TABLE dbo.Dmn_View_AGData; 
GO


--Function : 공통코드 상세 명 가져오기
Create FUNCTION [dbo].[fn_CommonDetailName] ( @CdCode VARCHAR(30), @CdDetailCode VARCHAR(50)) 
RETURNS varchar(200)
AS
BEGIN
DECLARE @returnVal VARCHAR(200);


SET		@returnVal =(
SELECT	D.CDCode_Name 
FROM	Domino_DB.dbo.Dmn_CommonCode_M AS M
			Inner Join Domino_DB.dbo.Dmn_CommonCode_D AS D
			ON M.CDCode = D.CDCode
WHERE	M.CDCode = @CdCode
AND		D.CDCode_Dtl = @CdDetailCode);
 

RETURN @returnVal;
END
;
GO

USE [Domino_DB]
GO

/****** Object:  View [dbo].[Dmn_View_PMData]    Script Date: 2021-01-25 오후 4:30:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Dmn_View_PMData] AS

	SELECT SP.[ProdStdCode]
		  ,SP.[SerialNum]
		  ,SP.[JobDetailType]
		  ,SP.[OrderNo]
		  ,SP.[SeqNo]
		  ,dbo.fn_CommonDetailName('191', SP.[ResourceType]) AS ResourceType
		  ,dbo.fn_CommonDetailName('190', SP.[SerialType]) AS [SerialType]
		  ,SP.Idx_Group
		  ,SP.Idx_Insert
		  ,SP.UseDate
		  ,SP.InspectedDate
		  ,dbo.fn_CommonDetailName('193', SP.[UseYN]) AS [UseYN]
		  ,dbo.fn_CommonDetailName('192', SP.[Status]) AS [Status]
		  ,SP.[FileName]
		  ,SP.[Reserved1] AS SP_Reserved1
		  ,SP.[Reserved2] AS SP_Reserved2
		  ,SP.[Reserved3] AS SP_Reserved3
		  ,SP.[Reserved4] AS SP_Reserved4
		  ,SP.[Reserved5] AS SP_Reserved5
		  ,SP.InsertDate AS SP_InsertDate
		  ,SP.InsertUser AS SP_InsertUser
		  ,SP.UpdateDate AS SP_UpdateDate
		  ,SP.UpdateUser AS SP_UpdateUser
		  ,VR.DecodedBarcode
		  ,VR.Read_OCR
		  ,VR.Grade_Barcode
		  ,VR.[FilePath]
		  ,VR.CameraIndex
		  ,VR.[Reserved1] AS VR_Reserved1
		  ,VR.[Reserved2] AS VR_Reserved2
		  ,VR.[Reserved3] AS VR_Reserved3
		  ,VR.[Reserved4] AS VR_Reserved4
		  ,VR.[Reserved5] AS VR_Reserved5
		  ,VR.InsertDate AS VR_InsertDate
		  ,VR.InsertUser AS VR_InsertUser
		  ,VR.UpdateDate AS VR_UpdateDate
		  ,VR.UpdateUser AS VR_UpdateUser
		  ,JM.LineID
		  ,JM.CorCode
		  ,JM.PlantCode
		  ,JM.ErpOrderNo
		  ,dbo.fn_CommonDetailName('201', JM.[OrderType]) AS [OrderType]
		  ,JM.MfdDate
		  ,JM.ExpDate
		  ,JM.LotNo
		  ,JM.LotNo_Sub
		  ,JM.Cnt_JobPlan
		  ,JM.Reserved1 AS JM_Reserved1
		  ,JM.Reserved2 AS JM_Reserved2
		  ,JM.Reserved3 AS JM_Reserved3
		  ,JM.Reserved4 AS JM_Reserved4
		  ,JM.Reserved5 AS JM_Reserved5
		  ,JM.AssignDate
		  ,JM.AssignUser
		  ,JM.InsertDate AS JM_InsertDate
		  ,JM.InsertUser AS JM_InsertUser
		  ,JM.UpdateDate AS JM_UpdateDate
		  ,JM.UpdateUser AS JM_UpdateUser
		  ,JD.Cnt_Good
		  ,JD.Cnt_Error
		  ,JD.Cnt_Sample
		  ,JD.Cnt_Destroy
		  ,JD.Cnt_Status1
		  ,JD.Cnt_Status2
		  ,JD.Cnt_Status3
		  ,JD.Cnt_Status4
		  ,JD.Cnt_Status5
		  ,JD.UserDefineData1
		  ,JD.UserDefineData2
		  ,JD.PrinterVariable1
		  ,JD.PrinterVariable2
		  ,JD.PrinterVariable3
		  ,JD.PrinterVariable4
		  ,JD.PrinterVariable5
		  ,JD.Reserved1 AS JD_Reserved1
		  ,JD.Reserved2 AS JD_Reserved2
		  ,JD.Reserved3 AS JD_Reserved3
		  ,JD.Reserved4 AS JD_Reserved4
		  ,JD.Reserved5 AS JD_Reserved5
		  ,JD.StartUser As StartUser
		  ,JD.StartDate AS StartDate
		  ,JD.CompleteDate
		  ,JD.CompleteUser
		  ,PR.ProdCode
		  ,PR.ProdName
		  ,PR.ProdName2
		  ,PR.AGLevel
		  ,PR.Remark
		  ,PR.Exp_Day AS EXP_OFFSET
		  ,PR.Delay_Print
		  ,PR.Delay_Shot1
		  ,PR.Delay_Shot2
		  ,PR.Delay_NG
		  ,PR.Reserved1 AS PM_Reserved1
		  ,PR.Reserved2 AS PM_Reserved2
		  ,PR.Reserved3 AS PM_Reserved3
		  ,PR.Reserved4 AS PM_Reserved4
		  ,PR.Reserved5 AS PM_Reserved5
		  ,PR.InsertDate AS PM_InsertDate
		  ,PR.InsertUser AS PM_InsertUser
		  ,PR.UpdateDate AS PM_UpdateDate
		  ,PR.UpdateUser AS PM_UpdateUser
		  ,PD.BarcodeType
		  ,PD.BarcodeDataFormat
		  ,dbo.fn_CommonDetailName('201', PD.SnType) AS SerialNumberType
		  ,PD.SnExpressionID
		  ,PD.DesignID
		  ,PD.Capacity
		  ,PD.LIC
		  ,PD.PCN
		  ,PD.Condition
		  ,PD.Reserved1 AS PD_Reserved1
		  ,PD.Reserved2 AS PD_Reserved2
		  ,PD.Reserved3 AS PD_Reserved3
		  ,PD.Reserved4 AS PD_Reserved4
		  ,PD.Reserved5 AS PD_Reserved5
		  ,PD.InsertDate AS PD_InsertDate
		  ,PD.InsertUser AS PD_InsertUser
		  ,PD.UpdateDate AS PD_UpdateDate
		  ,PD.UpdateUser AS PD_UpdateUser
		  ,IsPass = Case When SP.[Status] = 'PA' THEN 'TRUE' ELSE 'FALSE' END 
		  ,IsSample = Case When SP.[Status] = 'SN' OR SP.[Status] = 'ST' OR SP.[Status] = 'SS' OR SP.[Status] = 'SC' OR SP.[Status] = 'SA' THEN 'TRUE' ELSE 'FALSE' END
		  ,IsReject = Case When SP.[Status] = 'RE' THEN 'TRUE' ELSE 'FALSE' END
	  FROM [Domino_DB].[dbo].[Dmn_SerialPool] AS SP
		LEFT OUTER JOIN Domino_DB.dbo.Dmn_JobOrder_M AS JM ON SP.OrderNo = JM.OrderNo AND	SP.SeqNo = JM.SeqNo
		LEFT OUTER JOIN Domino_DB.dbo.Dmn_JobOrder_PM AS JD ON SP.OrderNo = JD.OrderNo AND SP.SeqNo = JD.SeqNo AND Sp.JobDetailType = JD.JobDetailType 
		LEFT OUTER JOIN Domino_DB.dbo.Dmn_Product_M AS PR ON JM.ProdCode = PR.ProdCode
		LEFT OUTER JOIN Domino_DB.dbo.Dmn_Product_PM AS PD ON JM.ProdCode = PD.ProdCode AND SP.JobDetailType = PD.JobDetailType
		LEFT OUTER JOIN [Domino_DB].[dbo].[Dmn_VisionResult]AS VR ON SP.OrderNo = VR.OrderNo AND SP.SeqNo = VR.SeqNo AND SP.JobDetailType = VR.JobDetailType AND SP.InspectedDate = VR.InsertDate
;
GO

SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [SerialPoolIndex] ON [dbo].[Dmn_SerialPool]
(
	[JobDetailType] ASC,
	[OrderNo] ASC,
	[SeqNo] ASC,
	[Idx_Insert] ASC,
	[UseYN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


CREATE NONCLUSTERED INDEX [SerialPoolIndex] ON [dbo].[Dmn_SerialPool]
(
	[JobDetailType] ASC,
	[OrderNo] ASC,
	[SeqNo] ASC,
	[Idx_Insert] ASC,
	[UseYN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [SerialIndex] ON [dbo].[Dmn_SerialPool]
(
	[SerialNum] ASC,
	[ProdStdCode] ASC,
	[Status] ASC,
	[SeqNo] ASC,
	[OrderNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [SerialIndex2] ON [dbo].[Dmn_SerialPool]
(
	[InspectedDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [VisionResultIndex] ON [dbo].[Dmn_VisionResult]
(
	[InsertDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO








