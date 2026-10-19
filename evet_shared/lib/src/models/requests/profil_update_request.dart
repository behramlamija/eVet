import 'package:json_annotation/json_annotation.dart';

part 'profil_update_request.g.dart';

@JsonSerializable(explicitToJson: true)
class ProfilUpdateRequest {
  final String ime;
  final String prezime;
  final String email;
  final String? telefon;

  const ProfilUpdateRequest({
    required this.ime,
    required this.prezime,
    required this.email,
    this.telefon,
  });

  factory ProfilUpdateRequest.fromJson(Map<String, dynamic> json) => _$ProfilUpdateRequestFromJson(json);

  Map<String, dynamic> toJson() => _$ProfilUpdateRequestToJson(this);
}
