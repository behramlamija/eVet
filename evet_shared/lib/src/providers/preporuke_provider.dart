import '../api/api_client.dart';
import '../models/models.dart';

/// Sistem preporuke "Korisnici također rezervišu".
class PreporukeProvider {
  PreporukeProvider(this._api);

  final ApiClient _api;

  /// Usluge koje se najčešće rezervišu zajedno s navedenim uslugama, uz objašnjenje preporuke.
  Future<List<PreporukeUsluga>> zaUsluge(Iterable<int> uslugeIds) async {
    final json = await _api.get('Preporuke', query: {'uslugeIds': uslugeIds.toList()});
    return (json as List).map((e) => PreporukeUsluga.fromJson(e as Map<String, dynamic>)).toList();
  }
}
