import '../api/api_client.dart';
import '../models/models.dart';

/// Slike se dohvaćaju preko URL-a (uz token u zaglavlju), a ne kao base64 u listama.
class SlikeProvider {
  SlikeProvider(this._api);

  final ApiClient _api;

  String url(TipSlike tip, int id, {int? verzija}) =>
      _api.uri('Slike/${tip.name}/$id', verzija == null ? null : {'v': verzija}).toString();

  /// Zaglavlja za Image.network, jer je endpoint za slike zaštićen.
  Map<String, String> get headers => _api.authHeaders;

  Future<void> postavi(TipSlike tip, int id, {required List<int> bytes, required String nazivDatoteke, required String contentType}) {
    return _api.putMultipart('Slike/${tip.name}/$id', polje: 'slika', bytes: bytes, nazivDatoteke: nazivDatoteke, contentType: contentType);
  }

  Future<void> obrisi(TipSlike tip, int id) => _api.delete('Slike/${tip.name}/$id');
}
