import 'dart:async';

import '../api/api_client.dart';
import '../api/api_exception.dart';
import '../models/models.dart';
import 'base_provider.dart';

/// Notifikacije se automatski osvježavaju periodičnim upitom (polling), bez ručnog refresha.
class ObavijestiProvider extends BaseProvider<Obavijesti> {
  ObavijestiProvider(ApiClient api, {this.interval = const Duration(seconds: 30)}) : super(api, 'Obavijesti');

  final Duration interval;
  Timer? _timer;
  int _brojNeprocitanih = 0;

  int get brojNeprocitanih => _brojNeprocitanih;

  @override
  Obavijesti fromJson(Map<String, dynamic> json) => Obavijesti.fromJson(json);

  void pokreniOsvjezavanje() {
    _timer?.cancel();
    osvjezi();
    _timer = Timer.periodic(interval, (_) => osvjezi());
  }

  void zaustaviOsvjezavanje() {
    _timer?.cancel();
    _timer = null;
    _brojNeprocitanih = 0;
  }

  Future<void> osvjezi() async {
    try {
      final broj = await api.get('$endpoint/broj-neprocitanih') as int;
      if (broj != _brojNeprocitanih) {
        _brojNeprocitanih = broj;
        notifyListeners();
      }
    } on ApiException {
      // Neuspjelo pozadinsko osvježavanje ne prekida rad; pokušava se ponovo u sljedećem intervalu.
    }
  }

  Future<void> oznaciProcitano(int id) async {
    await api.put('$endpoint/$id/procitano');
    await osvjezi();
    notifyListeners();
  }

  Future<void> oznaciSveProcitano() async {
    await api.put('$endpoint/procitaj-sve');
    await osvjezi();
    notifyListeners();
  }

  @override
  void dispose() {
    _timer?.cancel();
    super.dispose();
  }
}
