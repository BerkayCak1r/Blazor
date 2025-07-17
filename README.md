# Northwind Blazor Uygulaması

Bu proje, Northwind veritabanı ile entegre çalışan basit bir ürün yönetim sistemidir. Blazor Server ile geliştirilen kullanıcı arayüzü, ASP.NET Web API ile haberleşerek veritabanı işlemlerini gerçekleştirir.

## Katmanlar

- **Northwind.Core** → Entity (Model) sınıflarını barındırır. (`Product`, `Category`)
- **Northwind.Data** → EF Core `DbContext` yapılandırmasını içerir.
- **WebAPI** → Ürün ve kategori işlemlerini sağlayan RESTful API'yi barındırır.
- **BlazorApp** → Ürün listeleme, ekleme, güncelleme ve silme işlemlerinin yapıldığı Blazor Server arayüzüdür.

## Temel Özellikler

- Ürünleri listeleme
- Ürün arama ve filtreleme
- Yeni ürün ekleme
- Ürün güncelleme ve silme
- Kategori seçimi
- API ile iletişim (HttpClient)
- JSON işlemleri `System.Text.Json` ile yapılmaktadır

## Gereksinimler

- .NET 7 SDK
- Visual Studio 2022+
- SQL Server (Northwind veritabanı yüklü)

## Çalıştırma

1. **WebAPI** ve **BlazorApp** projeleri aynı anda çalıştırılmalıdır.
2. `appsettings.json` içindeki bağlantı dizesi güncel olmalıdır.
3. API URL’si BlazorApp içinde `HttpClient` için doğru tanımlanmalıdır.

---

