/// Validacija na klijentu; pravila i poruke odgovaraju ValidacijskaPravila na backendu.
class Validatori {
  Validatori._();

  static final _email = RegExp(r'^[^@\s]+@[^@\s]+\.[^@\s]+$');
  static final _telefon = RegExp(r'^\+?[0-9][0-9 /-]{5,19}$');
  static final _korisnickoIme = RegExp(r'^[a-zA-Z0-9._]{3,50}$');
  static final _lozinka = RegExp(r'^(?=.*[A-Za-z])(?=.*\d).{8,100}$');
  static final _postanskiBroj = RegExp(r'^\d{5}$');
  static final _kodPromocije = RegExp(r'^[A-Z0-9-]{3,50}$');

  static String? obavezno(String? v, String naziv) => (v == null || v.trim().isEmpty) ? 'Polje "$naziv" je obavezno.' : null;

  static String? Function(String?) tekst(String naziv, {bool obavezan = true, int maksimalnaDuzina = 255}) {
    return (v) {
      if (obavezan) {
        if (obavezno(v, naziv) case final greska?) return greska;
      }
      if (v != null && v.trim().length > maksimalnaDuzina) return 'Polje "$naziv" može imati najviše $maksimalnaDuzina znakova.';
      return null;
    };
  }

  static String? email(String? v) {
    if (obavezno(v, 'Email') case final greska?) return greska;
    return _email.hasMatch(v!.trim()) ? null : 'Unesite validnu email adresu u formatu ime@domena.com.';
  }

  static String? telefonOpcionalno(String? v) {
    if (v == null || v.trim().isEmpty) return null;
    return _telefon.hasMatch(v.trim()) ? null : 'Unesite validan broj telefona, npr. +387 61 123 456 ili 061/123-456.';
  }

  static String? korisnickoIme(String? v) {
    if (obavezno(v, 'Korisničko ime') case final greska?) return greska;
    return _korisnickoIme.hasMatch(v!.trim()) ? null : 'Korisničko ime mora imati 3-50 znakova (slova, brojevi, tačka i donja crta).';
  }

  static String? lozinka(String? v) {
    if (v == null || v.isEmpty) return 'Unesite lozinku.';
    return _lozinka.hasMatch(v) ? null : 'Lozinka mora imati 8-100 znakova, uključujući barem jedno slovo i jedan broj.';
  }

  /// Lozinka koja se validira samo ako je unesena (npr. pri uređivanju korisnika).
  static String? lozinkaOpcionalno(String? v) => (v == null || v.isEmpty) ? null : lozinka(v);

  static String? Function(String?) potvrdaLozinke(String Function() lozinka) {
    return (v) => (v ?? '') == lozinka() ? null : 'Lozinka i potvrda lozinke se ne poklapaju.';
  }

  static String? postanskiBroj(String? v) {
    if (obavezno(v, 'Poštanski broj') case final greska?) return greska;
    return _postanskiBroj.hasMatch(v!.trim()) ? null : 'Poštanski broj mora imati tačno 5 cifara, npr. 88000.';
  }

  static String? kodPromocije(String? v) {
    if (obavezno(v, 'Kod') case final greska?) return greska;
    return _kodPromocije.hasMatch(v!.trim())
        ? null
        : 'Kod mora imati 3-50 znakova: velika slova, brojevi i crtica, npr. LJETO-2026.';
  }

  /// Decimalni broj u rasponu; prihvata zarez i tačku kao decimalni separator.
  static String? Function(String?) broj(String naziv, {required num min, required num max, bool obavezan = true, String jedinica = ''}) {
    return (v) {
      if (v == null || v.trim().isEmpty) return obavezan ? 'Polje "$naziv" je obavezno.' : null;
      final broj = parsirajBroj(v);
      if (broj == null) return 'Unesite broj za polje "$naziv", npr. 12,50.';
      if (broj < min || broj > max) return 'Polje "$naziv" mora biti između $min i $max$jedinica.';
      return null;
    };
  }

  static double? parsirajBroj(String? v) => v == null ? null : double.tryParse(v.trim().replaceAll(',', '.'));

  static String? odabir(Object? v, String naziv) => v == null ? 'Odaberite vrijednost za polje "$naziv".' : null;
}
