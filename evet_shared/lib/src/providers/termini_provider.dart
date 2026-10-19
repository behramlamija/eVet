import '../api/api_client.dart';
import '../models/models.dart';
import '../utils/formatters.dart';
import 'base_provider.dart';

/// Termini se ne uređuju niti brišu direktno; mijenja im se status kroz akcije na backendu.
class TerminiProvider extends BaseProvider<Termini> {
  TerminiProvider(ApiClient api) : super(api, 'Termini');

  @override
  Termini fromJson(Map<String, dynamic> json) => Termini.fromJson(json);

  Future<Termini> kreiraj(TerminInsertRequest request) => insert(request.toJson());

  Future<Termini> potvrdi(int id) => _akcija(id, 'potvrdi');

  Future<Termini> zavrsi(int id) => _akcija(id, 'zavrsi');

  Future<Termini> odbij(int id, String razlog) => _akcija(id, 'odbij', PromjenaStatusaRequest(razlog: razlog));

  Future<Termini> otkazi(int id, String razlog) => _akcija(id, 'otkazi', PromjenaStatusaRequest(razlog: razlog));

  /// Pravila zakazivanja sa servera (cijena prevoza, rok za otkazivanje, radno vrijeme).
  Future<TerminiKonfiguracija> konfiguracija() async =>
      TerminiKonfiguracija.fromJson(await api.get('$endpoint/konfiguracija') as Map<String, dynamic>);

  /// Slobodna vremena početka ("HH:mm:ss") za veterinara, dan i ukupno trajanje usluga u minutama.
  Future<List<String>> slobodnaVremena({required int veterinarId, required DateTime datum, required int trajanje}) async {
    final json = await api.get('$endpoint/slobodna-vremena', query: {
      'veterinarId': veterinarId,
      'datum': formatDatumApi(datum),
      'trajanje': trajanje,
    });
    return (json as List).cast<String>();
  }

  Future<Termini> _akcija(int id, String akcija, [PromjenaStatusaRequest? request]) async {
    final rezultat = fromJson(await api.put('$endpoint/$id/$akcija', body: request?.toJson()) as Map<String, dynamic>);
    notifyListeners();
    return rezultat;
  }
}
