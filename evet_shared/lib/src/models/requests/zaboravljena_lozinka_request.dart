import 'package:json_annotation/json_annotation.dart';

part 'zaboravljena_lozinka_request.g.dart';

@JsonSerializable(explicitToJson: true)
class ZaboravljenaLozinkaRequest {
  final String email;

  const ZaboravljenaLozinkaRequest({
    required this.email,
  });

  factory ZaboravljenaLozinkaRequest.fromJson(Map<String, dynamic> json) => _$ZaboravljenaLozinkaRequestFromJson(json);

  Map<String, dynamic> toJson() => _$ZaboravljenaLozinkaRequestToJson(this);
}
