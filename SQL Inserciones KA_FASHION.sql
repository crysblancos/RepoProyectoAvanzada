USE KA_FASHION_BD;
GO

DECLARE @IdActivo INT = (SELECT ID_ESTADO FROM dbo.FIDE_ESTADOS_TB WHERE NOMBRE_ESTADO = N'Activo');

INSERT INTO dbo.FIDE_CATEGORIAS_TB (NOMBRE, DESCRIPCION, ID_ESTADO)
SELECT v.NOMBRE, v.DESCRIPCION, @IdActivo
FROM (VALUES
    (N'Blusas',      N'Colección femenina'),
    (N'Blazers',     N'Estilo elegante'),
    (N'Accesorios',  N'Complementa tu estilo'),
    (N'Bolsos',      N'Para cada ocasión'),
    (N'Cardigans',   N'Comodidad para tu día'),
    (N'Chalecos',    N'Diseños modernos'),
    (N'Enterizos',   N'Un conjunto completo'),
    (N'Fajas',       N'Detalles para complementar'),
    (N'Faldas',      N'Frescura y estilo'),
    (N'Gabardinas',  N'Estilo para cualquier clima'),
    (N'Gorros',      N'Accesorios para tu look'),
    (N'Jackets',     N'Moda para cada temporada'),
    (N'Jeans',       N'Un básico para combinar'),
    (N'Pantalones',  N'Comodidad y moda'),
    (N'Shorts',      N'Comodidad para días cálidos'),
    (N'Suéteres',    N'Abrigo con estilo'),
    (N'Vestidos',    N'Para cada ocasión'),
    (N'Zapatos',     N'El complemento perfecto')
) AS v(NOMBRE, DESCRIPCION)
WHERE NOT EXISTS (SELECT 1 FROM dbo.FIDE_CATEGORIAS_TB c WHERE c.NOMBRE = v.NOMBRE);
GO

DECLARE @IdActivo INT = (SELECT ID_ESTADO FROM dbo.FIDE_ESTADOS_TB WHERE NOMBRE_ESTADO = N'Activo');

INSERT INTO dbo.FIDE_SUCURSALES_TB (NOMBRE, DIRECCION, TELEFONO, HORARIO, ID_ESTADO)
SELECT v.NOMBRE, v.DIRECCION, v.TELEFONO, v.HORARIO, @IdActivo
FROM (VALUES
    (N'KA Fashion San José Centro', N'Avenida Central, San José',      N'8846-3007', N'Lunes a viernes de 9:00 a.m. a 5:00 p.m.'),
    (N'KA Fashion Heredia',         N'Centro de Heredia',              N'7238-4515', N'Lunes a viernes de 9:00 a.m. a 5:00 p.m.'),
    (N'KA Fashion Alajuela',        N'Centro de Alajuela',             N'8846-3007', N'Lunes a viernes de 9:00 a.m. a 5:00 p.m.'),
    (N'KA Fashion Cartago',         N'Centro de Cartago',              N'7238-4515', N'Lunes a viernes de 9:00 a.m. a 5:00 p.m.'),
    (N'KA Fashion Escazú',          N'Multicentro Escazú, San José',   N'8846-3007', N'Lunes a viernes de 9:00 a.m. a 5:00 p.m.'),
    (N'KA Fashion Liberia',         N'Centro de Liberia, Guanacaste',  N'7238-4515', N'Lunes a viernes de 9:00 a.m. a 5:00 p.m.')
) AS v(NOMBRE, DIRECCION, TELEFONO, HORARIO)
WHERE NOT EXISTS (SELECT 1 FROM dbo.FIDE_SUCURSALES_TB s WHERE s.NOMBRE = v.NOMBRE);
GO

DECLARE @IdActivo INT = (SELECT ID_ESTADO FROM dbo.FIDE_ESTADOS_TB WHERE NOMBRE_ESTADO = N'Activo');
DECLARE @IdCategoriaBlusas INT = (SELECT ID_CATEGORIA FROM dbo.FIDE_CATEGORIAS_TB WHERE NOMBRE = N'Blusas');

INSERT INTO dbo.FIDE_PRODUCTOS_TB
    (NOMBRE, DESCRIPCION, PRECIO, IMAGEN, TALLA, COLOR, DESTACADO, NOVEDAD, ID_CATEGORIA, ID_ESTADO)
SELECT
    v.NOMBRE,
    N'Blusa femenina de estilo moderno, cómoda y fácil de combinar. Ideal para utilizar en ocasiones casuales y actividades diarias.',
    v.PRECIO, v.IMAGEN, v.TALLA, v.COLOR, v.DESTACADO, v.NOVEDAD,
    @IdCategoriaBlusas, @IdActivo
FROM (VALUES
    (N'Blusa Rosada Unitalla',  16500.00, N'blusa1.png',  N'Unitalla', N'Rosado',     CAST(1 AS BIT), CAST(0 AS BIT)),
    (N'Blusa Blanca Unitalla',  16500.00, N'blusa2.png',  N'Unitalla', N'Blanco',     CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Blusa Beige Unitalla',   16500.00, N'blusa3.png',  N'Unitalla', N'Beige',      CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Blusa Negra Unitalla',   16500.00, N'blusa4.png',  N'Unitalla', N'Negro',      CAST(1 AS BIT), CAST(0 AS BIT)),
    (N'Blusa Beige S',          14900.00, N'blusa5.png',  N'S',        N'Beige',      CAST(0 AS BIT), CAST(1 AS BIT)),
    (N'Blusa Beige S',          14900.00, N'blusa6.png',  N'S',        N'Beige',      CAST(0 AS BIT), CAST(1 AS BIT)),
    (N'Blusa Blanca S',         14900.00, N'blusa7.png',  N'S',        N'Blanco',     CAST(0 AS BIT), CAST(1 AS BIT)),
    (N'Blusa Verde S',          14900.00, N'blusa8.png',  N'S',        N'Verde',      CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Blusa Negra S',          14900.00, N'blusa9.png',  N'S',        N'Negro',      CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Blusa Gris S',           14900.00, N'blusa10.png', N'S',        N'Gris',       CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Blusa Blanca S',         14900.00, N'blusa11.png', N'S',        N'Blanco',     CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Blusa Azul Navy S',      16500.00, N'blusa12.png', N'S',        N'Azul Navy',  CAST(1 AS BIT), CAST(0 AS BIT))
) AS v(NOMBRE, PRECIO, IMAGEN, TALLA, COLOR, DESTACADO, NOVEDAD)
WHERE NOT EXISTS (SELECT 1 FROM dbo.FIDE_PRODUCTOS_TB p WHERE p.IMAGEN = v.IMAGEN);
GO

DECLARE @IdActivo INT = (SELECT ID_ESTADO FROM dbo.FIDE_ESTADOS_TB WHERE NOMBRE_ESTADO = N'Activo');
DECLARE @IdSucursalPrincipal INT = (SELECT ID_SUCURSAL FROM dbo.FIDE_SUCURSALES_TB WHERE NOMBRE = N'KA Fashion San José Centro');

INSERT INTO dbo.FIDE_INVENTARIO_TB (TALLA, COLOR, EXISTENCIAS, FECHA_ACTUALIZACION, ID_PRODUCTO, ID_SUCURSAL, ID_ESTADO)
SELECT p.TALLA, p.COLOR, 20, GETDATE(), p.ID_PRODUCTO, @IdSucursalPrincipal, @IdActivo
FROM dbo.FIDE_PRODUCTOS_TB p
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.FIDE_INVENTARIO_TB i
    WHERE i.ID_PRODUCTO = p.ID_PRODUCTO AND i.ID_SUCURSAL = @IdSucursalPrincipal
);
GO