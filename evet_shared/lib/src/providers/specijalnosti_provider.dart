import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class SpecijalnostiProvider extends BaseProvider<Specijalnost> {
  SpecijalnostiProvider(ApiClient api) : super(api, 'Specijalnosti');

  @override
  Specijalnost fromJson(Map<String, dynamic> json) => Specijalnost.fromJson(json);
}
