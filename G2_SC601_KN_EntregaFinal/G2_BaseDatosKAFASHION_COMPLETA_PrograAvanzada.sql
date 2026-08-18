USE master;
GO

IF DB_ID('KA_FASHION_BD') IS NOT NULL
BEGIN
    ALTER DATABASE KA_FASHION_BD
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

    DROP DATABASE KA_FASHION_BD;
END
GO

CREATE DATABASE KA_FASHION_BD;
GO

USE KA_FASHION_BD;
GO

CREATE TABLE [dbo].[tbRol](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
 CONSTRAINT [PK_tbRol] PRIMARY KEY CLUSTERED ([Consecutivo] ASC)
)
GO

CREATE TABLE [dbo].[tbUsuario](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Identificacion] [varchar](15) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Apellido1] [varchar](100) NULL,
	[Apellido2] [varchar](100) NULL,
	[CorreoElectronico] [varchar](100) NOT NULL,
	[Telefono] [varchar](20) NULL,
	[Contrasenna] [varchar](20) NOT NULL,
	[Estado] [bit] NOT NULL DEFAULT(1),
	[TieneContrasennaTemp] [bit] NOT NULL DEFAULT(0),
	[VigenciaContrasennaTemp] [datetime] NULL,
	[ConsecutivoRol] [int] NOT NULL,
 CONSTRAINT [PK_tbUsuario] PRIMARY KEY CLUSTERED ([Consecutivo] ASC)
)
GO

CREATE TABLE [dbo].[tbError](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Mensaje] [varchar](max) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
	[Lugar] [varchar](50) NOT NULL,
	[ConsecutivoUsuario] [int] NOT NULL,
 CONSTRAINT [PK_tbError] PRIMARY KEY CLUSTERED ([Consecutivo] ASC)
)
GO

ALTER TABLE [dbo].[tbUsuario]
ADD CONSTRAINT [UK_tbUsuario_Correo] UNIQUE ([CorreoElectronico])
GO

ALTER TABLE [dbo].[tbUsuario]
ADD CONSTRAINT [UK_tbUsuario_Identificacion] UNIQUE ([Identificacion])
GO

ALTER TABLE [dbo].[tbUsuario] WITH CHECK
ADD CONSTRAINT [FK_tbUsuario_tbRol] FOREIGN KEY([ConsecutivoRol])
REFERENCES [dbo].[tbRol] ([Consecutivo])
GO

-- Roles base
INSERT INTO [dbo].[tbRol] (Nombre) VALUES ('Cliente')
INSERT INTO [dbo].[tbRol] (Nombre) VALUES ('Administrador')
GO

-- Usuario de prueba (contraseña: Prueba123!)
INSERT INTO [dbo].[tbUsuario]
(Identificacion, Nombre, Apellido1, Apellido2, CorreoElectronico, Telefono, Contrasenna, Estado, TieneContrasennaTemp, ConsecutivoRol)
VALUES
('101110111', 'María', 'Rodríguez', 'Solís', 'maria.rodriguez@correo.com', '88880000', 'Prueba123!', 1, 0, 1)
GO

-- Creacion de las otras tablas
IF OBJECT_ID('dbo.FIDE_ESTADOS_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_ESTADOS_TB (
    ID_ESTADO INT IDENTITY(1,1) NOT NULL,
    NOMBRE_ESTADO NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_FIDE_ESTADOS_TB PRIMARY KEY (ID_ESTADO)
);
GO

IF OBJECT_ID('dbo.FIDE_CATEGORIAS_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_CATEGORIAS_TB (
    ID_CATEGORIA INT IDENTITY(1,1) NOT NULL,
    NOMBRE NVARCHAR(100) NOT NULL,
    DESCRIPCION NVARCHAR(255) NULL,
    ID_ESTADO INT NOT NULL,
    CONSTRAINT PK_FIDE_CATEGORIAS_TB PRIMARY KEY (ID_CATEGORIA),
    CONSTRAINT FK_CATEGORIAS_ESTADO FOREIGN KEY (ID_ESTADO) REFERENCES dbo.FIDE_ESTADOS_TB(ID_ESTADO)
);
GO

IF OBJECT_ID('dbo.FIDE_SUCURSALES_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_SUCURSALES_TB (
    ID_SUCURSAL INT IDENTITY(1,1) NOT NULL,
    NOMBRE NVARCHAR(100) NOT NULL,
    DIRECCION NVARCHAR(255) NULL,
    TELEFONO NVARCHAR(20) NULL,
    HORARIO NVARCHAR(100) NULL,
    ID_ESTADO INT NOT NULL,
    CONSTRAINT PK_FIDE_SUCURSALES_TB PRIMARY KEY (ID_SUCURSAL),
    CONSTRAINT FK_SUCURSALES_ESTADO FOREIGN KEY (ID_ESTADO) REFERENCES dbo.FIDE_ESTADOS_TB(ID_ESTADO)
);
GO

IF OBJECT_ID('dbo.FIDE_PRODUCTOS_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_PRODUCTOS_TB (
    ID_PRODUCTO INT IDENTITY(1,1) NOT NULL,
    NOMBRE NVARCHAR(150) NOT NULL,
    DESCRIPCION NVARCHAR(500) NULL,
    PRECIO DECIMAL(18,2) NOT NULL,
    IMAGEN NVARCHAR(255) NULL,
    TALLA NVARCHAR(20) NULL,
    COLOR NVARCHAR(50) NULL,
    DESTACADO BIT NOT NULL DEFAULT (0),
    NOVEDAD BIT NOT NULL DEFAULT (0),
    ID_CATEGORIA INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    CONSTRAINT PK_FIDE_PRODUCTOS_TB PRIMARY KEY (ID_PRODUCTO),
    CONSTRAINT FK_PRODUCTOS_CATEGORIA FOREIGN KEY (ID_CATEGORIA) REFERENCES dbo.FIDE_CATEGORIAS_TB(ID_CATEGORIA),
    CONSTRAINT FK_PRODUCTOS_ESTADO FOREIGN KEY (ID_ESTADO) REFERENCES dbo.FIDE_ESTADOS_TB(ID_ESTADO)
);
GO

IF OBJECT_ID('dbo.FIDE_INVENTARIO_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_INVENTARIO_TB (
    ID_INVENTARIO INT IDENTITY(1,1) NOT NULL,
    TALLA NVARCHAR(20) NULL,
    COLOR NVARCHAR(50) NULL,
    EXISTENCIAS INT NOT NULL DEFAULT (0),
    FECHA_ACTUALIZACION DATETIME NOT NULL DEFAULT (GETDATE()),
    ID_PRODUCTO INT NOT NULL,
    ID_SUCURSAL INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    CONSTRAINT PK_FIDE_INVENTARIO_TB PRIMARY KEY (ID_INVENTARIO),
    CONSTRAINT FK_INVENTARIO_PRODUCTO FOREIGN KEY (ID_PRODUCTO) REFERENCES dbo.FIDE_PRODUCTOS_TB(ID_PRODUCTO),
    CONSTRAINT FK_INVENTARIO_SUCURSAL FOREIGN KEY (ID_SUCURSAL) REFERENCES dbo.FIDE_SUCURSALES_TB(ID_SUCURSAL),
    CONSTRAINT FK_INVENTARIO_ESTADO FOREIGN KEY (ID_ESTADO) REFERENCES dbo.FIDE_ESTADOS_TB(ID_ESTADO)
);
GO

IF OBJECT_ID('dbo.FIDE_CARRITOS_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_CARRITOS_TB (
    ID_CARRITO INT IDENTITY(1,1) NOT NULL,
    FECHA_CREACION DATETIME NOT NULL DEFAULT (GETDATE()),
    ID_USUARIO INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    CONSTRAINT PK_FIDE_CARRITOS_TB PRIMARY KEY (ID_CARRITO),
    CONSTRAINT FK_CARRITOS_USUARIO FOREIGN KEY (ID_USUARIO) REFERENCES dbo.tbUsuario(Consecutivo),
    CONSTRAINT FK_CARRITOS_ESTADO FOREIGN KEY (ID_ESTADO) REFERENCES dbo.FIDE_ESTADOS_TB(ID_ESTADO)
);
GO

IF OBJECT_ID('dbo.FIDE_CARRITO_DETALLES_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_CARRITO_DETALLES_TB (
    ID_DETALLE_CARRITO INT IDENTITY(1,1) NOT NULL,
    CANTIDAD INT NOT NULL,
    TALLA NVARCHAR(20) NULL,
    COLOR NVARCHAR(50) NULL,
    ID_CARRITO INT NOT NULL,
    ID_PRODUCTO INT NOT NULL,
    CONSTRAINT PK_FIDE_CARRITO_DETALLES_TB PRIMARY KEY (ID_DETALLE_CARRITO),
    CONSTRAINT FK_CARRITODET_CARRITO FOREIGN KEY (ID_CARRITO) REFERENCES dbo.FIDE_CARRITOS_TB(ID_CARRITO),
    CONSTRAINT FK_CARRITODET_PRODUCTO FOREIGN KEY (ID_PRODUCTO) REFERENCES dbo.FIDE_PRODUCTOS_TB(ID_PRODUCTO)
);
GO

IF OBJECT_ID('dbo.FIDE_PEDIDOS_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_PEDIDOS_TB (
    ID_PEDIDO INT IDENTITY(1,1) NOT NULL,
    METODO_ENTREGA NVARCHAR(50) NULL,
    OBSERVACIONES NVARCHAR(255) NULL,
    TOTAL DECIMAL(18,2) NOT NULL,
    FECHA_PEDIDO DATETIME NOT NULL DEFAULT (GETDATE()),
    ID_USUARIO INT NOT NULL,
    ID_SUCURSAL INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    CONSTRAINT PK_FIDE_PEDIDOS_TB PRIMARY KEY (ID_PEDIDO),
    CONSTRAINT FK_PEDIDOS_USUARIO FOREIGN KEY (ID_USUARIO) REFERENCES dbo.tbUsuario(Consecutivo),
    CONSTRAINT FK_PEDIDOS_SUCURSAL FOREIGN KEY (ID_SUCURSAL) REFERENCES dbo.FIDE_SUCURSALES_TB(ID_SUCURSAL),
    CONSTRAINT FK_PEDIDOS_ESTADO FOREIGN KEY (ID_ESTADO) REFERENCES dbo.FIDE_ESTADOS_TB(ID_ESTADO)
);
GO

IF OBJECT_ID('dbo.FIDE_PEDIDOS_DETALLE_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_PEDIDOS_DETALLE_TB (
    ID_DETALLE INT IDENTITY(1,1) NOT NULL,
    CANTIDAD INT NOT NULL,
    TALLA NVARCHAR(20) NULL,
    COLOR NVARCHAR(50) NULL,
    PRECIO_UNITARIO DECIMAL(18,2) NOT NULL,
    SUBTOTAL DECIMAL(18,2) NOT NULL,
    ID_PEDIDO INT NOT NULL,
    ID_PRODUCTO INT NOT NULL,
    CONSTRAINT PK_FIDE_PEDIDOS_DETALLE_TB PRIMARY KEY (ID_DETALLE),
    CONSTRAINT FK_PEDIDOSDET_PEDIDO FOREIGN KEY (ID_PEDIDO) REFERENCES dbo.FIDE_PEDIDOS_TB(ID_PEDIDO),
    CONSTRAINT FK_PEDIDOSDET_PRODUCTO FOREIGN KEY (ID_PRODUCTO) REFERENCES dbo.FIDE_PRODUCTOS_TB(ID_PRODUCTO)
);
GO

IF OBJECT_ID('dbo.FIDE_PROMOCIONES_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_PROMOCIONES_TB (
    ID_PROMOCION INT IDENTITY(1,1) NOT NULL,
    NOMBRE NVARCHAR(100) NOT NULL,
    DESCRIPCION NVARCHAR(255) NULL,
    DESCUENTO DECIMAL(18,2) NOT NULL,
    FECHA_INICIO DATETIME NOT NULL,
    FECHA_FIN DATETIME NOT NULL,
    ID_ESTADO INT NOT NULL,
    CONSTRAINT PK_FIDE_PROMOCIONES_TB PRIMARY KEY (ID_PROMOCION),
    CONSTRAINT FK_PROMOCIONES_ESTADO FOREIGN KEY (ID_ESTADO) REFERENCES dbo.FIDE_ESTADOS_TB(ID_ESTADO)
);
GO

IF OBJECT_ID('dbo.FIDE_PROMO_PRODUCTOS_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_PROMO_PRODUCTOS_TB (
    ID_PRODUCTO INT NOT NULL,
    ID_PROMOCION INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    CONSTRAINT PK_FIDE_PROMO_PRODUCTOS_TB PRIMARY KEY (ID_PRODUCTO, ID_PROMOCION),
    CONSTRAINT FK_PROMOPROD_PRODUCTO FOREIGN KEY (ID_PRODUCTO) REFERENCES dbo.FIDE_PRODUCTOS_TB(ID_PRODUCTO),
    CONSTRAINT FK_PROMOPROD_PROMOCION FOREIGN KEY (ID_PROMOCION) REFERENCES dbo.FIDE_PROMOCIONES_TB(ID_PROMOCION),
    CONSTRAINT FK_PROMOPROD_ESTADO FOREIGN KEY (ID_ESTADO) REFERENCES dbo.FIDE_ESTADOS_TB(ID_ESTADO)
);
GO

IF OBJECT_ID('dbo.[FIDE_RESEÑAS_TB]', 'U') IS NULL
CREATE TABLE dbo.[FIDE_RESEÑAS_TB] (
    [ID_RESEÑA] INT IDENTITY(1,1) NOT NULL,
    CALIFICACION INT NOT NULL,
    COMENTARIO NVARCHAR(500) NULL,
    FECHA DATETIME NOT NULL DEFAULT (GETDATE()),
    ID_USUARIO INT NOT NULL,
    ID_PRODUCTO INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    CONSTRAINT [PK_FIDE_RESEÑAS_TB] PRIMARY KEY ([ID_RESEÑA]),
    CONSTRAINT FK_RESENAS_USUARIO FOREIGN KEY (ID_USUARIO) REFERENCES dbo.tbUsuario(Consecutivo),
    CONSTRAINT FK_RESENAS_PRODUCTO FOREIGN KEY (ID_PRODUCTO) REFERENCES dbo.FIDE_PRODUCTOS_TB(ID_PRODUCTO),
    CONSTRAINT FK_RESENAS_ESTADO FOREIGN KEY (ID_ESTADO) REFERENCES dbo.FIDE_ESTADOS_TB(ID_ESTADO),
    CONSTRAINT CK_RESENAS_CALIFICACION CHECK (CALIFICACION BETWEEN 1 AND 5)
);
GO

IF OBJECT_ID('dbo.FIDE_CONTACTOS_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_CONTACTOS_TB (
    ID_CONTACTO INT IDENTITY(1,1) NOT NULL,
    NOMBRE NVARCHAR(150) NOT NULL,
    CORREO NVARCHAR(150) NOT NULL,
    FECHA DATETIME NOT NULL DEFAULT (GETDATE()),
    ASUNTO NVARCHAR(150) NULL,
    MENSAJE NVARCHAR(1000) NULL,
    ID_ESTADO INT NOT NULL,
    CONSTRAINT PK_FIDE_CONTACTOS_TB PRIMARY KEY (ID_CONTACTO),
    CONSTRAINT FK_CONTACTOS_ESTADO FOREIGN KEY (ID_ESTADO) REFERENCES dbo.FIDE_ESTADOS_TB(ID_ESTADO)
);
GO

IF OBJECT_ID('dbo.FIDE_PERSONAL_SHOPPER_TB', 'U') IS NULL
CREATE TABLE dbo.FIDE_PERSONAL_SHOPPER_TB (
    ID_SOLICITUD INT IDENTITY(1,1) NOT NULL,
    ESTILO_BUSCADO NVARCHAR(150) NULL,
    TALLA NVARCHAR(20) NULL,
    PRESUPUESTO DECIMAL(18,2) NOT NULL,
    NECESIDADES NVARCHAR(500) NULL,
    FECHA DATETIME NOT NULL DEFAULT (GETDATE()),
    ID_USUARIO INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    CONSTRAINT PK_FIDE_PERSONAL_SHOPPER_TB PRIMARY KEY (ID_SOLICITUD),
    CONSTRAINT FK_PSHOPPER_USUARIO FOREIGN KEY (ID_USUARIO) REFERENCES dbo.tbUsuario(Consecutivo),
    CONSTRAINT FK_PSHOPPER_ESTADO FOREIGN KEY (ID_ESTADO) REFERENCES dbo.FIDE_ESTADOS_TB(ID_ESTADO)
);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.FIDE_ESTADOS_TB WHERE NOMBRE_ESTADO = N'Activo')
    INSERT INTO dbo.FIDE_ESTADOS_TB (NOMBRE_ESTADO) VALUES (N'Activo');

IF NOT EXISTS (SELECT 1 FROM dbo.FIDE_ESTADOS_TB WHERE NOMBRE_ESTADO = N'Inactivo')
    INSERT INTO dbo.FIDE_ESTADOS_TB (NOMBRE_ESTADO) VALUES (N'Inactivo');
GO


-- Insert de las categorias
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


-- Insert de las sucursales
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


-- Insert de las blusas
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


USE KA_FASHION_BD;
GO


-- Insert de los productos nuevos
DECLARE @IdActivo INT;

SELECT @IdActivo = ID_ESTADO
FROM dbo.FIDE_ESTADOS_TB
WHERE NOMBRE_ESTADO = N'Activo';


INSERT INTO dbo.FIDE_PRODUCTOS_TB
(
    NOMBRE,
    DESCRIPCION,
    PRECIO,
    IMAGEN,
    TALLA,
    COLOR,
    DESTACADO,
    NOVEDAD,
    ID_CATEGORIA,
    ID_ESTADO
)
SELECT
    p.NOMBRE,
    p.DESCRIPCION,
    p.PRECIO,
    p.IMAGEN,
    p.TALLA,
    p.COLOR,
    p.DESTACADO,
    p.NOVEDAD,
    c.ID_CATEGORIA,
    @IdActivo
FROM
(
    VALUES

    (N'Pantalón Beige Casual',
     N'Pantalón femenino cómodo y moderno, ideal para uso casual.',
     19900.00, N'pantalon1.jpg', N'M', N'Beige',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Pantalones'),

    (N'Pantalón Negro Formal',
     N'Pantalón femenino de estilo formal, ideal para oficina o eventos.',
     22900.00, N'pantalon2.jpg', N'M', N'Negro',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Pantalones'),

    (N'Pantalón Blanco Elegante',
     N'Pantalón blanco femenino fácil de combinar.',
     21900.00, N'pantalon4.jpg', N'M', N'Blanco',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Pantalones'),


    (N'Vestido Rojo Elegante',
     N'Vestido femenino elegante ideal para eventos y ocasiones especiales.',
     29900.00, N'vestido1.jpg', N'M', N'Rojo',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Vestidos'),

    (N'Vestido Negro Casual',
     N'Vestido negro moderno y fácil de combinar.',
     25900.00, N'vestido2.jpg', N'S', N'Negro',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Vestidos'),

    (N'Vestido Floral',
     N'Vestido femenino con diseño floral para ocasiones casuales.',
     27900.00, N'vestido3.jpg', N'M', N'Floral',
     CAST(0 AS BIT), CAST(1 AS BIT), N'Vestidos'),

    (N'Vestido Verde',
     N'Vestido femenino de diseño moderno y fresco.',
     26900.00, N'vestido4.jpg', N'L', N'Verde',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Vestidos'),

    (N'Vestido Beige',
     N'Vestido elegante en tono beige para diferentes ocasiones.',
     28900.00, N'vestido5.jpg', N'M', N'Beige',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Vestidos'),


    (N'Jean Azul Clásico',
     N'Jean femenino clásico cómodo para uso diario.',
     24900.00, N'jean1.jpg', N'M', N'Azul',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Jeans'),

    (N'Jean Azul Claro',
     N'Jean femenino en tono azul claro con corte moderno.',
     24900.00, N'jean2.jpg', N'S', N'Azul Claro',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Jeans'),

    (N'Jean Negro',
     N'Jean negro femenino fácil de combinar.',
     25900.00, N'jean3.jpg', N'M', N'Negro',
     CAST(0 AS BIT), CAST(1 AS BIT), N'Jeans'),

    (N'Jean Skinny Azul',
     N'Jean femenino de corte ajustado y estilo moderno.',
     26900.00, N'jean4.jpg', N'L', N'Azul',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Jeans'),

    (N'Jean Mom Fit',
     N'Jean femenino de corte relajado para mayor comodidad.',
     27500.00, N'jean5.jpg', N'M', N'Azul',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Jeans'),


    (N'Falda Negra',
     N'Falda femenina clásica para ocasiones casuales y formales.',
     18500.00, N'falda1.jpg', N'M', N'Negro',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Faldas'),

    (N'Falda Beige',
     N'Falda femenina elegante en tono beige.',
     18900.00, N'falda2.jpg', N'S', N'Beige',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Faldas'),

    (N'Falda Floral',
     N'Falda femenina fresca con diseño floral.',
     19500.00, N'falda3.jpg', N'M', N'Floral',
     CAST(0 AS BIT), CAST(1 AS BIT), N'Faldas'),

    (N'Falda Roja',
     N'Falda femenina moderna en color rojo.',
     19900.00, N'falda4.jpg', N'L', N'Rojo',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Faldas'),

    (N'Falda Denim',
     N'Falda de mezclilla para un estilo casual.',
     20500.00, N'falda5.jpg', N'M', N'Azul',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Faldas'),


    (N'Short Denim Azul',
     N'Short de mezclilla femenino para uso casual.',
     15900.00, N'short1.jpg', N'M', N'Azul',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Shorts'),

    (N'Short Negro',
     N'Short femenino cómodo en color negro.',
     14900.00, N'short2.jpg', N'S', N'Negro',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Shorts'),

    (N'Short Beige',
     N'Short femenino casual en tono beige.',
     15500.00, N'short3.jpg', N'M', N'Beige',
     CAST(0 AS BIT), CAST(1 AS BIT), N'Shorts'),

    (N'Short Blanco',
     N'Short femenino fresco y fácil de combinar.',
     15500.00, N'short4.jpg', N'L', N'Blanco',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Shorts'),

    (N'Short Verde',
     N'Short femenino moderno para días cálidos.',
     15900.00, N'short5.jpg', N'M', N'Verde',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Shorts'),


    (N'Blazer Beige',
     N'Blazer femenino elegante para oficina o eventos.',
     28900.00, N'blazer1.jpg', N'M', N'Beige',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Blazers'),

    (N'Collar Dorado',
     N'Collar femenino dorado para complementar diferentes estilos.',
     8900.00, N'accesorio1.jpg', N'Unitalla', N'Dorado',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Accesorios'),

    (N'Pulsera Dorada',
     N'Pulsera femenina moderna y elegante.',
     7500.00, N'accesorio2.jpg', N'Unitalla', N'Dorado',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Accesorios'),


    (N'Bolso Beige',
     N'Bolso femenino beige para uso diario.',
     18900.00, N'bolso1.jpg', N'Unitalla', N'Beige',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Bolsos'),

    (N'Bolso Negro',
     N'Bolso negro femenino de estilo elegante.',
     19900.00, N'bolso2.jpg', N'Unitalla', N'Negro',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Bolsos'),


    (N'Cardigan Beige',
     N'Cardigan femenino cómodo para complementar un look casual.',
     19900.00, N'cardigan1.jpg', N'M', N'Beige',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Cardigans'),


    (N'Chaleco Beige',
     N'Chaleco femenino moderno y fácil de combinar.',
     21900.00, N'chaleco1.jpg', N'M', N'Beige',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Chalecos'),


    (N'Enterizo Negro',
     N'Enterizo femenino elegante y cómodo.',
     26900.00, N'enterizo1.jpg', N'M', N'Negro',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Enterizos'),

    (N'Enterizo Rojo',
     N'Enterizo femenino moderno para ocasiones especiales.',
     27900.00, N'enterizo2.jpg', N'M', N'Rojo',
     CAST(0 AS BIT), CAST(1 AS BIT), N'Enterizos'),


    (N'Faja Negra',
     N'Faja femenina cómoda y ajustable.',
     12900.00, N'faja1.jpg', N'M', N'Negro',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Fajas'),

    (N'Faja Beige',
     N'Faja femenina en tono beige.',
     12900.00, N'faja2.jpg', N'L', N'Beige',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Fajas'),


    (N'Gabardina Beige',
     N'Gabardina femenina elegante para diferentes temporadas.',
     32900.00, N'gabardina1.jpg', N'M', N'Beige',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Gabardinas'),


    (N'Gorro Beige',
     N'Gorro femenino cómodo para complementar el outfit.',
     7900.00, N'gorro1.jpg', N'Unitalla', N'Beige',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Gorros'),


    (N'Jacket Denim',
     N'Jacket de mezclilla femenina para un estilo casual.',
     28900.00, N'jacket1.jpg', N'M', N'Azul',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Jackets'),


    (N'Suéter Beige',
     N'Suéter femenino cómodo para días frescos.',
     18500.00, N'sueter1.jpg', N'M', N'Beige',
     CAST(0 AS BIT), CAST(0 AS BIT), N'Suéteres'),


    (N'Tacones Negros',
     N'Tacones femeninos elegantes para ocasiones especiales.',
     24900.00, N'zapato1.jpg', N'38', N'Negro',
     CAST(1 AS BIT), CAST(0 AS BIT), N'Zapatos'),

    (N'Sandalias Beige',
     N'Sandalias femeninas cómodas para uso casual.',
     21900.00, N'zapato2.jpg', N'37', N'Beige',
     CAST(0 AS BIT), CAST(1 AS BIT), N'Zapatos')

) AS p
(
    NOMBRE,
    DESCRIPCION,
    PRECIO,
    IMAGEN,
    TALLA,
    COLOR,
    DESTACADO,
    NOVEDAD,
    CATEGORIA
)

INNER JOIN dbo.FIDE_CATEGORIAS_TB c
    ON c.NOMBRE = p.CATEGORIA

WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.FIDE_PRODUCTOS_TB existente
    WHERE existente.IMAGEN = p.IMAGEN
);
GO




-- Agregar inventario
DECLARE @IdActivo INT =
(
    SELECT ID_ESTADO
    FROM dbo.FIDE_ESTADOS_TB
    WHERE NOMBRE_ESTADO = N'Activo'
);

DECLARE @IdSucursalPrincipal INT =
(
    SELECT ID_SUCURSAL
    FROM dbo.FIDE_SUCURSALES_TB
    WHERE NOMBRE = N'KA Fashion San José Centro'
);

INSERT INTO dbo.FIDE_INVENTARIO_TB
(
    TALLA,
    COLOR,
    EXISTENCIAS,
    FECHA_ACTUALIZACION,
    ID_PRODUCTO,
    ID_SUCURSAL,
    ID_ESTADO
)
SELECT
    p.TALLA,
    p.COLOR,
    15,
    GETDATE(),
    p.ID_PRODUCTO,
    @IdSucursalPrincipal,
    @IdActivo
FROM dbo.FIDE_PRODUCTOS_TB p

WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.FIDE_INVENTARIO_TB i
    WHERE i.ID_PRODUCTO = p.ID_PRODUCTO
      AND i.ID_SUCURSAL = @IdSucursalPrincipal
);
GO


   
-- Agregar inventario
DECLARE @IdActivo INT =
(
    SELECT ID_ESTADO
    FROM dbo.FIDE_ESTADOS_TB
    WHERE NOMBRE_ESTADO = N'Activo'
);

INSERT INTO dbo.FIDE_INVENTARIO_TB
(
    TALLA,
    COLOR,
    EXISTENCIAS,
    FECHA_ACTUALIZACION,
    ID_PRODUCTO,
    ID_SUCURSAL,
    ID_ESTADO
)
SELECT
    p.TALLA,
    p.COLOR,
    10,
    GETDATE(),
    p.ID_PRODUCTO,
    s.ID_SUCURSAL,
    @IdActivo
FROM dbo.FIDE_PRODUCTOS_TB p
CROSS JOIN dbo.FIDE_SUCURSALES_TB s
WHERE s.ID_ESTADO = @IdActivo
AND p.ID_ESTADO = @IdActivo
AND NOT EXISTS
(
    SELECT 1
    FROM dbo.FIDE_INVENTARIO_TB i
    WHERE i.ID_PRODUCTO = p.ID_PRODUCTO
      AND i.ID_SUCURSAL = s.ID_SUCURSAL
);
GO






-- Crear rol Vendedor si todavía no existe
IF NOT EXISTS
(
    SELECT 1
    FROM dbo.tbRol
    WHERE Nombre = 'Vendedor'
)
BEGIN
    INSERT INTO dbo.tbRol (Nombre)
    VALUES ('Vendedor');
END
GO




-- Crear estados para solicitudes
IF NOT EXISTS
(
    SELECT 1
    FROM dbo.FIDE_ESTADOS_TB
    WHERE NOMBRE_ESTADO = N'Pendiente'
)
BEGIN
    INSERT INTO dbo.FIDE_ESTADOS_TB (NOMBRE_ESTADO)
    VALUES (N'Pendiente');
END

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.FIDE_ESTADOS_TB
    WHERE NOMBRE_ESTADO = N'Atendido'
)
BEGIN
    INSERT INTO dbo.FIDE_ESTADOS_TB (NOMBRE_ESTADO)
    VALUES (N'Atendido');
END
GO





-- Crear usuario vendedor
DECLARE @IdRolVendedor INT =
(
    SELECT Consecutivo
    FROM dbo.tbRol
    WHERE Nombre = 'Vendedor'
);

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.tbUsuario
    WHERE CorreoElectronico = 'vendedor@kafashion.com'
)
BEGIN

    INSERT INTO dbo.tbUsuario
    (
        Identificacion,
        Nombre,
        Apellido1,
        Apellido2,
        CorreoElectronico,
        Telefono,
        Contrasenna,
        Estado,
        TieneContrasennaTemp,
        ConsecutivoRol
    )
    VALUES
    (
        '200020002',
        'Andrea',
        'Vargas',
        'Mora',
        'vendedor@kafashion.com',
        '8888-1111',
        'Vendedor123!',
        1,
        0,
        @IdRolVendedor
    );

END
GO


-- Crear usuario administrador
DECLARE @IdRolAdministrador INT =
(
    SELECT Consecutivo
    FROM dbo.tbRol
    WHERE Nombre = 'Administrador'
);

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.tbUsuario
    WHERE CorreoElectronico = 'admin@kafashion.com'
)
BEGIN

    INSERT INTO dbo.tbUsuario
    (
        Identificacion,
        Nombre,
        Apellido1,
        Apellido2,
        CorreoElectronico,
        Telefono,
        Contrasenna,
        Estado,
        TieneContrasennaTemp,
        ConsecutivoRol
    )
    VALUES
    (
        '300030003',
        'Administrador',
        'KA',
        'Fashion',
        'admin@kafashion.com',
        '8888-2222',
        'Admin123!',
        1,
        0,
        @IdRolAdministrador
    );

END
GO