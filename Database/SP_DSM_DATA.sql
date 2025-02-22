CREATE PROCEDURE [dbo].[SP_DSM_DATA](
  @argPlantCode NVARCHAR(20) = NULL 
 ,@argOrderNo   NVARCHAR(50) = NULL 
 ,@argSeqNo 	NVARCHAR(4)  = NULL 
 ,@argStatus    NVARCHAR(2)  = NULL 
)
WITH RECOMPILE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE	 @PlantCode NVARCHAR(20)  = @argPlantCode 
			,@OrderNo   NVARCHAR(50)  = @argOrderNo   
			,@SeqNo 	NVARCHAR(4)   = @argSeqNo 	
			,@Status    NVARCHAR(50)  = @argStatus    


	DROP TABLE IF EXISTS #SP
	DROP TABLE IF EXISTS #VR
	DROP TABLE IF EXISTS #RB
	DROP TABLE IF EXISTS #RRB
	DROP TABLE IF EXISTS #RBBASE
	DROP TABLE IF EXISTS #MAP
	DROP TABLE IF EXISTS #SB

	SELECT * INTO #SP FROM Dmn_SerialPool WHERE PlantCode = @PlantCode AND OrderNo = @OrderNo AND SeqNo = @SeqNo AND (@Status IS NULL OR @Status = '' OR Status = @Status)
	CREATE NONCLUSTERED INDEX IX_SP_STD_SER ON #SP(ProdStdCode, SerialNum)
	CREATE NONCLUSTERED INDEX IX_SP_INSP ON #SP(JobDetailType, InspectedDate)

	SELECT * INTO #VR FROM Dmn_VisionResult WHERE PlantCode = @PlantCode AND OrderNo = @OrderNo AND SeqNo = @SeqNo
	CREATE NONCLUSTERED INDEX IX_VR_STD_SER ON #VR(JobDetailType, InsertDate)

	SELECT *, CASE JobDetailType WHEN 'EA' THEN 0 ELSE RIGHT(JobDetailType, 1) END [EnJobDetail] INTO #RRB FROM Dmn_ReadBarcode WHERE PlantCode = @PlantCode AND OrderNo = @OrderNo AND SeqNo = @SeqNo

	CREATE NONCLUSTERED INDEX IX_RRB_STD_SER ON #RRB(ProdStdCode, SerialNum)
	CREATE NONCLUSTERED INDEX IX_RRB_PSTD_PSER ON #RRB(ParentProdStdCode, ParentSerialNum)

	DECLARE @SECONDLAST NVARCHAR(20)
	SELECT TOP 1 @SECONDLAST = JobDetailType FROM #RRB ORDER BY EnJobDetail DESC

	;WITH CTE AS
	(
		SELECT 
				[PlantCode]
				,[ParentProdStdCode]			[ProdStdCode]
				,[ParentSerialNum]				[SerialNum]
				,[MachineID]
				,[OrderNo]
				,[SeqNo]
				,CONCAT('BX', EnJobDetail + 1)  [JobDetailType]
				,[FullBarcode_Parent]			[FullBarcode_Read]
				,[AI_FullBarcode_Parent]		[AI_FullBarcode_Read]
				,NULL							[FullBarcode_Parent]
				,NULL							[AI_FullBarcode_Parent]
				,NULL							[ParentProdStdCode]
				,NULL							[ParentSerialNum]
				,[Status]
				,[Reserved1]
				,[Reserved2]
				,[Reserved3]
				,[Reserved4]
				,[Reserved5]
				,[FilePath]
				,[InsertUser]
				,[InsertDate]
				,[UpdateUser]
				,[UpdateDate]
				,EnJobDetail + 1 [EnJobDetail]
				,ROW_NUMBER() OVER (PARTITION BY [ParentProdStdCode], [ParentSerialNum] ORDER BY COALESCE([UpdateDate], [InsertDate]) DESC) [ROWNUM]
		FROM	#RRB
		WHERE	JobDetailType = @SECONDLAST
	) 
	INSERT INTO #RRB 
	SELECT		[PlantCode]
				,[ProdStdCode]
				,[SerialNum]
				,[MachineID]
				,[OrderNo]
				,[SeqNo]
				,[JobDetailType]
				,[FullBarcode_Read]
				,[AI_FullBarcode_Read]
				,[FullBarcode_Parent]
				,[AI_FullBarcode_Parent]
				,[ParentProdStdCode]
				,[ParentSerialNum]
				,[Status]
				,[Reserved1]
				,[Reserved2]
				,[Reserved3]
				,[Reserved4]
				,[Reserved5]
				,[FilePath]
				,[InsertUser]
				,[InsertDate]
				,[UpdateUser]
				,[UpdateDate]
				,[EnJobDetail]
	FROM		CTE 
	WHERE		ROWNUM = 1

	SELECT	*
			,CASE JobDetailType WHEN 'EA'  THEN SerialNum END														EA_Serial
			,CASE JobDetailType WHEN 'EA'  THEN ParentSerialNum    WHEN 'BX1' THEN SerialNum END					BX1_Serial
			,CASE JobDetailType WHEN 'BX1' THEN ParentSerialNum    WHEN 'BX2' THEN SerialNum END					BX2_Serial
			,CASE JobDetailType WHEN 'BX2' THEN ParentSerialNum    WHEN 'BX3' THEN SerialNum END					BX3_Serial
			,CASE JobDetailType WHEN 'BX3' THEN ParentSerialNum    WHEN 'BX4' THEN SerialNum END					BX4_Serial
			,CASE JobDetailType WHEN 'BX4' THEN ParentSerialNum    WHEN 'BX5' THEN SerialNum END					BX5_Serial
			,CASE JobDetailType WHEN 'BX5' THEN ParentSerialNum    END												BX6_Serial
			,CASE JobDetailType WHEN 'EA'  THEN FullBarcode_Read   END												EA_FullBarcode_Read
			,CASE JobDetailType WHEN 'EA'  THEN FullBarcode_Parent WHEN 'BX1' THEN FullBarcode_Read END				BX1_FullBarcode_Read
			,CASE JobDetailType WHEN 'BX1' THEN FullBarcode_Parent WHEN 'BX2' THEN FullBarcode_Read END				BX2_FullBarcode_Read
			,CASE JobDetailType WHEN 'BX2' THEN FullBarcode_Parent WHEN 'BX3' THEN FullBarcode_Read END				BX3_FullBarcode_Read
			,CASE JobDetailType WHEN 'BX3' THEN FullBarcode_Parent WHEN 'BX4' THEN FullBarcode_Read END				BX4_FullBarcode_Read
			,CASE JobDetailType WHEN 'BX4' THEN FullBarcode_Parent WHEN 'BX5' THEN FullBarcode_Read END				BX5_FullBarcode_Read
			,CASE JobDetailType WHEN 'BX5' THEN FullBarcode_Parent END												BX6_FullBarcode_Read
			,CASE JobDetailType WHEN 'EA'  THEN AI_FullBarcode_Read   END											EA_AI_FullBarcode_Read
			,CASE JobDetailType WHEN 'EA'  THEN AI_FullBarcode_Parent WHEN 'BX1' THEN AI_FullBarcode_Read END		BX1_AI_FullBarcode_Read
			,CASE JobDetailType WHEN 'BX1' THEN AI_FullBarcode_Parent WHEN 'BX2' THEN AI_FullBarcode_Read END		BX2_AI_FullBarcode_Read
			,CASE JobDetailType WHEN 'BX2' THEN AI_FullBarcode_Parent WHEN 'BX3' THEN AI_FullBarcode_Read END		BX3_AI_FullBarcode_Read
			,CASE JobDetailType WHEN 'BX3' THEN AI_FullBarcode_Parent WHEN 'BX4' THEN AI_FullBarcode_Read END		BX4_AI_FullBarcode_Read
			,CASE JobDetailType WHEN 'BX4' THEN AI_FullBarcode_Parent WHEN 'BX5' THEN AI_FullBarcode_Read END		BX5_AI_FullBarcode_Read
			,CASE JobDetailType WHEN 'BX5' THEN AI_FullBarcode_Parent END											BX6_AI_FullBarcode_Read
	INTO	#RBBASE
	FROM	#RRB

	CREATE NONCLUSTERED INDEX IX_RBBASE_STD_SER ON #RBBASE(ProdStdCode, SerialNum)
	CREATE NONCLUSTERED INDEX IX_RBBASE_PSTD_PSER ON #RBBASE(ParentProdStdCode, ParentSerialNum)

	--CREATE MAP OVER BX1
	;WITH RECURSIV AS
	(
		SELECT		ProdStdCode
					,SerialNum
					,JobDetailType
					,ParentProdStdCode
					,ParentSerialNum
					,FullBarcode_Read
					,AI_FullBarcode_Read
					,FullBarcode_Parent
					,AI_FullBarcode_Parent
					,CASE JobDetailType WHEN 'EA'  THEN SerialNum END														EA_Serial
					,CASE JobDetailType WHEN 'EA'  THEN ParentSerialNum    WHEN 'BX1' THEN SerialNum END					BX1_Serial
					,CASE JobDetailType WHEN 'BX1' THEN ParentSerialNum    WHEN 'BX2' THEN SerialNum END					BX2_Serial
					,CASE JobDetailType WHEN 'BX2' THEN ParentSerialNum    WHEN 'BX3' THEN SerialNum END					BX3_Serial
					,CASE JobDetailType WHEN 'BX3' THEN ParentSerialNum    WHEN 'BX4' THEN SerialNum END					BX4_Serial
					,CASE JobDetailType WHEN 'BX4' THEN ParentSerialNum    WHEN 'BX5' THEN SerialNum END					BX5_Serial
					,CASE JobDetailType WHEN 'BX5' THEN ParentSerialNum    END												BX6_Serial
					,CASE JobDetailType WHEN 'EA'  THEN FullBarcode_Read   END												EA_FullBarcode_Read
					,CASE JobDetailType WHEN 'EA'  THEN FullBarcode_Parent WHEN 'BX1' THEN FullBarcode_Read END				BX1_FullBarcode_Read
					,CASE JobDetailType WHEN 'BX1' THEN FullBarcode_Parent WHEN 'BX2' THEN FullBarcode_Read END				BX2_FullBarcode_Read
					,CASE JobDetailType WHEN 'BX2' THEN FullBarcode_Parent WHEN 'BX3' THEN FullBarcode_Read END				BX3_FullBarcode_Read
					,CASE JobDetailType WHEN 'BX3' THEN FullBarcode_Parent WHEN 'BX4' THEN FullBarcode_Read END				BX4_FullBarcode_Read
					,CASE JobDetailType WHEN 'BX4' THEN FullBarcode_Parent WHEN 'BX5' THEN FullBarcode_Read END				BX5_FullBarcode_Read
					,CASE JobDetailType WHEN 'BX5' THEN FullBarcode_Parent END												BX6_FullBarcode_Read
					,CASE JobDetailType WHEN 'EA'  THEN AI_FullBarcode_Read   END											EA_AI_FullBarcode_Read
					,CASE JobDetailType WHEN 'EA'  THEN AI_FullBarcode_Parent WHEN 'BX1' THEN AI_FullBarcode_Read END		BX1_AI_FullBarcode_Read
					,CASE JobDetailType WHEN 'BX1' THEN AI_FullBarcode_Parent WHEN 'BX2' THEN AI_FullBarcode_Read END		BX2_AI_FullBarcode_Read
					,CASE JobDetailType WHEN 'BX2' THEN AI_FullBarcode_Parent WHEN 'BX3' THEN AI_FullBarcode_Read END		BX3_AI_FullBarcode_Read
					,CASE JobDetailType WHEN 'BX3' THEN AI_FullBarcode_Parent WHEN 'BX4' THEN AI_FullBarcode_Read END		BX4_AI_FullBarcode_Read
					,CASE JobDetailType WHEN 'BX4' THEN AI_FullBarcode_Parent WHEN 'BX5' THEN AI_FullBarcode_Read END		BX5_AI_FullBarcode_Read
					,CASE JobDetailType WHEN 'BX5' THEN AI_FullBarcode_Parent END											BX6_AI_FullBarcode_Read
		FROM		#RBBASE

		UNION ALL
		SELECT		R.ProdStdCode
					,R.SerialNum
					,R.JobDetailType
					,R.ParentProdStdCode
					,R.ParentSerialNum
					,R.FullBarcode_Read
					,R.AI_FullBarcode_Read
					,R.FullBarcode_Parent
					,R.AI_FullBarcode_Parent
					,CASE C.JobDetailType WHEN 'EA'  THEN C.SerialNum END														EA_Serial
					,CASE C.JobDetailType WHEN 'EA'  THEN C.ParentSerialNum    WHEN 'BX1' THEN C.SerialNum END					BX1_Serial
					,CASE C.JobDetailType WHEN 'BX1' THEN C.ParentSerialNum    WHEN 'BX2' THEN C.SerialNum END					BX2_Serial
					,CASE C.JobDetailType WHEN 'BX2' THEN C.ParentSerialNum    WHEN 'BX3' THEN C.SerialNum END					BX3_Serial
					,CASE C.JobDetailType WHEN 'BX3' THEN C.ParentSerialNum    WHEN 'BX4' THEN C.SerialNum END					BX4_Serial
					,CASE C.JobDetailType WHEN 'BX4' THEN C.ParentSerialNum    WHEN 'BX5' THEN C.SerialNum END					BX5_Serial
					,CASE C.JobDetailType WHEN 'BX5' THEN C.ParentSerialNum    END												BX6_Serial
					,CASE C.JobDetailType WHEN 'EA'  THEN C.FullBarcode_Read   END												EA_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'EA'  THEN C.FullBarcode_Parent WHEN 'BX1' THEN C.FullBarcode_Read END			BX1_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'BX1' THEN C.FullBarcode_Parent WHEN 'BX2' THEN C.FullBarcode_Read END			BX2_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'BX2' THEN C.FullBarcode_Parent WHEN 'BX3' THEN C.FullBarcode_Read END			BX3_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'BX3' THEN C.FullBarcode_Parent WHEN 'BX4' THEN C.FullBarcode_Read END			BX4_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'BX4' THEN C.FullBarcode_Parent WHEN 'BX5' THEN C.FullBarcode_Read END			BX5_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'BX5' THEN C.FullBarcode_Parent END												BX6_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'EA'  THEN C.AI_FullBarcode_Read END												EA_AI_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'EA'  THEN C.AI_FullBarcode_Parent WHEN 'BX1' THEN C.AI_FullBarcode_Read END		BX1_AI_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'BX1' THEN C.AI_FullBarcode_Parent WHEN 'BX2' THEN C.AI_FullBarcode_Read END		BX2_AI_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'BX2' THEN C.AI_FullBarcode_Parent WHEN 'BX3' THEN C.AI_FullBarcode_Read END		BX3_AI_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'BX3' THEN C.AI_FullBarcode_Parent WHEN 'BX4' THEN C.AI_FullBarcode_Read END		BX4_AI_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'BX4' THEN C.AI_FullBarcode_Parent WHEN 'BX5' THEN C.AI_FullBarcode_Read END		BX5_AI_FullBarcode_Read
					,CASE C.JobDetailType WHEN 'BX5' THEN C.AI_FullBarcode_Parent END											BX6_AI_FullBarcode_Read
		FROM		#RBBASE R 
		INNER JOIN	RECURSIV C 
		ON			R.ParentProdStdCode = C.ProdStdCode AND R.ParentSerialNum = C.SerialNum
	)
	SELECT		ProdStdCode
				,SerialNum
				,MAX(EA_Serial				) EA_Serial
				,MAX(BX1_Serial				) BX1_Serial
				,MAX(BX2_Serial				) BX2_Serial
				,MAX(BX3_Serial				) BX3_Serial
				,MAX(BX4_Serial				) BX4_Serial
				,MAX(BX5_Serial				) BX5_Serial
				,MAX(BX6_Serial				) BX6_Serial
				,MAX(EA_FullBarcode_Read	) EA_FullBarcode_Read
				,MAX(BX1_FullBarcode_Read	) BX1_FullBarcode_Read
				,MAX(BX2_FullBarcode_Read	) BX2_FullBarcode_Read
				,MAX(BX3_FullBarcode_Read	) BX3_FullBarcode_Read
				,MAX(BX4_FullBarcode_Read	) BX4_FullBarcode_Read
				,MAX(BX5_FullBarcode_Read	) BX5_FullBarcode_Read
				,MAX(BX6_FullBarcode_Read	) BX6_FullBarcode_Read
				,MAX(EA_AI_FullBarcode_Read	) EA_AI_FullBarcode_Read
				,MAX(BX1_AI_FullBarcode_Read) BX1_AI_FullBarcode_Read
				,MAX(BX2_AI_FullBarcode_Read) BX2_AI_FullBarcode_Read
				,MAX(BX3_AI_FullBarcode_Read) BX3_AI_FullBarcode_Read
				,MAX(BX4_AI_FullBarcode_Read) BX4_AI_FullBarcode_Read
				,MAX(BX5_AI_FullBarcode_Read) BX5_AI_FullBarcode_Read
				,MAX(BX6_AI_FullBarcode_Read) BX6_AI_FullBarcode_Read
	INTO		#MAP
	FROM		RECURSIV 
	GROUP BY	ProdStdCode, SerialNum
	OPTION		(MAXRECURSION 0)

	CREATE NONCLUSTERED INDEX IX_MAP_STD_SER ON #MAP(ProdStdCode, SerialNum)

	SELECT		 R.[PlantCode]
				,R.[ProdStdCode]
				,R.[SerialNum]
				,R.[MachineID]
				,R.[OrderNo]
				,R.[SeqNo]
				,R.[JobDetailType]
				,R.[FullBarcode_Read]
				,R.[AI_FullBarcode_Read]
				,R.[FullBarcode_Parent]
				,R.[AI_FullBarcode_Parent]
				,R.[ParentProdStdCode]
				,R.[ParentSerialNum]
				,R.[Status]
				,R.[Reserved1]
				,R.[Reserved2]
				,R.[Reserved3]
				,R.[Reserved4]
				,R.[Reserved5]
				,R.[FilePath]
				,R.[InsertUser]
				,R.[InsertDate]
				,R.[UpdateUser]
				,R.[UpdateDate]
				,R.[EnJobDetail]
				,COALESCE(R.EA_Serial			   , M.EA_Serial			  ) EA_Serial
				,COALESCE(R.BX1_Serial			   , M.BX1_Serial			  ) BX1_Serial
				,COALESCE(R.BX2_Serial			   , M.BX2_Serial			  ) BX2_Serial
				,COALESCE(R.BX3_Serial			   , M.BX3_Serial			  ) BX3_Serial
				,COALESCE(R.BX4_Serial			   , M.BX4_Serial			  ) BX4_Serial
				,COALESCE(R.BX5_Serial			   , M.BX5_Serial			  ) BX5_Serial
				,COALESCE(R.BX6_Serial			   , M.BX6_Serial			  ) BX6_Serial
				,COALESCE(R.EA_FullBarcode_Read	   , M.EA_FullBarcode_Read	  ) EA_FullBarcode_Read
				,COALESCE(R.BX1_FullBarcode_Read   , M.BX1_FullBarcode_Read	  ) BX1_FullBarcode_Read
				,COALESCE(R.BX2_FullBarcode_Read   , M.BX2_FullBarcode_Read	  ) BX2_FullBarcode_Read
				,COALESCE(R.BX3_FullBarcode_Read   , M.BX3_FullBarcode_Read	  ) BX3_FullBarcode_Read
				,COALESCE(R.BX4_FullBarcode_Read   , M.BX4_FullBarcode_Read	  ) BX4_FullBarcode_Read
				,COALESCE(R.BX5_FullBarcode_Read   , M.BX5_FullBarcode_Read	  ) BX5_FullBarcode_Read
				,COALESCE(R.BX6_FullBarcode_Read   , M.BX6_FullBarcode_Read	  ) BX6_FullBarcode_Read
				,COALESCE(R.EA_AI_FullBarcode_Read , M.EA_AI_FullBarcode_Read ) EA_AI_FullBarcode_Read
				,COALESCE(R.BX1_AI_FullBarcode_Read, M.BX1_AI_FullBarcode_Read) BX1_AI_FullBarcode_Read
				,COALESCE(R.BX2_AI_FullBarcode_Read, M.BX2_AI_FullBarcode_Read) BX2_AI_FullBarcode_Read
				,COALESCE(R.BX3_AI_FullBarcode_Read, M.BX3_AI_FullBarcode_Read) BX3_AI_FullBarcode_Read
				,COALESCE(R.BX4_AI_FullBarcode_Read, M.BX4_AI_FullBarcode_Read) BX4_AI_FullBarcode_Read
				,COALESCE(R.BX5_AI_FullBarcode_Read, M.BX5_AI_FullBarcode_Read) BX5_AI_FullBarcode_Read
				,COALESCE(R.BX6_AI_FullBarcode_Read, M.BX6_AI_FullBarcode_Read) BX6_AI_FullBarcode_Read
	INTO		#RB
	FROM		#RBBASE R
	LEFT JOIN	#MAP M
	ON			R.ParentProdStdCode = M.ProdStdCode AND R.ParentSerialNum = M.SerialNum


	DECLARE		 @JM_LineID		NVARCHAR(20)
				,@JM_CorCode		NVARCHAR(20)
				,@JM_ErpOrderNo	NVARCHAR(50)
				,@JM_OrderType		NVARCHAR(20)
				,@JM_MfdDate		DATETIME
				,@JM_ExpDate		DATETIME
				,@JM_LotNo			NVARCHAR(20)
				,@JM_LotNo_Sub		NVARCHAR(20)
				,@JM_Cnt_JobPlan	INT
				,@JM_DateOfTest	DATETIME
				,@JM_DSMReportDate DATETIME
				,@JM_DSMReportUser NVARCHAR(20)
				,@JM_Reserved1		NVARCHAR(90)
				,@JM_Reserved2		NVARCHAR(90)
				,@JM_Reserved3		NVARCHAR(90)
				,@JM_Reserved4		NVARCHAR(90)
				,@JM_Reserved5		NVARCHAR(90)
				,@JM_AssignDate	DATETIME
				,@JM_AssignUser	NVARCHAR(20)
				,@JM_InsertDate	DATETIME
				,@JM_InsertUser	NVARCHAR(20)
				,@JM_UpdateDate	DATETIME
				,@JM_UpdateUser	NVARCHAR(20)
				,@ProdCode			NVARCHAR(50)

	SELECT		 @JM_LineID			= LineID			
				,@JM_CorCode		= CorCode		
				,@JM_ErpOrderNo		= ErpOrderNo		
				,@JM_OrderType		= (SELECT TOP 1 CDCode_Name FROM Dmn_CommonCode_D WHERE CDCode = '201' AND CDCode_Dtl = OrderType)
				,@JM_MfdDate		= MfdDate		
				,@JM_ExpDate		= ExpDate		
				,@JM_LotNo			= LotNo			
				,@JM_LotNo_Sub		= LotNo_Sub		
				,@JM_Cnt_JobPlan	= Cnt_JobPlan	
				,@JM_DateOfTest		= DateOfTest		
				,@JM_DSMReportDate	= DSMReportDate	
				,@JM_DSMReportUser	= DSMReportUser	
				,@JM_Reserved1		= Reserved1		
				,@JM_Reserved2		= Reserved2		
				,@JM_Reserved3		= Reserved3		
				,@JM_Reserved4		= Reserved4		
				,@JM_Reserved5		= Reserved5		
				,@JM_AssignDate		= AssignDate		
				,@JM_AssignUser		= AssignUser		
				,@JM_InsertDate		= InsertDate		
				,@JM_InsertUser		= InsertUser		
				,@JM_UpdateDate		= UpdateDate		
				,@JM_UpdateUser		= UpdateUser
				,@ProdCode			= ProdCode
	FROM		Dmn_JobOrder_M
	WHERE		PlantCode = @PlantCode AND OrderNo = @OrderNo AND SeqNo = @SeqNo

	DECLARE		 @PM_ProdCode	nvarchar(50)
				,@PM_ProdName	nvarchar(200)
				,@PM_ProdName2	nvarchar(200)
				,@PM_AGLevel	int
				,@PM_Remark		nvarchar(200)
				,@PM_Exp_Day	int
				,@PM_Reserved1	nvarchar(90)
				,@PM_Reserved2	nvarchar(90)
				,@PM_Reserved3	nvarchar(90)
				,@PM_Reserved4	nvarchar(90)
				,@PM_Reserved5	nvarchar(90)
				,@PM_InsertUser	nvarchar(50)
				,@PM_InsertDate	datetime
				,@PM_UpdateUser	nvarchar(50)
				,@PM_UpdateDate	datetime

	SELECT		 @PM_ProdCode	= ProdCode		
				,@PM_ProdName	= ProdName		
				,@PM_ProdName2	= ProdName2	
				,@PM_AGLevel	= AGLevel		
				,@PM_Remark		= Remark		
				,@PM_Exp_Day	= Exp_Day		
				,@PM_Reserved1	= Reserved1	
				,@PM_Reserved2	= Reserved2	
				,@PM_Reserved3	= Reserved3	
				,@PM_Reserved4	= Reserved4	
				,@PM_Reserved5	= Reserved5	
				,@PM_InsertUser	= InsertUser	
				,@PM_InsertDate	= InsertDate	
				,@PM_UpdateUser	= UpdateUser	
				,@PM_UpdateDate	= UpdateDate	
	FROM		Dmn_Product_M
	WHERE		PlantCode = @PlantCode AND ProdCode = @ProdCode

	
	SELECT	*
	INTO	#SB
	FROM
	(
		SELECT ProdStdCode, SerialNum, JobDetailType, CASE JobDetailType WHEN 'EA' THEN 0 ELSE RIGHT(JobDetailType, 1) END [EnJobDetail] FROM #SP SP WHERE SP.PlantCode = @PlantCode AND SP.OrderNo = @OrderNo AND SP.SeqNo = @SeqNo
		UNION ALL SELECT ProdStdCode, SerialNum, JobDetailType, EnJobDetail FROM #RB RB WHERE RB.PlantCode = @PlantCode AND RB.OrderNo = @OrderNo AND RB.SeqNo = @SeqNo
	) [A]
	GROUP BY ProdStdCode, SerialNum, JobDetailType, EnJobDetail
	
	CREATE NONCLUSTERED INDEX IX_SB_STD_SER ON #SB(ProdStdCode, SerialNum)
	CREATE NONCLUSTERED INDEX IX_SB_STD_SER_JDT ON #SB(ProdStdCode, SerialNum, JobDetailType)

	SET NOCOUNT OFF;

	;WITH 
	 CD191   AS (SELECT CDCode_Dtl, CDCode_Name FROM Dmn_CommonCode_D WHERE CDCode = '191')
	,CD190   AS (SELECT CDCode_Dtl, CDCode_Name FROM Dmn_CommonCode_D WHERE CDCode = '190')
	,CD193   AS (SELECT CDCode_Dtl, CDCode_Name FROM Dmn_CommonCode_D WHERE CDCode = '193')
	,CD192   AS (SELECT CDCode_Dtl, CDCode_Name FROM Dmn_CommonCode_D WHERE CDCode = '192')
	,CD201   AS (SELECT CDCode_Dtl, CDCode_Name FROM Dmn_CommonCode_D WHERE CDCode = '201')
	SELECT			@PlantCode PlantCode
					,SB.ProdStdCode
					,SB.SerialNum
					,SP.SerialNum AS SP_SerialNum
					,RB.SerialNum AS RB_Serialnum
					,COALESCE(SP.MachineID, RB.MachineID) MachineID
					,SP.MachineID AS SP_MachineID
					,RB.MachineID AS RB_MachineID
					,SB.JobDetailType JobDetailType
					,@OrderNo OrderNo
					,@SeqNo SeqNo
					,CD191.CDCode_Name ResourceType
					,CD190.CDCode_Name SerialType
					,SP.idx_Group
					,SP.idx_Insert
					,SP.UseDate
					,SP.InspectedDate
					,CD193.CDCode_Name UseYN
					,CD192.CDCode_Name Status
					,SP.FileName
					,SP.Reserved1 SP_Reserved1
					,SP.Reserved2 SP_Reserved2
					,SP.Reserved3 SP_Reserved3
					,SP.Reserved4 SP_Reserved4
					,SP.Reserved5 SP_Reserved5
					,SP.InsertDate SP_InsertDate
					,SP.InsertUser SP_InsertUser
					,SP.UpdateDate SP_UpdateDate
					,SP.UpdateUser SP_UpdateUser
					,CASE COALESCE(SP.[Status], RB.[Status]) WHEN 'PA' THEN 'TRUE' ELSE 'FALSE' END								[IsPass]
					,CASE WHEN COALESCE(SP.[Status], RB.[Status])  IN ('SN','ST','SS','SC','SA') THEN 'TRUE' ELSE 'FALSE' END	[IsSample]
					,CASE COALESCE(SP.[Status], RB.[Status]) WHEN 'RE' THEN 'TRUE' ELSE 'FALSE' END								[IsReject]
					,DecodedBarcode
					,Read_OCR
					,Grade_Barcode
					,VR.FilePath
					,VR.FilePath AS DB_FilePath
					,VR.FilePath AS VR_FilePath
					,CameraIndex
					,VR.[Reserved1] AS VR_Reserved1
					,VR.[Reserved2] AS VR_Reserved2
					,VR.[Reserved3] AS VR_Reserved3
					,VR.[Reserved4] AS VR_Reserved4
					,VR.[Reserved5] AS VR_Reserved5
					,VR.InsertDate AS VR_InsertDate
					,VR.InsertUser AS VR_InsertUser
					,VR.UpdateDate AS VR_UpdateDate
					,VR.UpdateUser AS VR_UpdateUser
					,@JM_LineID			LineID
					,@JM_CorCode		CorCode
					,@JM_ErpOrderNo		ErpOrderNo
					,@JM_OrderType		[OrderType]
					,@JM_MfdDate		MfdDate
					,@JM_ExpDate		ExpDate
					,@JM_LotNo			LotNo
					,@JM_LotNo_Sub		LotNo_Sub
					,@JM_Cnt_JobPlan	Cnt_JobPlan
					,@JM_DateOfTest		DateOfTest
					,@JM_DSMReportDate	DSMReportDate
					,@JM_DSMReportUser	DSMReportUser
					,@JM_Reserved1		JM_Reserved1
					,@JM_Reserved2		JM_Reserved2
					,@JM_Reserved3		JM_Reserved3
					,@JM_Reserved4		JM_Reserved4
					,@JM_Reserved5		JM_Reserved5
					,@JM_AssignDate		AssignDate
					,@JM_AssignUser		AssignUser
					,@JM_InsertDate		JM_InsertDate
					,@JM_InsertUser		JM_InsertUser
					,@JM_UpdateDate		JM_UpdateDate
					,@JM_UpdateUser		JM_UpdateUser
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
					,JD.StartUser
					,JD.StartDate
					,JD.CompleteDate
					,JD.CompleteUser
					,@PM_ProdCode	ProdCode
					,@PM_ProdName	ProdName
					,@PM_ProdName2	ProdName2
					,@PM_AGLevel	AGLevel
					,@PM_Remark		Remark
					,@PM_Exp_Day	EXP_OFFSET
					,@PM_Reserved1	PM_Reserved1
					,@PM_Reserved2	PM_Reserved2
					,@PM_Reserved3	PM_Reserved3
					,@PM_Reserved4	PM_Reserved4
					,@PM_Reserved5	PM_Reserved5
					,@PM_InsertUser	PM_InsertUser
					,@PM_InsertDate	PM_InsertDate
					,@PM_UpdateUser	PM_UpdateUser
					,@PM_UpdateDate	PM_UpdateDate
					,SP.BarcodeType
					,PD.BarcodeDataFormat
					,CD190PD.CDCode_Name SerialNumberType
					,PD.SnExpressionID
					,PD.Capacity
					,PD.LIC
					,PD.PCN
					,PD.Condition
					,PD.ProdStdCodeChild
					,PD.PackingCount
					,PD.Prefix_SSCC
					,PD.Price
					,PD.MaximumWeight
					,PD.MinimumWeight
					,PD.Reserved1 AS PD_Reserved1
					,PD.Reserved2 AS PD_Reserved2
					,PD.Reserved3 AS PD_Reserved3
					,PD.Reserved4 AS PD_Reserved4
					,PD.Reserved5 AS PD_Reserved5
					,PD.InsertDate AS PD_InsertDate
					,PD.InsertUser AS PD_InsertUser
					,PD.UpdateDate AS PD_UpdateDate
					,PD.UpdateUser AS PD_UpdateUser
					,EA_Serial
					,BX1_Serial
					,BX2_Serial
					,BX3_Serial
					,BX4_Serial
					,BX5_Serial
					,BX6_Serial
					,EA_FullBarcode_Read
					,BX1_FullBarcode_Read
					,BX2_FullBarcode_Read
					,BX3_FullBarcode_Read
					,BX4_FullBarcode_Read
					,BX5_FullBarcode_Read
					,BX6_FullBarcode_Read
					,EA_AI_FullBarcode_Read
					,BX1_AI_FullBarcode_Read
					,BX2_AI_FullBarcode_Read
					,BX3_AI_FullBarcode_Read
					,BX4_AI_FullBarcode_Read
					,BX5_AI_FullBarcode_Read
					,BX6_AI_FullBarcode_Read
					,RB.FullBarcode_Read
					,RB.AI_FullBarcode_Read
					,RB.FullBarcode_Parent
					,RB.AI_FullBarcode_Parent
					,RB.ParentProdStdCode
					,RB.ParentSerialNum
					,RB.Reserved1  RB_Reserved1
					,RB.Reserved2  RB_Reserved2
					,RB.Reserved3  RB_Reserved3
					,RB.Reserved4  RB_Reserved4
					,RB.Reserved5  RB_Reserved5
					,RB.InsertDate RB_InsertDate
					,RB.InsertUser RB_InsertUser
					,RB.UpdateDate RB_UpdateDate
					,RB.UpdateUser RB_UpdateUser
					,UI.UserName UI_UserName
					,UU.UserName UU_UserName
	FROM			#SB SB
	LEFT JOIN		#SP SP				ON SB.ProdStdCode = SP.ProdStdCode AND SB.SerialNum = SP.SerialNum
	LEFT JOIN		#VR VR				ON VR.JobDetailType = SP.JobDetailType AND VR.InsertDate = SP.InspectedDate
	LEFT JOIN		#RB RB				ON SB.ProdStdCode = RB.ProdStdCode AND SB.SerialNum = RB.SerialNum
	LEFT JOIN		CD191 CD191			ON SP.ResourceType = CD191.CDCode_Dtl
	LEFT JOIN		CD190 CD190			ON SP.SerialType   = CD190.CDCode_Dtl
	LEFT JOIN		CD193 CD193			ON SP.UseYN		   = CD193.CDCode_Dtl
	LEFT JOIN		CD192 CD192			ON COALESCE(SP.Status, RB.Status) = CD192.CDCode_Dtl
	LEFT JOIN		Dmn_JobOrder_D JD	ON JD.PlantCode = @PlantCode AND JD.OrderNo = @OrderNo AND JD.SeqNo = @SeqNo AND JD.JobDetailType = SB.JobDetailType
	LEFT JOIN		Dmn_Product_D PD	ON PD.PlantCode = @PlantCode AND PD.ProdCode = @ProdCode AND PD.JobDetailType = SB.JobDetailType
	LEFT JOIN		CD190 CD190PD		ON PD.SnType = CD190PD.CDCode_Dtl
	LEFT JOIN		Dmn_User_M UI		ON UI.PlantCode = SP.PlantCode AND UI.UserID = SP.InsertUser
	LEFT JOIN		Dmn_User_M UU		ON UU.PlantCode = SP.PlantCode AND UU.UserID = SP.UpdateUser
	ORDER BY		SB.EnJobDetail, SB.SerialNum

	DROP TABLE IF EXISTS #SP
	DROP TABLE IF EXISTS #VR
	DROP TABLE IF EXISTS #RB
	DROP TABLE IF EXISTS #RRB
	DROP TABLE IF EXISTS #RBBASE
	DROP TABLE IF EXISTS #MAP
	DROP TABLE IF EXISTS #SB
END
