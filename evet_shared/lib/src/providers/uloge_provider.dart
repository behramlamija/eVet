import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class UlogeProvider extends BaseProvider<Uloga> {
  UlogeProvider(ApiClient api) : super(api, 'Uloge');

  @override
  Uloga fromJson(Map<String, dynamic> json) => Uloga.fromJson(json);
}
