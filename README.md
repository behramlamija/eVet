# eVet (PetCity)

Seminarski rad iz predmeta Razvoj softvera II na Fakultetu informacijskih tehnologija u Mostaru.

Sistem za veterinarsku stanicu: desktop aplikacija za vlasnika i veterinare, mobilna aplikacija za klijente i REST API.

## Struktura

| Folder | Sadržaj |
|---|---|
| `eVet.API` | ASP.NET Core Web API (kontroleri, autentifikacija, obrada grešaka) |
| `eVet.Services` | Poslovna logika, EF Core `DbContext`, migracije i seed podaci |
| `eVet.Model` | DTO, request i search objekti, enumi i konstante |
| `eVet.Messaging` | Zajednički ugovori za RabbitMQ (poruke, redovi, publisher) |
| `eVet.Worker` | Pomoćni servis koji preuzima poruke iz RabbitMQ-a i šalje e-mailove |
| `evet_shared` | Zajednički Flutter paket: API klijent, modeli, provideri, tema |
| `evet_desktop` | Flutter Windows aplikacija (vlasnik i veterinari) |
| `evet_mobile` | Flutter Android aplikacija (klijenti) |

## Pokretanje

### Backend

1. Raspakovati `.env-tajne.zip` (šifra je predana na DL) u root folder repozitorija, tako da `.env` bude pored `docker-compose.yml`.
   Sva konfiguracija (konekcijski string, JWT, RabbitMQ, SMTP, PayPal) nalazi se samo u `.env` datoteci, a ne u kodu ili `appsettings.json`.
2. Pokrenuti API:

   ```bash
   cd eVet.API
   dotnet run --launch-profile http
   ```

   API je dostupan na `http://localhost:5103`, a Swagger na `http://localhost:5103/swagger`.
   Pri pokretanju se automatski kreira baza `210210`, primjenjuju migracije i upisuju testni podaci.

### Docker (API, baza, RabbitMQ, Worker, Mailpit)

```bash
docker compose up -d --build
```

| Servis | Adresa |
|---|---|
| API i Swagger | `http://localhost:5103/swagger` |
| RabbitMQ management | `http://localhost:15672` (podaci iz `.env`) |
| Mailpit (pregled poslanih e-mailova) | `http://localhost:8025` |

API događaje (registracija, promjene termina, recenzije, zaboravljena lozinka) šalje u RabbitMQ red `evet.emailovi`.
Worker iz tog reda šalje e-mailove preko SMTP-a; neuspjelo slanje ponavlja 4 puta (1, 2, 4, 8 s), a zatim poruku prebacuje u red `evet.emailovi.neuspjeli`.
U razvoju se kao SMTP server koristi Mailpit, pa se svi poslani e-mailovi (npr. kod za promjenu lozinke) vide na `http://localhost:8025`.

### PayPal plaćanje (sandbox)

Klijent u mobilnoj aplikaciji može platiti termin PayPalom (način plaćanja „PayPal” pri zakazivanju ili dugme „Plati PayPalom” u detaljima termina).

- Plaćanje se odobrava u PayPal prozoru unutar aplikacije. Server zatim sam izvršava i provjerava naplatu (capture) i tek tada označava termin kao plaćen.
- Iznos određuje server iz cijene termina. PayPal ne podržava KM, pa se naplaćuje u valuti `PayPal__Valuta` po kursu `PayPal__KursKonverzije` iz `.env`.
- Kad se plaćeni termin otkaže ili odbije, stvarno naplaćeni iznos se automatski vraća (refund) na PayPal račun klijenta.
- Za plaćanje se koristi sandbox **Personal** račun. Njegov e-mail i lozinka navedeni su na kraju `.env` datoteke.

### Pokretanje iz release buildova

U GitHub Release-u se nalazi arhiva `fit-build-<datum>.zip` sa gotovim aplikacijama. Prije njihovog pokretanja backend mora raditi (`docker compose up -d --build`).

| Aplikacija | Putanja u arhivi | Pokretanje |
|---|---|---|
| Mobilna (Android) | `evet_mobile/build/app/outputs/flutter-apk/app-release.apk` | Obrisati staru verziju iz emulatora, prevući `.apk` u Android emulator (AVD) i pokrenuti aplikaciju **PetCity**. API adresa je `http://10.0.2.2:5103`. |
| Desktop (Windows) | `evet_desktop/build/windows/x64/runner/Release/` | Pokrenuti `evet_desktop.exe`. API adresa je `http://localhost:5103`. |

### Desktop aplikacija (iz izvornog koda)

```bash
cd evet_desktop
flutter run -d windows --dart-define=API_BASE_URL=http://localhost:5103
```

### Mobilna aplikacija (iz izvornog koda, Android emulator)

```bash
cd evet_mobile
flutter run --dart-define=API_BASE_URL=http://10.0.2.2:5103
```

## Pristupni podaci

Lozinka za sve testne korisnike je `test`.

| Kontekst | Korisničko ime | Lozinka |
|---|---|---|
| Desktop verzija (vlasnik) | `desktop` | `test` |
| Desktop verzija (veterinar) | `veterinar` | `test` |
| Mobilna verzija (klijent) | `mobile` | `test` |

Dodatni testni korisnici (lozinka `test`): veterinari `veterinar2` i `veterinar3`, klijent `klijent2`.
