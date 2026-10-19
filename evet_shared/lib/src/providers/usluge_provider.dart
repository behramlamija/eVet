import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class UslugeProvider extends BaseProvider<Usluge> {
  UslugeProvider(ApiClient api) : super(api, 'Usluga');

  @override
  Usluge fromJson(Map<String, dynamic> json) => Usluge.fromJson(json);
}
