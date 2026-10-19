import '../api/api_client.dart';
import '../models/models.dart';

/// Podaci prijavljenog korisnika; ID se uzima iz tokena na backendu.
class ProfilProvider {
  ProfilProvider(this._api);

  final ApiClient _api;

  Future<Korisnici> get() async => Korisnici.fromJson(await _api.get('Profil') as Map<String, dynamic>);

  Future<Korisnici> update(ProfilUpdateRequest request) async =>
      Korisnici.fromJson(await _api.put('Profil', body: request.toJson()) as Map<String, dynamic>);

  Future<void> promijeniLozinku(PromjenaLozinkeRequest request) => _api.put('Profil/lozinka', body: request.toJson());
}
