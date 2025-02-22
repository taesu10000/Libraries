CREATE FUNCTION [dbo].[fn_CommonDetailName] (@CdCode VARCHAR(30), @CdDetailCode VARCHAR(50)) 
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