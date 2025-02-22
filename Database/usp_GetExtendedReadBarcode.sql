CREATE PROCEDURE [dbo].[usp_GetExtendedReadBarcode](@argPlantCode NVARCHAR(50), @argOrderNo NVARCHAR(10), @argSeqNo NVARCHAR(4))
WITH RECOMPILE
AS
BEGIN
	DECLARE	 @PlantCode		nvarchar(50) = @argPlantCode
			,@OrderNo		nvarchar(10) = @argOrderNo
			,@SeqNo			nvarchar(4)  = @argSeqNo

	DROP TABLE IF EXISTS #RB
	DROP TABLE IF EXISTS #SP
	DROP TABLE IF EXISTS #SER

	SELECT	ProdStdCode, SerialNum, JobDetailType, FullBarcode_Read, AI_FullBarcode_Read, FullBarcode_Parent, AI_FullBarcode_Parent, ParentProdStdCode, ParentSerialNum, InsertDate
	INTO	#RB
	FROM	Dmn_ReadBarcode 
	WHERE	PlantCode = @PlantCode AND OrderNo = @OrderNo AND SeqNo = @SeqNo

	CREATE NONCLUSTERED INDEX IX_EXTRB_RB		 ON #RB(ProdStdCode, SerialNum)
	CREATE NONCLUSTERED INDEX IX_EXTRB_RB_JD	 ON #RB(ProdStdCode, SerialNum, JobDetailType)
	CREATE NONCLUSTERED INDEX IX_EXTRB_RB_Parent ON #RB(ParentProdStdCode, ParentSerialNum)

	SELECT	ProdStdCode, SerialNum, JobDetailType, UseDate 
	INTO	#SP
	FROM	Dmn_SerialPool 
	WHERE	PlantCode = @PlantCode AND OrderNo = @OrderNo AND SeqNo = @SeqNo AND Status = 'PA'

	CREATE NONCLUSTERED INDEX IX_EXTRB_SP		 ON #SP(ProdStdCode, SerialNum)
	CREATE NONCLUSTERED INDEX IX_EXTRB_SP_JD	 ON #SP(ProdStdCode, SerialNum, JobDetailType)

	SELECT	*
	INTO	#SER
	FROM
	(
		SELECT ProdStdCode, SerialNum, JobDetailType, CASE JobDetailType WHEN 'EA' THEN 0 ELSE RIGHT(JobDetailType, 1) END [EnJobDetail] FROM #SP SP
		UNION ALL SELECT ProdStdCode, SerialNum, JobDetailType, CASE JobDetailType WHEN 'EA' THEN 0 ELSE RIGHT(JobDetailType, 1) END [EnJobDetail] FROM #RB RB
	) [A]
	GROUP BY ProdStdCode, SerialNum, JobDetailType, EnJobDetail

	CREATE NONCLUSTERED INDEX IX_SB_STD_SER		ON #SER(ProdStdCode, SerialNum)
	CREATE NONCLUSTERED INDEX IX_SB_STD_SER_JDT ON #SER(ProdStdCode, SerialNum, JobDetailType)

	SELECT		 S.ProdStdCode
				,S.SerialNum
				,S.JobDetailType
				,ISNULL(UseDate, [ParentUseDate])						   [UseDate]
				,ISNULL(RB.[FullBarcode_Read],	  B.[FullBarcode_Read])	   [FullBarcode_Read]
				,ISNULL(RB.[AI_FullBarcode_Read], B.[AI_FullBarcode_Read]) [AI_FullBarcode_Read]
				,FullBarcode_Parent
				,AI_FullBarcode_Parent
				,ParentProdStdCode
				,ParentSerialNum
	FROM		#SER S
	LEFT JOIN	#SP	SP
	ON			S.ProdStdCode        = SP.ProdStdCode
				AND S.SerialNum	     = SP.SerialNum
				AND S.JobDetailType	 = SP.JobDetailType
	LEFT JOIN	#RB RB
	ON			RB.ProdStdCode       = S.ProdStdCode
				AND RB.SerialNum     = S.SerialNum
				AND RB.JobDetailType = S.JobDetailType
	OUTER APPLY
	(
		SELECT	TOP 1 FullBarcode_Parent [FullBarcode_Read], AI_FullBarcode_Parent [AI_FullBarcode_Read], InsertDate
		FROM	#RB RRB 
		WHERE	RRB.ParentProdStdCode = S.ProdStdCode AND RRB.ParentSerialNum = S.SerialNum AND RB.ProdStdCode IS NULL
		ORDER BY InsertDate DESC
	) [B]
	OUTER APPLY
	(
		SELECT	TOP 1 UseDate [ParentUseDate]
		FROM	#SP SSP
		WHERE	SSP.ProdStdCode = RB.ParentProdStdCode AND SSP.SerialNum = RB.ParentSerialNum
	) [C]
	ORDER BY	CASE S.JobDetailType WHEN 'LBL' THEN -1 WHEN 'EA' THEN 0 ELSE RIGHT(S.JobDetailType,1) END, ParentSerialNum, SerialNum

	DROP TABLE IF EXISTS #RB
	DROP TABLE IF EXISTS #SP
	DROP TABLE IF EXISTS #SER
END