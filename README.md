# Adesso World League - Kura Çekme Sistemi

Adesso World League için 32 takımı (8 ülke × 4 takım) gruplara dağıtan kura çekme uygulaması.

---

**Bağımlılık Yönü:** WebAPI → Application → Domain ← Persistence

## Teknoloji Stack

### Backend
- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 8.0**
- **SQLite**

### Libraries & Tools
- **AutoMapper 12.0**
- **FluentValidation 11.9**
- **Swashbuckle (Swagger)**
- **Serilog 9.0**

### Design Patterns
- **Repository Pattern**
- **Unit of Work Pattern**
- **Dependency Injection**

---

### Neden Scoped Lifetime?
services.AddScoped<IDrawService, DrawService>();
services.AddScoped<IDrawRepository, DrawRepository>();
services.AddScoped<IUnitOfWork, UnitOfWork>();
```

**Scoped Seçme Nedenleri:**
- Request boyunca aynı DbContext instance'ı kullanılır
- UnitOfWork ile transaction koordinasyonu sağlanır
- Memory leak riski yok (request sonunda dispose edilir)
- Thread-safe (her request farklı thread)

**Örnek Senaryo:**
```
Request geldi:
  ├─ DrawController instance oluştu
  ├─ DrawService inject edildi (Scoped)
  ├─ UnitOfWork inject edildi (AYNI Scoped context)
  │   └─ DrawRepository'de AYNI DbContext
  └─ SaveChanges() → Hepsi senkronize çalıştı

Request bitti → Hepsi dispose edildi, memory temizlendi
```

---

### 2. Neden SQLite?
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=AdessoWorldLeague.db"
}
```

**Avantajlar:**
- Tek `.db` dosyası, kolayca taşınabilir
- Docker, SQL Server kurulumu gerekmiyor
- Visual Studio yeterli
- Hızlı, test kolaylığı
- Database dosyası proje ile birlikte gider

---

## Kura Algoritması

### Algoritma Akışı
```
1. 
   ├─ Takımları database'den çek
   ├─ Ülkelere göre grupla
   └─ Validasyon (8 ülke, her ülkede 4 takım)

2. 
   ├─ Her ülkenin takımlarını rastgele karıştır
   └─ Queue'ya dönüştür (O(1) performans için)

3. 
   ├─ Slot 0 → Grup A (1. tur)
   ├─ Slot 1 → Grup B
   ├─ Slot 2 → Grup C
   ├─ ...
   ├─ Slot 8 → Grup A (2. tur)
   └─ ...continue

4. 
   ├─ Entity'leri oluştur
   ├─ Database'e kaydet
   └─ Response DTO dön
```

### Detaylı Açıklama

#### 1. Queue Kullanımı (Performans Optimizasyonu)
var countryTeamQueues = teamsByCountry.ToDictionary(
    kvp => kvp.Key,
    kvp => new Queue<Team>(kvp.Value.OrderBy(_ => _random.Next()))
);
```

#### 2. Round-Robin Mantığı
var totalSlots = numberOfGroups * teamsPerGroup;  // 8 × 4 = 32

for (int slot = 0; slot < totalSlots; slot++)
{
    var groupName = groupNames[slot % numberOfGroups];
    // slot % 8 → 0,1,2,3,4,5,6,7, 0,1,2,3,4,5,6,7, ...
```

#### 3. Ülke Çakışması Önleme
var availableCountry = allCountries.FirstOrDefault(country =>
    !groupCountries[groupName].Contains(country) &&
    countryTeamQueues[country].Count > 0);
```

**HashSet Kullanımı:**
var groupCountries = new Dictionary<string, HashSet<string>>();
```

## Kurulum

### Gereksinimler

- **.NET 8 SDK**
- **Visual Studio 2022** veya **VS Code**

## API Kullanımı

### Base URL
```
https://localhost:7175
```

### Endpoints

#### 1. Kura Çek
```http
POST /api/draw
Content-Type: application/json

{
  "drawerFirstName": "Sercan",
  "drawerLastName": "Karakuyu",
  "numberOfGroups": 8
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Kura başarıyla çekildi",
  "data": {
    "id": 1,
    "drawerFirstName": "Sercan",
    "drawerLastName": "Karakuyu",
    "drawerFullName": "Sercan Karakuyu",
    "numberOfGroups": 8,
    "drawDate": "2025-11-17T16:30:00Z",
    "groups": [
      {
        "groupName": "A",
        "teams": [
          { "name": "Adesso İstanbul" },
          { "name": "Adesso Berlin" },
          { "name": "Adesso Paris" },
          { "name": "Adesso Amsterdam" }
        ]
      }
    ]
  },
  "errors": []
}
```

#### 2. Tüm Kuraları Listele
```http
GET /api/draw
```

#### 3. Belirli Kurayı Getir
```http
GET /api/draw/1
```

### .http File Kullanımı

Proje root'unda `AdessoTurkey.WebAPI.http` dosyası hazır test senaryoları içerir.

Visual Studio'da `.http` dosyasını açın, her isteğin üstündeki **"Send Request"** butonuna tıklayın.

---
