import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class NaciniPlacanjaProvider extends BaseProvider<NacinPlacanja> {
  NaciniPlacanjaProvider(ApiClient api) : super(api, 'NaciniPlacanja');

  @override
  NacinPlacanja fromJson(Map<String, dynamic> json) => NacinPlacanja.fromJson(json);
}
