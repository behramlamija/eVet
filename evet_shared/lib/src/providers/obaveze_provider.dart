import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class ObavezeProvider extends BaseProvider<Obaveze> {
  ObavezeProvider(ApiClient api) : super(api, 'Obaveze');

  @override
  Obaveze fromJson(Map<String, dynamic> json) => Obaveze.fromJson(json);
}
