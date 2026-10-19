import 'package:json_annotation/json_annotation.dart';

part 'registracija_request.g.dart';

@JsonSerializable(explicitToJson: true)
class RegistracijaRequest {
  final String ime;
  final String prezime;
  final String korisnickoIme;
  final String email;
  final String? telefon;
  final String lozinka;
  final String lozinkaPotvrda;

  const RegistracijaRequest({
    required this.ime,
    required this.prezime,
    required this.korisnickoIme,
    required this.email,
    this.telefon,
    required this.lozinka,
    required this.lozinkaPotvrda,
  });

  factory RegistracijaRequest.fromJson(Map<String, dynamic> json) => _$RegistracijaRequestFromJson(json);

  Map<String, dynamic> toJson() => _$RegistracijaRequestToJson(this);
}
