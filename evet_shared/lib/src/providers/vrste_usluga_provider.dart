import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class VrsteUslugaProvider extends BaseProvider<VrstaUsluge> {
  VrsteUslugaProvider(ApiClient api) : super(api, 'VrstaUsluge');

  @override
  VrstaUsluge fromJson(Map<String, dynamic> json) => VrstaUsluge.fromJson(json);
}
