import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class VeterinariProvider extends BaseProvider<KorisnikOsnovno> {
  VeterinariProvider(ApiClient api) : super(api, 'Veterinari');

  @override
  KorisnikOsnovno fromJson(Map<String, dynamic> json) => KorisnikOsnovno.fromJson(json);
}
