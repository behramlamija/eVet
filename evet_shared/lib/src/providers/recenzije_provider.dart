import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class RecenzijeProvider extends BaseProvider<Recenzije> {
  RecenzijeProvider(ApiClient api) : super(api, 'Recenzije');

  @override
  Recenzije fromJson(Map<String, dynamic> json) => Recenzije.fromJson(json);

  Future<Recenzije> ostavi(RecenzijaInsertRequest request) => insert(request.toJson());

  Future<Recenzije> odgovori(int recenzijaId, String komentar) async {
    final json = await api.post('$endpoint/$recenzijaId/odgovori', body: RecenzijaOdgovorRequest(komentar: komentar).toJson());
    notifyListeners();
    return fromJson(json as Map<String, dynamic>);
  }
}
