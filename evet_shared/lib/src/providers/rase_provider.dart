import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class RaseProvider extends BaseProvider<Rasa> {
  RaseProvider(ApiClient api) : super(api, 'Rase');

  @override
  Rasa fromJson(Map<String, dynamic> json) => Rasa.fromJson(json);
}
