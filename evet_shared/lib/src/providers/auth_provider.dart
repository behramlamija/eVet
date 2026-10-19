import 'package:flutter/foundation.dart';

import '../api/api_client.dart';
import '../api/api_exception.dart';
import '../models/models.dart';

/// Stanje prijave. Token se drži samo u memoriji i šalje uz svaki zahtjev preko [ApiClient].
class AuthProvider extends ChangeNotifier {
  AuthProvider(this._api, {required this.dozvoljeneUloge}) {
    _api.onUnauthorized = _sesijaIstekla;
  }

  final ApiClient _api;

  /// Uloge kojima je dozvoljen rad u ovoj aplikaciji (desktop: vlasnik i veterinar, mobilna: klijent).
  final List<String> dozvoljeneUloge;

  Korisnici? _korisnik;
  String? _porukaOdjave;

  Korisnici? get korisnik => _korisnik;

  bool get jePrijavljen => _korisnik != null;

  /// Razlog automatske odjave (npr. istekla sesija), za prikaz na ekranu prijave.
  String? get porukaOdjave => _porukaOdjave;

  bool imaUlogu(String uloga) => _korisnik?.uloge.any((u) => u.naziv == uloga) ?? false;

  Future<void> login(String korisnickoIme, String lozinka) async {
    final json = await _api.post('Auth/login', body: LoginRequest(korisnickoIme: korisnickoIme, lozinka: lozinka).toJson());
    final odgovor = LoginResponse.fromJson(json as Map<String, dynamic>);

    if (!odgovor.korisnik.uloge.any((u) => dozvoljeneUloge.contains(u.naziv))) {
      _api.setToken(odgovor.token);
      await _api.post('Auth/logout');
      _api.setToken(null);
      throw const ApiException('Nemate pristup ovoj aplikaciji sa ovim korisničkim računom.');
    }

    _api.setToken(odgovor.token);
    _korisnik = odgovor.korisnik;
    _porukaOdjave = null;
    notifyListeners();
  }

  Future<Korisnici> registracija(RegistracijaRequest request) async {
    return Korisnici.fromJson(await _api.post('Auth/registracija', body: request.toJson()) as Map<String, dynamic>);
  }

  /// Šalje kod za promjenu lozinke na e-mail (ako je e-mail registrovan).
  Future<void> zatraziResetLozinke(String email) => _api.post('Auth/zaboravljena-lozinka', body: ZaboravljenaLozinkaRequest(email: email).toJson());

  Future<void> resetujLozinku(ResetLozinkeRequest request) => _api.post('Auth/reset-lozinke', body: request.toJson());

  /// Odjava poništava token i na serveru, ne samo lokalno.
  Future<void> logout() async {
    try {
      await _api.post('Auth/logout');
    } finally {
      _ocisti();
    }
  }

  /// Osvježava podatke prijavljenog korisnika nakon izmjene profila.
  void azurirajKorisnika(Korisnici korisnik) {
    _korisnik = korisnik;
    notifyListeners();
  }

  void _sesijaIstekla() {
    _porukaOdjave = 'Vaša sesija je istekla. Prijavite se ponovo.';
    _ocisti();
  }

  void _ocisti() {
    _api.setToken(null);
    _korisnik = null;
    notifyListeners();
  }
}
