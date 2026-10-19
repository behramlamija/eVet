import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class KlijentiProvider extends BaseProvider<KorisnikOsnovno> {
  KlijentiProvider(ApiClient api) : super(api, 'Klijenti');

  @override
  KorisnikOsnovno fromJson(Map<String, dynamic> json) => KorisnikOsnovno.fromJson(json);
}
