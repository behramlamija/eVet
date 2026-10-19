import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class KorisniciProvider extends BaseProvider<Korisnici> {
  KorisniciProvider(ApiClient api) : super(api, 'Korisnici');

  @override
  Korisnici fromJson(Map<String, dynamic> json) => Korisnici.fromJson(json);
}
