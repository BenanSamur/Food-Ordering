/* ============================================================
   MEVCUT veritabanını güncelleme script'i

   Yeni sürümde şifreler hash olarak saklanıyor; hash 50 karakterden
   uzun olduğu için Kullanıcı tablosundaki Şifre sütununun genişletilmesi
   gerekir. Veritabanınızı SİLMEDEN, yalnızca bu script'i çalıştırın.

   Var olan kullanıcıların düz metin şifreleri bozulmaz: uygulama,
   her kullanıcının ilk başarılı girişinde şifresini otomatik olarak
   hash'e çevirir.

   (Sıfırdan kurulum yapıyorsanız bunun yerine database.sql kullanın.)
   ============================================================ */

USE [yemekdb];
GO

ALTER TABLE [dbo].[Kullanıcı] ALTER COLUMN [Şifre] VARCHAR(200) NULL;
GO

PRINT N'Kullanıcı.Şifre sütunu 200 karaktere genişletildi.';
GO
