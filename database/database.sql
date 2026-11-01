/* ============================================================
   Food-Ordering  -  yemekdb veritabanı kurulum script'i
   SQL Server (Express / LocalDB / Developer) için hazırlanmıştır.

   Kullanım:
     1) SQL Server Management Studio (SSMS) ile sunucunuza bağlanın.
     2) Bu dosyayı açıp "Execute" (F5) deyin.
     3) Projedeki Web.config içinde "data source=" kısmını kendi
        sunucu adınızla değiştirin (örn. .\SQLEXPRESS veya
        (localdb)\MSSQLLocalDB).

   Dikkat: Aynı adda bir veritabanı varsa SİLİNİR ve yeniden oluşturulur.
   ============================================================ */

USE [master];
GO

IF DB_ID(N'yemekdb') IS NOT NULL
BEGIN
    ALTER DATABASE [yemekdb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [yemekdb];
END
GO

-- Türkçe karakterlerin varchar sütunlarda doğru saklanması için Turkish_CI_AS
CREATE DATABASE [yemekdb] COLLATE Turkish_CI_AS;
GO

USE [yemekdb];
GO

/* ---------------------------- Tablolar ---------------------------- */

CREATE TABLE [dbo].[Kategoriler] (
    [KategoriID]   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [KategoriAdı]  NVARCHAR(50) NULL
);

CREATE TABLE [dbo].[AltKategori] (
    [AltKategoriID]  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [KategoriID]     INT NULL,
    [AltKategoriAdı] VARCHAR(50) NULL,
    [Açıklama]       VARCHAR(200) NULL,
    CONSTRAINT [FK_AltKategori_Kategoriler]
        FOREIGN KEY ([KategoriID]) REFERENCES [dbo].[Kategoriler]([KategoriID])
);

/* Admin hesapları. Şifre, uygulamanın PBKDF2 biçiminde (hash) saklanır. */
CREATE TABLE [dbo].[Admin] (
    [AdminID]  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [İsim]     NVARCHAR(100) NULL,
    [E_posta]  NVARCHAR(100) NULL,
    [Şifre]    NVARCHAR(100) NULL
);

/* Kullanıcı şifresi hash olarak saklandığı için sütun 200 karakter. */
CREATE TABLE [dbo].[Kullanıcı] (
    [KullanıcıID]  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [KullanıcıAdı] VARCHAR(100) NULL,
    [Şifre]        VARCHAR(200) NULL,
    [EMail]        VARCHAR(200) NULL,
    [Adres]        VARCHAR(200) NULL
);

CREATE TABLE [dbo].[Firma] (
    [FirmaID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Ad]      VARCHAR(100) NULL,
    [Eposta]  VARCHAR(100) NULL,
    [Şifre]   VARCHAR(50)  NULL,
    [Mesaj]   VARCHAR(200) NULL
);

CREATE TABLE [dbo].[İletişim] (
    [Ad]     VARCHAR(50)  NULL,
    [EMail]  VARCHAR(100) NULL,
    [Mesaj]  VARCHAR(200) NULL
);

/* ----- Ürün tabloları (kategori başına bir tablo) ----- */

CREATE TABLE [dbo].[Hamburger] (
    [ID]         INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Adı]        NVARCHAR(100) NULL,
    [Açıklama]   NVARCHAR(255) NULL,
    [Fiyat]      DECIMAL(10,2) NULL,
    [KategoriID] INT NULL,
    [ResimURL]   VARCHAR(255) NULL,
    CONSTRAINT [FK_Hamburger_Kategoriler] FOREIGN KEY ([KategoriID]) REFERENCES [dbo].[Kategoriler]([KategoriID])
);

CREATE TABLE [dbo].[Pizza] (
    [ID]         INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Adı]        NVARCHAR(100) NULL,
    [Açıklama]   NVARCHAR(255) NULL,
    [Fiyat]      DECIMAL(10,2) NULL,
    [KategoriID] INT NULL,
    [ResimURL]   VARCHAR(255) NULL,
    CONSTRAINT [FK_Pizza_Kategoriler] FOREIGN KEY ([KategoriID]) REFERENCES [dbo].[Kategoriler]([KategoriID])
);

CREATE TABLE [dbo].[Sushi] (
    [ID]         INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Adı]        NVARCHAR(100) NULL,
    [Açıklama]   NVARCHAR(255) NULL,
    [Fiyat]      DECIMAL(10,2) NULL,
    [KategoriID] INT NULL,
    [ResimURL]   VARCHAR(255) NULL,
    CONSTRAINT [FK_Sushi_Kategoriler] FOREIGN KEY ([KategoriID]) REFERENCES [dbo].[Kategoriler]([KategoriID])
);

CREATE TABLE [dbo].[Balık] (
    [ID]         INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Adı]        NVARCHAR(100) NULL,
    [Açıklama]   NVARCHAR(255) NULL,
    [Fiyat]      DECIMAL(10,2) NULL,
    [KategoriID] INT NULL,
    [ResimURL]   VARCHAR(255) NULL,
    CONSTRAINT [FK_Balık_Kategoriler] FOREIGN KEY ([KategoriID]) REFERENCES [dbo].[Kategoriler]([KategoriID])
);

CREATE TABLE [dbo].[Tatlı] (
    [ID]         INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Adı]        NVARCHAR(100) NULL,
    [Açıklama]   NVARCHAR(255) NULL,
    [Fiyat]      DECIMAL(10,2) NULL,
    [KategoriID] INT NULL,
    [ResimURL]   VARCHAR(255) NULL,
    CONSTRAINT [FK_Tatlı_Kategoriler] FOREIGN KEY ([KategoriID]) REFERENCES [dbo].[Kategoriler]([KategoriID])
);

CREATE TABLE [dbo].[İçecekler] (
    [İçecekID]      INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [İçecekAdı]     VARCHAR(100) NULL,
    [Açıklama]      VARCHAR(250) NULL,
    [Fiyat]         DECIMAL(10,2) NULL,
    [AltKategoriID] INT NULL,
    [ResimURL]      VARCHAR(255) NULL,
    CONSTRAINT [FK_İçecekler_AltKategori] FOREIGN KEY ([AltKategoriID]) REFERENCES [dbo].[AltKategori]([AltKategoriID])
);

CREATE TABLE [dbo].[Yemekler] (
    [YemekID]       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [YemekAdı]      NVARCHAR(100) NULL,
    [Açıklama]      NVARCHAR(255) NULL,
    [Fiyat]         DECIMAL(10,2) NULL,
    [AltKategoriID] INT NULL,
    CONSTRAINT [FK_Yemekler_AltKategori] FOREIGN KEY ([AltKategoriID]) REFERENCES [dbo].[AltKategori]([AltKategoriID])
);

/* ----- Sipariş tabloları ----- */

/* Sepetteki / verilen siparişler. Her satır, ürün tablolarından yalnızca birine bağlanır.
   Not: Eski şemada Sushi/Tatlı/İçecek sütunları yanlışlıkla Balık tablosuna
   bağlıydı; burada her biri kendi tablosuna bağlanıyor. */
CREATE TABLE [dbo].[Siparişler] (
    [SiparişID]   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [HamburgerId] INT NULL,
    [PizzaId]     INT NULL,
    [BalıkId]     INT NULL,
    [SushiId]     INT NULL,
    [İçeceklerId] INT NULL,
    [TatlıId]     INT NULL,
    [Miktar]      INT NULL,
    [Adı]         NVARCHAR(255) NULL,
    [Fiyat]       DECIMAL(18,2) NULL,
    [ResimURL]    NVARCHAR(255) NULL,
    [Açıklama]    NVARCHAR(MAX) NULL,
    CONSTRAINT [FK_Siparişler_Hamburger] FOREIGN KEY ([HamburgerId]) REFERENCES [dbo].[Hamburger]([ID]),
    CONSTRAINT [FK_Siparişler_Pizza]     FOREIGN KEY ([PizzaId])     REFERENCES [dbo].[Pizza]([ID]),
    CONSTRAINT [FK_Siparişler_Balık]     FOREIGN KEY ([BalıkId])     REFERENCES [dbo].[Balık]([ID]),
    CONSTRAINT [FK_Siparişler_Sushi]     FOREIGN KEY ([SushiId])     REFERENCES [dbo].[Sushi]([ID]),
    CONSTRAINT [FK_Siparişler_İçecekler] FOREIGN KEY ([İçeceklerId]) REFERENCES [dbo].[İçecekler]([İçecekID]),
    CONSTRAINT [FK_Siparişler_Tatlı]     FOREIGN KEY ([TatlıId])     REFERENCES [dbo].[Tatlı]([ID])
);

CREATE TABLE [dbo].[SiparişDetayları] (
    [SiparişDetayID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [SiparişID]      INT NULL,
    [YemekID]        INT NULL,
    [Miktar]         INT NULL,
    [Fiyat]          DECIMAL(10,2) NULL,
    CONSTRAINT [FK_SiparişDetayları_Yemekler] FOREIGN KEY ([YemekID]) REFERENCES [dbo].[Yemekler]([YemekID])
);

CREATE TABLE [dbo].[Rating] (
    [RatingID]    INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [SiparişID]   INT NULL,
    [KullanıcıID] INT NULL,
    [Rate]        INT NULL,
    [RatingTarihi] DATETIME NULL,
    CONSTRAINT [FK_Rating_Kullanıcı] FOREIGN KEY ([KullanıcıID]) REFERENCES [dbo].[Kullanıcı]([KullanıcıID])
);

CREATE TABLE [dbo].[SiparişYorumları] (
    [YorumID]     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [SiparişID]   INT NULL,
    [KullanıcıID] INT NULL,
    [Açıklama]    VARCHAR(200) NOT NULL,
    [YorumTarihi] DATETIME NULL,
    CONSTRAINT [FK_SiparişYorumları_Kullanıcı] FOREIGN KEY ([KullanıcıID]) REFERENCES [dbo].[Kullanıcı]([KullanıcıID])
);
GO

/* ------------------------- Örnek veriler ------------------------- */

INSERT INTO [dbo].[Kategoriler] ([KategoriAdı]) VALUES
    (N'Hamburger'),   -- 1
    (N'Pizza'),       -- 2
    (N'Sushi'),       -- 3
    (N'Balık'),       -- 4
    (N'Tatlı'),       -- 5
    (N'İçecekler');   -- 6

INSERT INTO [dbo].[AltKategori] ([KategoriID], [AltKategoriAdı], [Açıklama]) VALUES
    (6, N'Soğuk İçecekler', N'Gazlı ve gazsız soğuk içecekler'),
    (6, N'Sıcak İçecekler', N'Çay, kahve ve diğer sıcak içecekler');

INSERT INTO [dbo].[Hamburger] ([Adı], [Açıklama], [Fiyat], [KategoriID], [ResimURL]) VALUES
    (N'Hamburger',    N'Dana köfte, marul, domates, turşu',      180.00, 1, '/Content/images/hamburger.jpg'),
    (N'Cheeseburger', N'Dana köfte, cheddar peyniri, özel sos',  200.00, 1, '/Content/images/cheeseburger.jpg'),
    (N'Veggie Burger', N'Sebze köftesi, taze yeşillikler',       170.00, 1, '/Content/images/veggieburger.jpg');

INSERT INTO [dbo].[Pizza] ([Adı], [Açıklama], [Fiyat], [KategoriID], [ResimURL]) VALUES
    (N'Margherita',    N'Domates sosu, mozzarella, fesleğen',      220.00, 2, '/Content/images/margherita.jpg'),
    (N'Sucuklu Pizza', N'Sucuk, mozzarella, domates sosu',         250.00, 2, '/Content/images/sucuklupizza.jpg'),
    (N'Veggie Pizza',  N'Mevsim sebzeleri, mozzarella',            230.00, 2, '/Content/images/veggie%20pizza.jpg');

INSERT INTO [dbo].[Sushi] ([Adı], [Açıklama], [Fiyat], [KategoriID], [ResimURL]) VALUES
    (N'Somon Maki',     N'8 parça somonlu maki rulo',   320.00, 3, '/web-site/image/SUSH%C4%B0.jpg'),
    (N'Karides Nigiri', N'6 parça karidesli nigiri',    340.00, 3, '/web-site/image/SUSH%C4%B0.jpg');

INSERT INTO [dbo].[Balık] ([Adı], [Açıklama], [Fiyat], [KategoriID], [ResimURL]) VALUES
    (N'Izgara Levrek', N'Izgara levrek, roka salatası ile',   360.00, 4, '/web-site/image/BALIK.jpg'),
    (N'Çipura Tava',   N'Tavada çipura, limon ve patates',    340.00, 4, '/web-site/image/BALIK.jpg');

INSERT INTO [dbo].[Tatlı] ([Adı], [Açıklama], [Fiyat], [KategoriID], [ResimURL]) VALUES
    (N'Sütlaç',  N'Fırında sütlaç',               90.00, 5, '/web-site/image/TATLI.jpg'),
    (N'Baklava', N'Antep fıstıklı baklava, 4 dilim', 150.00, 5, '/web-site/image/TATLI.jpg');

INSERT INTO [dbo].[İçecekler] ([İçecekAdı], [Açıklama], [Fiyat], [AltKategoriID], [ResimURL]) VALUES
    (N'Kola',    N'330 ml kutu',         45.00, 1, '/web-site/image/drinks.jpg'),
    (N'Ayran',   N'300 ml',              30.00, 1, '/web-site/image/drinks.jpg'),
    (N'Çay',     N'Demlik çay',          20.00, 2, '/web-site/image/drinks.jpg');

/* ---------------------- Demo admin hesabı ----------------------
   E-posta : admin@example.com
   Şifre   : Admin123!
   Şifre aşağıda PBKDF2 hash olarak saklanır (düz metin değildir).

   !!! SADECE GELİŞTİRME / DEMO İÇİNDİR !!!
   Siteyi gerçekten yayına almadan önce bu hesabı silin veya şifresini
   değiştirin. Bu şifre herkese açık repoda yazdığı için güvenli değildir.
   ------------------------------------------------------------------ */
INSERT INTO [dbo].[Admin] ([İsim], [E_posta], [Şifre]) VALUES
    (N'Demo Admin', N'admin@example.com',
     N'PBKDF2$100000$ABEiM0RVZneImaq7zN3u/w==$eDh9NKddpwgeM6+ZLeY3+1vk2zlCod7Gi7bOTnsEG7U=');
GO

PRINT N'yemekdb veritabanı başarıyla oluşturuldu.';
GO
