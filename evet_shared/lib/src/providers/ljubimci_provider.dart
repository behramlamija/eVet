import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class LjubimciProvider extends BaseProvider<Ljubimci> {
  LjubimciProvider(ApiClient api) : super(api, 'Ljubimci');

  @override
  Ljubimci fromJson(Map<String, dynamic> json) => Ljubimci.fromJson(json);
}
