# Geliştirme İlerlemesi

Hedef: 20 farklı gerçek takvim gününde en az 20 anlamlı commit. Tarihler gerçek commit tarihleridir; kesin hash ve tarihler için `git log` esas alınır. Bir commit kendi hash değerini içeremeyeceği için hash bir sonraki güncellemede eklenir.

| Gün | Çalışma Tarihi | Commit Tarihi | Commit | Yapılan İş |
|-----|----------------|---------------|--------|------------|
| 1 | 2026-11-20 | 2026-11-20 | `44ce401` | README.md ve .gitignore eklendi |
| 2 | 2026-11-20 | 2026-11-21 | (bu commit) | MVC solution yapısı ve EF Core veritabanı temeli |

## Gün 2 Ayrıntıları

**MVC kurulumu**
- ASP.NET Core MVC (.NET 10) projesi `src/VetClinic.Web`, xUnit test projesi `tests/VetClinic.Tests`
- `VetClinic.slnx`, `global.json` (SDK 10.0.300), `CLAUDE.md`, `.claude/settings.json`

**Veritabanı temeli (EF Core Code First, SQL Server LocalDB)**
- Paketler: EF Core SqlServer, Identity EF Core, EF Core Design — 10.0.15
- Varlıklar: ApplicationUser, Owner, Pet, Veterinarian, Specialty, VeterinarianSpecialty, ClinicService, VeterinarianClinicService, ClinicWorkingHour, VeterinarianAvailability, VeterinarianTimeOff, Appointment, ExaminationRecord, Vaccination, McpAccessToken
- `VetClinicDbContext` (Identity entegre), Fluent API: anahtarlar, ilişkiler, benzersiz/filtreli indeksler, check constraint'ler, decimal hassasiyeti, RowVersion, silme kuralları
- Roller: Admin, Veterinarian, Member
- Randevu anları UTC, haftalık çalışma saatleri yerel saat (Europe/Istanbul)

**Doğrulama**
- `dotnet build`: başarılı (0 uyarı, 0 hata)
- `dotnet test`: 30/30 test başarılı (model yapılandırması ve saat dilimi testleri)
- HTTPS: `https://localhost:7132/` → 200 (güvenilir geliştirme sertifikası ile)

**Henüz yapılmadı:** migration'lar, Admin hesabı oluşturma, randevu servisleri, controller'lar, REST API, MCP.

## Durum

🚧 Geliştirme devam etmektedir.
