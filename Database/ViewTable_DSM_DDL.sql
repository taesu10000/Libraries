CREATE VIEW [dbo].[Dmn_View_DSMData] AS
SELECT SP.PlantCode
	  ,SP.ProdStdCode
      ,SP.SerialNum
	  ,SP.SerialNum AS SP_SerialNum
	  ,RB.SerialNum AS RB_Serialnum
	  ,SP.MachineID
	  ,SP.MachineID AS SP_MachineID
	  ,RB.MachineID AS RB_MachineID
      ,SP.JobDetailType
	  ,SP.OrderNo
	  ,SP.SeqNo
	  ,dbo.fn_CommonDetailName('191', SP.ResourceType) AS ResourceType
	  ,dbo.fn_CommonDetailName('190', SP.SerialType) AS SerialType
	  ,SP.Idx_Group
	  ,SP.Idx_Insert
	  ,SP.UseDate
	  ,SP.InspectedDate
	  ,dbo.fn_CommonDetailName('193', SP.UseYN) AS UseYN
	  ,dbo.fn_CommonDetailName('192', SP.Status) AS Status
	  ,SP.FileName
	  ,SP.Reserved1 AS SP_Reserved1
	  ,SP.Reserved2 AS SP_Reserved2
	  ,SP.Reserved3 AS SP_Reserved3
	  ,SP.Reserved4 AS SP_Reserved4
	  ,SP.Reserved5 AS SP_Reserved5
	  ,SP.InsertDate AS SP_InsertDate
	  ,SP.InsertUser AS SP_InsertUser
	  ,SP.UpdateDate AS SP_UpdateDate
	  ,SP.UpdateUser AS SP_UpdateUser
	  ,IsPass = Case When SP.Status = 'PA' THEN 'TRUE' ELSE 'FALSE' END
	  ,IsSample = Case When SP.Status = 'SN' OR SP.Status = 'ST' OR SP.Status = 'SS' OR SP.Status = 'SC' OR SP.Status = 'SA' THEN 'TRUE' ELSE 'FALSE' END
	  ,IsReject = Case When SP.Status = 'RE' THEN 'TRUE' ELSE 'FALSE' END
	  ,VR.DecodedBarcode
	  ,VR.Read_OCR
	  ,VR.Grade_Barcode
	  ,VR.FilePath
	  ,VR.FilePath AS DB_FilePath
	  ,VR.FilePath AS VR_FilePath
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
	  ,JD.StartUser
	  ,JD.StartDate
	  ,JD.CompleteDate
	  ,JD.CompleteUser
	  ,PM.ProdCode
	  ,PM.ProdName
	  ,PM.ProdName2
	  ,PM.AGLevel
	  ,PM.Remark
	  ,PM.Exp_Day AS EXP_OFFSET
	  ,PM.Reserved1 AS PM_Reserved1
	  ,PM.Reserved2 AS PM_Reserved2
	  ,PM.Reserved3 AS PM_Reserved3
	  ,PM.Reserved4 AS PM_Reserved4
	  ,PM.Reserved5 AS PM_Reserved5
	  ,PM.InsertDate AS PM_InsertDate
	  ,PM.InsertUser AS PM_InsertUser
	  ,PM.UpdateDate AS PM_UpdateDate
	  ,PM.UpdateUser AS PM_UpdateUser
	  ,SP.BarcodeType
	  ,PD.BarcodeDataFormat
	  ,dbo.fn_CommonDetailName('201', PD.SnType) AS SerialNumberType
	  ,PD.SnExpressionID
	  ,PD.Capacity
	  ,PD.LIC
	  ,PD.PCN
	  ,PD.Condition
	  ,PD.ProdStdCodeChild
	  ,PD.PackingCount
	  ,PD.Prefix_SSCC
	  ,PD.Reserved1 AS PD_Reserved1
	  ,PD.Reserved2 AS PD_Reserved2
	  ,PD.Reserved3 AS PD_Reserved3
	  ,PD.Reserved4 AS PD_Reserved4
	  ,PD.Reserved5 AS PD_Reserved5
	  ,PD.InsertDate AS PD_InsertDate
	  ,PD.InsertUser AS PD_InsertUser
	  ,PD.UpdateDate AS PD_UpdateDate
	  ,PD.UpdateUser AS PD_UpdateUser
--serial
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then CASE When SP.SerialNum = '' OR SP.SerialNum IS NULL Then RB.SerialNum ELSE SP.SerialNum END
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then STUFF((SELECT ';' + SerialNum FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then STUFF((SELECT ';' + tmp1.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then STUFF((SELECT ';' + tmp2.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + tmp3.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + tmp4.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp4 ON tmp3.PlantCode = tmp4.PlantCode AND tmp3.ProdStdCode = tmp4.ParentProdStdCode AND tmp3.SerialNum = tmp4.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + tmp5.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp4 ON tmp3.PlantCode = tmp4.PlantCode AND tmp3.ProdStdCode = tmp4.ParentProdStdCode AND tmp3.SerialNum = tmp4.ParentSerialNum
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp5 ON tmp4.PlantCode = tmp5.PlantCode AND tmp4.ProdStdCode = tmp5.ParentProdStdCode AND tmp4.SerialNum = tmp5.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
		END) AS EA_Serial
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then RB.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then SP.SerialNum
	 		 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then STUFF((SELECT ';' + SerialNum FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then STUFF((SELECT ';' + tmp1.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + tmp2.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + tmp3.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + tmp4.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp4 ON tmp3.PlantCode = tmp4.PlantCode AND tmp3.ProdStdCode = tmp4.ParentProdStdCode AND tmp3.SerialNum = tmp4.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')

		END) AS BX1_Serial
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA'  Then RB10.ParentSerialNum 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then RB.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then SP.SerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then STUFF((SELECT ';' + SerialNum FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + tmp1.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 							where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when  CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + tmp2.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 										LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
			 							where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when  CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + tmp3.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 										LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
			 										LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
			 										where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
		END) AS BX2_Serial
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then RB10.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then RB00.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then RB.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then SP.SerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + SerialNum FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + tmp1.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 						where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + tmp2.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 									LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
			 									where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
		END) AS BX3_Serial
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then RB20.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then RB10.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then RB00.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then RB.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then SP.SerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + SerialNum FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + tmp1.SerialNum FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
		END) AS BX4_Serial
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then RB30.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then RB20.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then RB10.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then RB00.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then RB.ParentSerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then SP.SerialNum
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + SerialNum FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
		END) AS BX5_Serial
	  ,(case when SP.JobDetailType = 'EA' Then RB40.ParentSerialNum
			 when SP.JobDetailType = 'BX1' Then RB30.ParentSerialNum
			 when SP.JobDetailType = 'BX2' Then RB20.ParentSerialNum
			 when SP.JobDetailType = 'BX3' Then RB10.ParentSerialNum
			 when SP.JobDetailType = 'BX4' Then RB00.ParentSerialNum
			 when SP.JobDetailType = 'BX5' Then RB.ParentSerialNum
			 when SP.JobDetailType = 'BX6' Then SP.SerialNum
		END) AS BX6_Serial

--FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then REPLACE(RB.FullBarcode_Read, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then STUFF((SELECT ';' + REPLACE(FullBarcode_Read, char(29),'') FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then STUFF((SELECT ';' + REPLACE(tmp1.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then STUFF((SELECT ';' + REPLACE(tmp2.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + REPLACE(tmp3.FullBarcode_Read, char(29),'') FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + REPLACE(tmp4.FullBarcode_Read, char(29),'') FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp4 ON tmp3.PlantCode = tmp4.PlantCode AND tmp3.ProdStdCode = tmp4.ParentProdStdCode AND tmp3.SerialNum = tmp4.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + REPLACE(tmp5.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp4 ON tmp3.PlantCode = tmp4.PlantCode AND tmp3.ProdStdCode = tmp4.ParentProdStdCode AND tmp3.SerialNum = tmp4.ParentSerialNum
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp5 ON tmp4.PlantCode = tmp5.PlantCode AND tmp4.ProdStdCode = tmp5.ParentProdStdCode AND tmp4.SerialNum = tmp5.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
		END) AS EA_FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then REPLACE(RB.FullBarcode_Parent, char(29),'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then REPLACE(RB.FullBarcode_Read, char(29),'') 
	 		 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then STUFF((SELECT ';' + REPLACE(FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then STUFF((SELECT ';' + REPLACE(tmp1.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + REPLACE(tmp2.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + REPLACE(tmp3.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + REPLACE(tmp4.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp4 ON tmp3.PlantCode = tmp4.PlantCode AND tmp3.ProdStdCode = tmp4.ParentProdStdCode AND tmp3.SerialNum = tmp4.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')

		END) AS BX1_FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA'  Then REPLACE(RB10.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then REPLACE(RB.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then REPLACE(RB.FullBarcode_Read, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then STUFF((SELECT ';' + REPLACE(FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + REPLACE(tmp1.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 							where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when  CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + REPLACE(tmp2.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 							LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
			 							where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when  CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + REPLACE(tmp3.FullBarcode_Read, char(29),'') FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 									LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
			 									LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
			 									where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
		END) AS BX2_FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then REPLACE(RB10.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then REPLACE(RB00.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then REPLACE(RB.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then REPLACE(RB.FullBarcode_Read, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + REPLACE(FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + REPLACE(tmp1.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 						where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + REPLACE(tmp2.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 									LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
			 									where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
		END) AS BX3_FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then REPLACE(RB20.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then REPLACE(RB10.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then REPLACE(RB00.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then REPLACE(RB.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then REPLACE(RB.FullBarcode_Read, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + REPLACE(FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + REPLACE(tmp1.FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
		END) AS BX4_FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then REPLACE(RB30.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then REPLACE(RB20.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then REPLACE(RB10.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then REPLACE(RB00.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then REPLACE(RB.FullBarcode_Parent, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then REPLACE(RB.FullBarcode_Read, char(29),'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + REPLACE(FullBarcode_Read, char(29),'')  FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
		END) AS BX5_FullBarcode_Read
	  ,(case when SP.JobDetailType = 'EA' Then RB40.FullBarcode_Parent
			 when SP.JobDetailType = 'BX1' Then RB30.FullBarcode_Parent
			 when SP.JobDetailType = 'BX2' Then RB20.FullBarcode_Parent
			 when SP.JobDetailType = 'BX3' Then RB10.FullBarcode_Parent
			 when SP.JobDetailType = 'BX4' Then RB00.FullBarcode_Parent
			 when SP.JobDetailType = 'BX5' Then RB.FullBarcode_Parent
			 when SP.JobDetailType = 'BX6' Then RB.FullBarcode_Read
		END) AS BX6_FullBarcode_Read

		--AI_FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then RB.AI_FullBarcode_Read
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then STUFF((SELECT ';' + AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then STUFF((SELECT ';' + tmp1.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then STUFF((SELECT ';' + tmp2.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + tmp3.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + tmp4.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp4 ON tmp3.PlantCode = tmp4.PlantCode AND tmp3.ProdStdCode = tmp4.ParentProdStdCode AND tmp3.SerialNum = tmp4.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + tmp5.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp4 ON tmp3.PlantCode = tmp4.PlantCode AND tmp3.ProdStdCode = tmp4.ParentProdStdCode AND tmp3.SerialNum = tmp4.ParentSerialNum
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp5 ON tmp4.PlantCode = tmp5.PlantCode AND tmp4.ProdStdCode = tmp5.ParentProdStdCode AND tmp4.SerialNum = tmp5.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
		END) AS EA_AI_FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then RB.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then RB.AI_FullBarcode_Read
	 		 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then STUFF((SELECT ';' + AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then STUFF((SELECT ';' + tmp1.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + tmp2.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + tmp3.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + tmp4.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
												    LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
													LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp4 ON tmp3.PlantCode = tmp4.PlantCode AND tmp3.ProdStdCode = tmp4.ParentProdStdCode AND tmp3.SerialNum = tmp4.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')

		END) AS BX1_AI_FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA'  Then RB10.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then RB.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then RB.AI_FullBarcode_Read
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then STUFF((SELECT ';' + AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + tmp1.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 							where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when  CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + tmp2.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 							LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
			 							where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
			 when  CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + tmp3.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 									LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
			 									LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp3 ON tmp2.PlantCode = tmp3.PlantCode AND tmp2.ProdStdCode = tmp3.ParentProdStdCode AND tmp2.SerialNum = tmp3.ParentSerialNum												   
			 									where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
		END) AS BX2_AI_FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then RB10.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then RB00.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then RB.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then RB.AI_FullBarcode_Read
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then STUFF((SELECT ';' + AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + tmp1.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 						where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + tmp2.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
			 									LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp2 ON tmp1.PlantCode = tmp2.PlantCode AND tmp1.ProdStdCode = tmp2.ParentProdStdCode AND tmp1.SerialNum = tmp2.ParentSerialNum												   
			 									where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'')
		END) AS BX3_AI_FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then RB20.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then RB10.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then RB00.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then RB.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then RB.AI_FullBarcode_Read
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then STUFF((SELECT ';' + AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + tmp1.AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp LEFT OUTER JOIN dbo.Dmn_ReadBarcode tmp1 ON tmp.PlantCode = tmp1.PlantCode AND tmp.ProdStdCode = tmp1.ParentProdStdCode AND tmp.SerialNum = tmp1.ParentSerialNum
													where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
		END) AS BX4_AI_FullBarcode_Read
	  ,(case when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'EA' Then RB30.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX1' Then RB20.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX2' Then RB10.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX3' Then RB00.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX4' Then RB.AI_FullBarcode_Parent
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX5' Then RB.AI_FullBarcode_Read
			 when CASE When SP.JobDetailType = '' OR SP.JobDetailType IS NULL Then RB.JobDetailType ELSE SP.JobDetailType END = 'BX6' Then STUFF((SELECT ';' + AI_FullBarcode_Read FROM dbo.Dmn_ReadBarcode tmp where SP.PlantCode = tmp.PlantCode AND SP.ProdStdCode = tmp.ParentProdStdCode AND SP.SerialNum = tmp.ParentSerialNum FOR XML PATH('')),1,1,'') 
		END) AS BX5_AI_FullBarcode_Read
	  ,(case when SP.JobDetailType = 'EA'  Then RB40.AI_FullBarcode_Parent
			 when SP.JobDetailType = 'BX1' Then RB30.AI_FullBarcode_Parent
			 when SP.JobDetailType = 'BX2' Then RB20.AI_FullBarcode_Parent
			 when SP.JobDetailType = 'BX3' Then RB10.AI_FullBarcode_Parent
			 when SP.JobDetailType = 'BX4' Then RB00.AI_FullBarcode_Parent
			 when SP.JobDetailType = 'BX5' Then RB.AI_FullBarcode_Parent
			 when SP.JobDetailType = 'BX6' Then RB.AI_FullBarcode_Read
		END) AS BX6_AI_FullBarcode_Read
	,RB.FullBarcode_Read 
	,RB.AI_FullBarcode_Read
	,RB.FullBarcode_Parent
	,RB.AI_FullBarcode_Parent
	,RB.ParentProdStdCode
	,RB.ParentSerialNum
	,RB.Reserved1 AS RB_Reserved1
	,RB.Reserved2 AS RB_Reserved2
	,RB.Reserved3 AS RB_Reserved3
	,RB.Reserved4 AS RB_Reserved4
	,RB.Reserved5 AS RB_Reserved5
	,RB.InsertDate AS RB_InsertDate
	,RB.InsertUser AS RB_InsertUser
	,RB.UpdateDate AS RB_UpdateDate
	,RB.UpdateUser AS RB_UpdateUser
	,(SELECT UserName from Dmn_User_M UM where SP.PlantCode = UM.PlantCode AND SP.InsertUser = UM.UserID) AS UI_UserName 
	,(SELECT UserName from Dmn_User_M UM where SP.PlantCode = UM.PlantCode AND SP.UpdateUser = UM.UserID) AS UU_UserName

  FROM [DSM].[dbo].Dmn_Serialpool AS SP
  FULL OUTER JOIN [Dmn_ReadBarcode] AS RB (NOLOCK) ON SP.PlantCode = RB.PlantCode AND SP.ProdStdCode = RB.ProdStdCode AND SP.SerialNum = RB.SerialNum 
  LEFT OUTER JOIN [Dmn_ReadBarcode] AS RB00 (NOLOCK) ON SP.PlantCode = RB00.PlantCode AND SP.ProdStdCode = RB00.ProdStdCode AND SP.SerialNum = RB00.SerialNum AND SP.JobDetailType = 'EA'
  LEFT OUTER JOIN [Dmn_ReadBarcode] AS RB10 (NOLOCK) ON SP.PlantCode = RB10.PlantCode AND RB00.ParentProdStdCode = RB10.ProdStdCode AND RB00.ParentSerialNum = RB10.SerialNum
  LEFT OUTER JOIN [Dmn_ReadBarcode] AS RB20 (NOLOCK) ON SP.PlantCode = RB20.PlantCode AND RB10.ParentProdStdCode = RB20.ProdStdCode AND RB10.ParentSerialNum = RB20.SerialNum
  LEFT OUTER JOIN [Dmn_ReadBarcode] AS RB30 (NOLOCK) ON SP.PlantCode = RB30.PlantCode AND RB20.ParentProdStdCode = RB30.ProdStdCode AND RB20.ParentSerialNum = RB30.SerialNum
  LEFT OUTER JOIN [Dmn_ReadBarcode] AS RB40 (NOLOCK) ON SP.PlantCode = RB40.PlantCode AND RB30.ParentProdStdCode = RB40.ProdStdCode AND RB30.ParentSerialNum = RB40.SerialNum		
  LEFT OUTER JOIN DSM.dbo.Dmn_JobOrder_M AS JM (NOLOCK) ON SP.PlantCode = JM.PlantCode AND SP.OrderNo = JM.OrderNo AND SP.SeqNo = JM.SeqNo
  LEFT OUTER JOIN DSM.dbo.Dmn_JobOrder_D AS JD (NOLOCK) ON SP.PlantCode = JD.PlantCode AND SP.OrderNo = JD.OrderNo AND SP.SeqNo = JD.SeqNo AND JD.JobDetailType = SP.JobDetailType 
  LEFT OUTER JOIN DSM.dbo.Dmn_Product_M AS PM (NOLOCK) ON SP.PlantCode = PM.PlantCode AND JM.ProdCode = PM.ProdCode 
  LEFT OUTER JOIN DSM.dbo.Dmn_Product_D AS PD (NOLOCK) ON SP.PlantCode = PD.PlantCode AND JM.ProdCode = PD.ProdCode AND JD.JobDetailType = PD.JobDetailType
  LEFT OUTER JOIN DSM.dbo.Dmn_VisionResult AS VR (NOLOCK) ON SP.PlantCode = VR.PlantCode AND SP.OrderNo = VR.OrderNo AND SP.SeqNo = VR.SeqNo AND SP.JobDetailType = VR.JobDetailType AND SP.InspectedDate = VR.InsertDate

