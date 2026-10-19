import '../api/api_client.dart';
import '../models/models.dart';
import '../models/paged_result.dart';
import 'base_provider.dart';

class PromocijeProvider extends BaseProvider<Promocije> {
  PromocijeProvider(ApiClient api) : super(api, 'Promocije');

  @override
  Promocije fromJson(Map<String, dynamic> json) => Promocije.fromJson(json);

  Future<AktiviranePromocije> aktiviraj(int promocijaId) async {
    final rezultat = AktiviranePromocije.fromJson(await api.post('$endpoint/$promocijaId/aktiviraj') as Map<String, dynamic>);
    notifyListeners();
    return rezultat;
  }

  Future<PagedResult<AktiviranePromocije>> moje({int page = 0, int pageSize = BaseProvider.defaultPageSize}) async {
    final json = await api.get('$endpoint/moje', query: {'Page': page, 'PageSize': pageSize});
    return PagedResult.fromJson(json as Map<String, dynamic>, AktiviranePromocije.fromJson);
  }
}
