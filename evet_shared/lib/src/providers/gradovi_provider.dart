import '../api/api_client.dart';
import '../models/models.dart';
import 'base_provider.dart';

class GradoviProvider extends BaseProvider<Grad> {
  GradoviProvider(ApiClient api) : super(api, 'Gradovi');

  @override
  Grad fromJson(Map<String, dynamic> json) => Grad.fromJson(json);
}
