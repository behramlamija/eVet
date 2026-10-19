import '../api/api_client.dart';
import '../models/models.dart';
import '../models/paged_result.dart';
import 'base_provider.dart';

class StatistikaProvider {
  StatistikaProvider(this._api);

  final ApiClient _api;

  Future<StatistikaPoslovanja> poslovanje({DateTime? datumOd, DateTime? datumDo}) async {
    final json = await _api.get('Statistika/poslovanje', query: {'datumOd': datumOd, 'datumDo': datumDo});
    return StatistikaPoslovanja.fromJson(json as Map<String, dynamic>);
  }

  Future<List<StatistikaPeriod>> poPeriodima(GrupisanjeStatistike grupisanje, {int? godina}) async {
    final json = await _api.get('Statistika/po-periodima', query: {'grupisanje': grupisanje, 'godina': godina});
    return (json as List).map((e) => StatistikaPeriod.fromJson(e as Map<String, dynamic>)).toList();
  }

  Future<PagedResult<StatistikaVeterinara>> veterinari({String? fts, int page = 0, int pageSize = BaseProvider.defaultPageSize}) async {
    final json = await _api.get('Statistika/veterinari', query: {'FTS': fts, 'Page': page, 'PageSize': pageSize});
    return PagedResult.fromJson(json as Map<String, dynamic>, StatistikaVeterinara.fromJson);
  }
}
