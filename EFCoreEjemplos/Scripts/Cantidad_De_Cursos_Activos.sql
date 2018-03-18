/****** Object:  UserDefinedFunction [dbo].[Cantidad_De_Cursos_Activos]    Script Date: 17-Mar-18 5:56:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Felipe Gavilán
-- Create date: 17-03-2018
-- Description:	Devuelve la cantidad de cursos activos que
-- lleva el estudiante
-- =============================================
CREATE FUNCTION [dbo].[Cantidad_De_Cursos_Activos]
(
	-- Add the parameters for the function here
	@EstudianteId int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result int

	-- Add the T-SQL statements to compute the return value here
	SELECT @result = count(*)
	from EstudiantesCursos
	where EstudianteId = @EstudianteId and Activo = 'true'

	-- Return the result of the function
	RETURN @result

END
GO


