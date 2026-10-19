import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class TerapijeProvider extends BaseProvider<Terapije> {
  TerapijeProvider(ApiClient api) : super(api, 'Terapije');

  @override
  Terapije fromJson(Map<String, dynamic> json) => Terapije.fromJson(json);
}
