# 🍔 Food-Ordering

ASP.NET MVC ve C# kullanılarak geliştirilmiş bir **yemek sipariş sistemi**. Kullanıcılar restoranları inceleyebilir, menüden ürün seçip sepetlerine ekleyebilir ve sipariş verebilir.

## ✨ Özellikler

- Kullanıcı kaydı ve girişi
- Restoranları ve menüleri listeleme
- Sepete ürün ekleme / çıkarma
- Sipariş oluşturma ve sipariş takibi
- Yönetici paneli (restoran, ürün ve sipariş yönetimi)

> Not: Bu liste projenin genel amacına göre hazırlanmıştır. Projenizde olmayan maddeleri silin, eksik olanları ekleyin.

## 🛠️ Kullanılan Teknolojiler

- C#
- ASP.NET MVC
- Entity Framework
- SQL Server
- HTML, CSS, JavaScript, Bootstrap

## 📋 Gereksinimler

- Visual Studio 2019 veya üzeri (ASP.NET ve web geliştirme iş yükü yüklü)
- .NET Framework 4.8 (kullandığınız sürüme göre güncelleyin)
- SQL Server veya SQL Server LocalDB

## 🚀 Kurulum

1. Repoyu klonlayın:
   ```bash
   git clone https://github.com/BenanSamur/Food-Ordering.git
   ```
2. `YemekUygulamasi` klasöründeki `.sln` dosyasını Visual Studio ile açın.
3. `Web.config` dosyasındaki bağlantı dizesini (`connectionString`) kendi SQL Server bilgilerinize göre düzenleyin:
   ```xml
   <connectionStrings>
     <add name="DefaultConnection"
          connectionString="Data Source=.;Initial Catalog=YemekDb;Integrated Security=True"
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```
4. NuGet paketlerini geri yükleyin (Visual Studio otomatik yapar, yapmazsa: sağ tık → *Restore NuGet Packages*).
5. Veritabanını oluşturun (Package Manager Console):
   ```
   Update-Database
   ```
6. Projeyi çalıştırın: `F5` veya `Ctrl + F5`.

## 📸 Ekran Görüntüleri

Ekran görüntülerini projede bir `screenshots` klasörüne koyup buraya ekleyebilirsiniz:

```markdown
![Ana Sayfa](screenshots/anasayfa.png)
![Sepet](screenshots/sepet.png)
```

## 📁 Proje Yapısı

```
Food-Ordering
└── YemekUygulamasi
    ├── Controllers   # İstekleri karşılayan denetleyiciler
    ├── Models        # Veri modelleri
    ├── Views         # Arayüz sayfaları (Razor)
    └── Content       # CSS, görseller, script dosyaları
```

## 🤝 Katkıda Bulunma

1. Repoyu fork'layın
2. Yeni bir dal oluşturun (`git checkout -b yeni-ozellik`)
3. Değişikliklerinizi commit edin (`git commit -m "Yeni özellik eklendi"`)
4. Dalı gönderin (`git push origin yeni-ozellik`)
5. Pull Request açın

## 📄 Lisans

Bu proje [MIT Lisansı](LICENSE) ile lisanslanmıştır.

## 👤 Geliştirici

**Benan Samur** — [GitHub](https://github.com/BenanSamur)
