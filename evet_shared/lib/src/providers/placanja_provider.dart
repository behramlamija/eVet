import '../api/api_client.dart';
import '../models/models.dart';

/// PayPal plaćanje termina. Iznos određuje i naplatu potvrđuje server.
class PlacanjaProvider {
  PlacanjaProvider(this._api);

  final ApiClient _api;

  Future<PayPalNarudzba> zapocniPayPal(int terminId) async {
    final json = await _api.post('Placanja/termin/$terminId/paypal');
    return PayPalNarudzba.fromJson(json as Map<String, dynamic>);
  }

  Future<Termini> potvrdiPayPal(int uplataId) async {
    final json = await _api.post('Placanja/$uplataId/potvrdi');
    return Termini.fromJson(json as Map<String, dynamic>);
  }
}
