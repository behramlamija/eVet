import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class VrsteLjubimacaProvider extends BaseProvider<VrstaLjubimca> {
  VrsteLjubimacaProvider(ApiClient api) : super(api, 'VrsteLjubimaca');

  @override
  VrstaLjubimca fromJson(Map<String, dynamic> json) => VrstaLjubimca.fromJson(json);
}
