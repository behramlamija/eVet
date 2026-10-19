import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/foundation.dart';

/// Usluge odabrane za rezervaciju, kupon i prevoz. Konačnu cijenu uvijek računa server pri zakazivanju;
/// ovdje se računa samo za prikaz korisniku.
class KorpaProvider extends ChangeNotifier {
  final List<Usluge> _stavke = [];
  AktiviranePromocije? _kupon;
  bool _jePrevoz = false;
  String _adresaPrevoza = '';
  int? _gradPrevozaId;

  List<Usluge> get stavke => List.unmodifiable(_stavke);

  bool get jePrazna => _stavke.isEmpty;

  int get brojStavki => _stavke.length;

  AktiviranePromocije? get kupon => _kupon;

  bool get jePrevoz => _jePrevoz;

  String get adresaPrevoza => _adresaPrevoza;

  int? get gradPrevozaId => _gradPrevozaId;

  bool sadrzi(int uslugaId) => _stavke.any((u) => u.uslugaId == uslugaId);

  double get iznosUsluga => _stavke.fold(0, (zbir, u) => zbir + u.cijena);

  int get ukupnoTrajanje => _stavke.fold(0, (zbir, u) => zbir + u.trajanje);

  /// Popust se odnosi samo na uslugu za koju je kupon izdan i samo ako je ona u korpi.
  double get iznosPopusta {
    final kupon = _kupon;
    if (kupon == null) return 0;
    final usluga = _stavke.where((u) => u.uslugaId == kupon.uslugaId).firstOrNull;
    if (usluga == null) return 0;
    return (usluga.cijena * kupon.promocijaPopust / 100 * 100).roundToDouble() / 100;
  }

  double cijenaPrevoza(double tarifa) => _jePrevoz ? tarifa : 0;

  double ukupno(double tarifaPrevoza) => (iznosUsluga - iznosPopusta + cijenaPrevoza(tarifaPrevoza)).clamp(0, double.infinity);

  void dodaj(Usluge usluga) {
    if (sadrzi(usluga.uslugaId)) return;
    _stavke.add(usluga);
    notifyListeners();
  }

  void ukloni(int uslugaId) {
    _stavke.removeWhere((u) => u.uslugaId == uslugaId);
    if (_kupon != null && !_stavke.any((u) => u.uslugaId == _kupon!.uslugaId)) {
      _kupon = null;
    }
    notifyListeners();
  }

  void postaviKupon(AktiviranePromocije? kupon) {
    _kupon = kupon;
    notifyListeners();
  }

  void postaviPrevoz({required bool jePrevoz, String? adresa, int? gradId}) {
    _jePrevoz = jePrevoz;
    if (adresa != null) _adresaPrevoza = adresa;
    _gradPrevozaId = gradId ?? (jePrevoz ? _gradPrevozaId : null);
    notifyListeners();
  }

  void isprazni() {
    _stavke.clear();
    _kupon = null;
    _jePrevoz = false;
    _adresaPrevoza = '';
    _gradPrevozaId = null;
    notifyListeners();
  }
}
