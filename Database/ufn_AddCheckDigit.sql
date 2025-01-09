-- =============================================
-- Author:		Sangil.Yoon
-- Create date: 2024-07-10
-- Description:	GTIN-14 Check Digit Calculator
--
-- How to use:  SELECT [dbo].[ufn_GetCheckDigitAddedString_GTIN14] ( '3', '8803201415640' )
-- Returns:		'38803201415641'
-- Caution:		If you're not giving full representitive GTIN-14 (14 digits), you should never give @STDCODE with Indicator.
--				Cut off over than 14 digits on @STDCODE 
-- =============================================
CREATE FUNCTION [dbo].[ufn_AddCheckDigit] ( @INDICATOR NVARCHAR(1), @STDCODE NVARCHAR(14) ) RETURNS NVARCHAR(14)
AS
BEGIN
IF @INDICATOR IS NULL OR LTRIM(RTRIM(@INDICATOR)) = ''
BEGIN
	RETURN 'INDICATOR ERR'
END
ELSE IF @STDCODE IS NULL OR LTRIM(RTRIM(@STDCODE)) = ''
BEGIN
	RETURN 'STDCD ERROR'
END

DECLARE @RET NVARCHAR(14) = '', 
		@LEN INT = LEN(LTRIM(RTRIM(@STDCODE)))

IF @LEN = 12 OR @LEN = 13
BEGIN
	SET @STDCODE = @INDICATOR + LEFT(@STDCODE, 12)
END
ELSE IF @LEN = 14
BEGIN
	SET @STDCODE = @INDICATOR + SUBSTRING(@STDCODE, 2, 12)
END
ELSE IF @LEN < 12
BEGIN
	RETURN 'STDCD LEN ERR'
END

;WITH CTE AS(
	SELECT 
		CAST(0 AS INT) [DIGIT],
		CAST(0 AS INT) [TIMES],
		CAST(@STDCODE AS NVARCHAR(13)) [LEFTOVR]
	
	UNION	ALL
	SELECT	CAST(RIGHT([LEFTOVR], 1) AS INT)									[DIGIT],
			CASE [TIMES] WHEN 3 THEN 1 ELSE 3 END								[TIMES],
			CAST(SUBSTRING([LEFTOVR], 1, LEN([LEFTOVR]) - 1) AS NVARCHAR(13))	[LEFTOVR]
	FROM	CTE
	WHERE	LEN([LEFTOVR]) > 0
)
SELECT @RET = (10 - (SUM(digit * times) % 10)) % 10 FROM CTE where TIMES > 0

RETURN CONCAT(@STDCODE, @RET)
END