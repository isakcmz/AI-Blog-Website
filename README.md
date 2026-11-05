# BtkAkademiAIBlog 🚀

Yapay zekâ destekli, tam kapsamlı bir **blog platformu**. Kullanıcılar makale okuyup yorum yapabilirken, admin paneli Groq, OpenAI, Gemini, ElevenLabs, Tavily gibi yapay zekâ servisleriyle içerik üretiminden e-posta yönetimine kadar birçok işi otomatikleştirebiliyor.

---

## 🎯 Projenin Amacı

Bu proje, modern bir blog sisteminin tüm ihtiyaçlarını (içerik yönetimi, üyelik, yorumlar, iletişim) **iki katmanlı bir mimari**yle karşılayan; ayrıca admin tarafına **yapay zekâ destekli içerik üretim ve moderasyon araçları** entegre edilmiş bir web uygulamasıdır. Temel hedefler:

- Kullanıcıya sezgisel bir okuma deneyimi sunmak (slider, trending stories, kategori bazlı listeler, makale detayı)
- Yöneticiye içeriği hızlıca üretme, çevirme, seslendirme ve moderasyon imkânı vermek
- Frontend (MVC) ile backend (REST API) katmanlarını net bir şekilde ayırarak sürdürülebilir bir yapı kurmak

---

## ✨ Proje Neler Yapabiliyor?

### Kullanıcı Tarafı (Public)
- 📰 **Makale listesi ve detayı** — sayfalama, yazar bilgisi, ilgili yazılar, önceki/sonraki makale gezinmesi
- 🗂️ **Kategori bazlı listeler** — her kategorinin son makaleleri, son 4 makale, son teknoloji/spor/seyahat makaleleri
- 💬 **Yorum yapma** ve makale altında son yorumları görüntüleme
- 🔥 **Trending Stories** ve featured içerik bölümleri
- 📩 **İletişim formu** (mesaj gönderme)
- 👤 **Kayıt olma / Giriş yapma** (ASP.NET Core Identity + Cookie Authentication)
- 🎬 **Trading video** bölümü
- 🚫 Özel 404 hata sayfası

### Admin Tarafı (Yönetim Paneli)
- 📝 Makale, kategori, yorum, slider, trading video, kullanıcı, iletişim ve mesaj yönetimi (CRUD)
- 🤖 **AI ile makale ve başlık üretimi** (Groq)
- 🌍 **AI ile çok dilli blog içeriği oluşturma** (Gemini — EN / DE / IT / FR)
- 🛡️ **AI destekli toksisite (zehirli içerik) analizi** (Gemini)
- 🔊 **Metinden sese dönüştürme** (ElevenLabs — MP3 olarak kaydedilir)
- 🌐 **AI destekli web araması** (Tavily)
- ✉️ **AI ile e-posta yanıtı tasarlama** (OpenAI / Groq) ve **SMTP ile e-posta gönderme** (MailKit)
- 💬 **SignalR üzerinden canlı, akış (streaming) destekli AI sohbeti**
- 🎥 **RapidAPI (IMDb) ile film listesi** görüntüleme

---

## 🤖 Yapay Zekâ Entegrasyonları

| Servis | Model / Özellik | Projede Kullanımı |
|---|---|---|
| **Groq** | `openai/gpt-oss-120b` | SEO uyumlu Türkçe makale üretimi, makale başlığı önerme, e-posta yanıtı oluşturma, SignalR üzerinden **token token'a akış yapan** canlı AI sohbet asistanı |
| **OpenAI** | `gpt-4o-mini` | Kullanıcıdan gelen e-postayı otomatik dil algılayıp aynı dilde profesyonel yanıt taslağı oluşturma |
| **Google Gemini** | `generateContent` | Türkçe blog içeriğini kelime kelime çevirmek yerine yerel okuyucu için yeniden yazma (meta açıklamasıyla birlikte JSON çıktı); yorum moderasyonu için 6 kategoride (toxic, severe_toxic, obscene, threat, insult, identity_hate) toksisite skorlama |
| **ElevenLabs** | `eleven_multilingual_v2` | Metinden sese dönüştürme; üretilen ses `wwwroot/voices` altına MP3 olarak kaydedilip arayüzde oynatılır |
| **Tavily** | Search API (advanced) | AI destekli web araması; özet cevap + en fazla 5 sonuç listelenir |
| **RapidAPI (IMDb)** | IMDb list endpoint | Sayfalanmış film listesi görüntüleme |

Tüm API anahtarları `appsettings.Development.json` üzerinden `IConfiguration` ile okunur ve bu dosya **`.gitignore`'a dahildir** (repoya gönderilmez).

---

## 🏗️ Mimari

Proje, **iki ayrı .NET 6 projesinden** oluşan katmanlı bir yapıya sahiptir:

```
┌─────────────────────────┐         ┌──────────────────────────┐         ┌────────────┐
│   BtkAkademiAIBlog.WebUI │  HTTP   │   BtkAkademiAIBlog.WebApi │  EF Core │ SQL Server │
│      (ASP.NET Core MVC)  │ ──────▶ │     (REST API Katmanı)    │ ──────▶ │            │
└─────────────────────────┘  JSON   └──────────────────────────┘         └────────────┘
```

- **WebApi** — veri erişimi (EF Core), iş kuralları, kimlik doğrulama (Identity + Cookie Auth) ve REST endpoint'leri. Entity'ler DTO'lara **AutoMapper** ile map'lenir; API **Swagger** ile dokümante edilir.
- **WebUI** — MVC arayüzü. Veritabanına **hiç doğrudan dokunmaz**; `IHttpClientFactory` ile API'yi tüketir, JSON yanıtları kendi DTO'larına deserialize eder. Admin paneli **Area** yapısı içinde tutulur; sayfa parçaları **ViewComponent**'lerle modülerleştirilmiştir.

### Katmanlar Arası Veri Akışı
```
Tarayıcı → MVC Controller → HttpClient (JSON) → API Controller → AutoMapper → EF Core → Veritabanı
```

### 📐 Geliştirme İş Akışı (Yeni Bir Özellik Eklerken)

Projede yeni bir işlem eklenirken izlenen adımlar:

1. **WebApi tarafında:**
   - `Entities/` altına **entity** sınıfı yazılır
   - `Dtos/` altına o entity'ye ait **DTO'lar** oluşturulur (Create / Update / Get / Result)
   - **AutoMapper mapping** tanımlanır
   - `Context/BlogAIContext.cs`'e **DbSet** eklenir, migration alınır
   - `Controllers/` altına **API Controller** yazılır (REST endpoint'leri)
2. **WebUI tarafında (arayüz gerekiyorsa):**
   - API DTO'larını yansıyan **DTO'lar** oluşturulur
   - API'yi tüketen **MVC Controller** yazılır
   - **View** sayfaları (ve gerekirse ViewComponent'ler) hazırlanır

---

## 🗂️ Proje Yapısı

```
BtkAkademiAIBlog/
│
├── BtkAkademiAIBlog.WebApi/              # REST API katmanı
│   ├── Context/BlogAIContext.cs           # EF Core + Identity DbContext
│   ├── Entities/                          # Article, Category, Comment, Contact,
│   │                                      # Message, SliderCarousel, TradingVideo,
│   │                                      # Employee, About, AppUser
│   ├── Dtos/                              # Entity bazlı klasörlenmiş DTO'lar
│   ├── Controllers/                       # REST endpoint'leri
│   ├── Extensions/ServiceExtensions.cs    # DbContext, AutoMapper, Identity, Auth kayıtları
│   └── Migrations/                        # EF Core migration'ları
│
└── BtkAkademiAIBlog.WebUI/                # MVC arayüz katmanı
    ├── Controllers/                       # Public controller'lar (Default, Article,
    │                                      # Comment, Contact, Register, Login ...)
    ├── Areas/Admin/                       # Admin paneli (controller + view + modeller)
    │   ├── Controllers/                   # AdminArticle, AdminComment, AdminChat,
    │   │                                  # AdminToxicity, AdminTavily, AdminElevenLabs,
    │   │                                  # AdminEmail, AdminGeminiMultiLanguage, MovieAdmin ...
    │   └── Models/                        # Tavily, Movie, Email modelleri
    ├── ViewComponents/                    # Layout, Default, ArticleDetail, Admin komponentleri
    ├── Views/                             # Razor view'lar + Shared/Components
    ├── Services/                          # OpenAIArticleService, OpenAIArticleTitleService
    ├── Hubs/ChatHub.cs                    # SignalR + Groq streaming sohbet
    └── Dtos/                              # API tüketimi için DTO'lar
```

---

## 🛠️ Kullanılan Teknolojiler

| Katman | Teknoloji |
|---|---|
| Framework | **.NET 6** (Web API + MVC) |
| Veri Erişimi | **Entity Framework Core 6** (SQL Server) — Code First + Migrations |
| Kimlik Doğrulama | **ASP.NET Core Identity** + Cookie Authentication |
| Nesne Eşleme | **AutoMapper 13** |
| Gerçek Zamanlı İletişim | **SignalR** (akış destekli AI sohbeti) |
| API Dokümantasyonu | **Swagger (Swashbuckle)** |
| API Tüketimi | **IHttpClientFactory** + Newtonsoft.Json |
| E-posta | **MailKit** (Gmail SMTP) |
| Yapay Zekâ | **Groq, OpenAI, Google Gemini, ElevenLabs, Tavily** |
| Harici API | **RapidAPI** (IMDb film listesi) |

---

## 🚀 Kurulum

1. **Depoyu klonlayın:**
   ```bash
   git clone <repo-url>
   ```

2. **Veritabanı bağlantısını düzenleyin:** `BtkAkademiAIBlog.WebApi/Context/BlogAIContext.cs` içindeki SQL Server connection string'ini kendi sunucunuza göre güncelleyin.

3. **Migration'ları uygulayın** (API projesi dizininde):
   ```bash
   dotnet ef database update
   ```

4. **API anahtarlarını ekleyin:** Her iki proje için de `appsettings.Development.json` oluşturup aşağıdaki gibi kendi anahtarlarınızı girin. (Bu dosya `.gitignore`'dadır, repoya gönderilmez.)
   ```json
   {
     "Groq": { "ApiKey": "..." },
     "OpenAI": { "ApiKey": "..." },
     "Gemini": { "ApiKey": "...", "Model": "..." },
     "ElevenLabs": { "ApiKey": "..." },
     "Tavily": { "ApiKey": "..." },
     "RapidApi": { "Key": "...", "ImdbHost": "..." },
     "EmailSettings": {
       "SenderEmail": "...",
       "SenderName": "...",
       "AppPassword": "..."
     }
   }
   ```

5. **Projeleri çalıştırın:** Önce **WebApi** projesini (Swagger arayüzü ile), ardından **WebUI** projesini başlatın. UI, API'ye `https://localhost:7017` üzerinden bağlanır; portu kendi ortamınıza göre kontrol edin.

---

## 🔒 Güvenlik Notu

Tüm API anahtarları ve SMTP bilgileri `appsettings.Development.json` içinde tutulur ve bu dosya **`.gitignore` ile versiyon kontrolüne dahil edilmez**. Repoda yalnızca anahtar içermeyen `appsettings.json` dosyaları bulunur.
