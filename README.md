# 🍔 Food-Ordering

ASP.NET MVC ve C# ile geliştirilmiş bir **yemek sipariş web uygulaması**. Kullanıcılar menüdeki ürünleri kategorilere göre inceleyebilir, sepetlerine ekleyebilir ve sipariş verebilir.

## ✨ Özellikler

- Kullanıcı kaydı ve girişi (şifreler PBKDF2 ile hash'lenerek saklanır)
- Ayrı yönetici (admin) girişi ve korumalı yönetici sayfası
- 6 ürün kategorisi: Hamburger, Pizza, Sushi, Balık, Tatlı, İçecekler
- Sepete ürün ekleme, sepetten çıkarma, toplam tutar hesaplama
- Ödeme sayfası ve sipariş onay ekranı
- İletişim formu
- Yönetici paneli arayüzü (menü, iletişim ve sayfa yönetimi taslağı)

## 🛠️ Kullanılan Teknolojiler

- C#, ASP.NET MVC 5 (.NET Framework 4.7.2)
- Entity Framework 6 (Database First, `Model1.edmx`)
- SQL Server
- Razor, HTML, CSS, JavaScript, Bootstrap, jQuery

## 📋 Gereksinimler

- Windows ve **Visual Studio 2019 veya üzeri** ("ASP.NET ve web geliştirme" iş yükü yüklü)
- .NET Framework 4.7.2 Developer Pack
- SQL Server Express, LocalDB veya Developer sürümü
- SQL Server Management Studio (SSMS) – veritabanını kurmak için

## 🚀 Kurulum

1. Repoyu klonlayın:
   ```bash
   git clone https://github.com/BenanSamur/Food-Ordering.git
   ```
2. **Veritabanını oluşturun:** SSMS ile sunucunuza bağlanıp [`database/database.sql`](database/database.sql) dosyasını çalıştırın. Bu script `yemekdb` veritabanını, tabloları ve örnek menü verilerini oluşturur.
3. `YemekUygulaması/YemekUygulaması.sln` dosyasını Visual Studio ile açın.
4. `YemekUygulaması/Web.config` içindeki `yemekdbEntities1` bağlantı dizesinde `data source=` değerini kendi SQL Server adınızla değiştirin:
   ```
   data source=BENAN            →  .\SQLEXPRESS   veya   (localdb)\MSSQLLocalDB
   ```
5. NuGet paketlerini geri yükleyin (Visual Studio otomatik yapar; yapmazsa çözüme sağ tık → *Restore NuGet Packages*).
6. Projeyi çalıştırın: `F5`.

> Veritabanını daha önce eski sürümle kurduysanız, silmeden güncellemek için [`database/migration-sifre-sutunu.sql`](database/migration-sifre-sutunu.sql) dosyasını çalıştırmanız yeterlidir. Mevcut kullanıcıların şifreleri, ilk başarılı girişlerinde otomatik olarak hash'e dönüştürülür.

## 🔑 Demo yönetici hesabı

`database.sql` ile bir demo admin hesabı oluşur:

| Alan | Değer |
|------|-------|
| Giriş adresi | `/Default/AdminLogin` |
| E-posta | `admin@example.com` |
| Şifre | `Admin123!` |

> ⚠️ Bu hesap **yalnızca geliştirme ve deneme içindir**. Şifre herkese açık bir repoda yazdığı için güvenli değildir; siteyi gerçek bir ortama yayınlamadan önce bu hesabı silin veya şifresini değiştirin.
>
> Yeni admin hesapları kayıt formundan açılamaz; `Admin` tablosuna veritabanından eklenir.

## 📁 Proje Yapısı

```
Food-Ordering
├── database/                  # Veritabanı kurulum ve güncelleme script'leri
│   ├── database.sql
│   └── migration-sifre-sutunu.sql
└── YemekUygulaması/
    ├── YemekUygulaması.sln
    └── YemekUygulaması/
        ├── Controllers/       # Default, Urun, Sepet, Home denetleyicileri
        ├── Helpers/           # PasswordHasher (şifre hash'leme)
        ├── Models/            # EF modelleri (Model1.edmx)
        ├── Views/             # Razor sayfaları
        ├── Content/           # CSS ve ürün görselleri
        ├── Scripts/           # JavaScript dosyaları
        └── Web.config
```

## ⚠️ Bilinen Sınırlamalar

- **Ödeme sayfası gösterim amaçlıdır.** Gerçek bir ödeme sağlayıcısına bağlı değildir ve kart bilgileri saklanmaz. Gerçek kart bilgisi girmeyin.
- Yönetici paneli şu an bir arayüz taslağıdır; menü ve içerik yönetimi işlevleri henüz tamamlanmamıştır.
- Bağlantı dizesinde SQL Server adı yerel bilgisayara göre ayarlanmıştır; kendi ortamınıza göre değiştirmeniz gerekir.

## 👤 Geliştirici

**Benan Samur** — [GitHub](https://github.com/BenanSamur)

<!-- Ekran görüntüleri eklemek için: projeyi çalıştırıp görüntü alın, repoya `screenshots/` klasörü açın ve buraya
     ![Ana Sayfa](screenshots/anasayfa.png) biçiminde ekleyin. -->
