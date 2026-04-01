--- Consulta del usuario que más tiempo ha estado logueado:

WITH Eventos AS (
    SELECT 
        User_id,
        TipoMov,
        Fecha,
        LEAD(TipoMov) OVER (PARTITION BY User_id ORDER BY Fecha) AS next_tipo,
        LEAD(Fecha)   OVER (PARTITION BY User_id ORDER BY Fecha) AS next_fecha
    FROM LoginDb.dbo.Logins
),
Sesiones AS (
    SELECT 
        User_id,
        Fecha AS login_time,
        next_fecha AS logout_time,
        DATEDIFF(SECOND, Fecha, next_fecha) AS duracion
    FROM Eventos
    WHERE TipoMov = 1 
      AND next_tipo = 0
)
SELECT TOP 1
    User_id,
    CONCAT(
        SUM(duracion) / 86400, ' días, ',
        (SUM(duracion) % 86400) / 3600, ' horas, ',
        (SUM(duracion) % 3600) / 60, ' minutos, ',
        SUM(duracion) % 60, ' segundos'
    ) AS duracion_total
FROM Sesiones
GROUP BY User_id
ORDER BY SUM(duracion) DESC;

--- Consulta del usuario que menos tiempo ha estado logueado:

WITH Eventos AS (
    SELECT 
        User_id,
        TipoMov,
        Fecha,
        LEAD(TipoMov) OVER (PARTITION BY User_id ORDER BY Fecha) AS next_tipo,
        LEAD(Fecha)   OVER (PARTITION BY User_id ORDER BY Fecha) AS next_fecha
    FROM LoginDb.dbo.Logins
),
Sesiones AS (
    SELECT 
        User_id,
        Fecha AS login_time,
        next_fecha AS logout_time,
        DATEDIFF(SECOND, Fecha, next_fecha) AS duracion
    FROM Eventos
    WHERE TipoMov = 1 
      AND next_tipo = 0
)
SELECT TOP 1
    User_id,
    CONCAT(
        SUM(duracion) / 86400, ' días, ',
        (SUM(duracion) % 86400) / 3600, ' horas, ',
        (SUM(duracion) % 3600) / 60, ' minutos, ',
        SUM(duracion) % 60, ' segundos'
    ) AS duracion_total
FROM Sesiones
GROUP BY User_id
ORDER BY SUM(duracion) ASC;

-- Promedio de tiempo logueado por usuario por mes:

WITH Eventos AS (
    SELECT 
        User_id,
        TipoMov,
        Fecha,
        LEAD(TipoMov) OVER (PARTITION BY User_id ORDER BY Fecha) AS next_tipo,
        LEAD(Fecha)   OVER (PARTITION BY User_id ORDER BY Fecha) AS next_fecha
    FROM LoginDb.dbo.Logins
),
Sesiones AS (
    SELECT 
        User_id,
        Fecha,
        DATEDIFF(SECOND, Fecha, next_fecha) AS duracion
    FROM Eventos
    WHERE TipoMov = 1 
      AND next_tipo = 0
)
SELECT 
    User_id,
    YEAR(Fecha)  AS anio,
    MONTH(Fecha) AS mes,
    AVG(duracion) AS promedio_segundos
FROM Sesiones
GROUP BY 
    User_id,
    YEAR(Fecha),
    MONTH(Fecha)
ORDER BY User_id, anio, mes;