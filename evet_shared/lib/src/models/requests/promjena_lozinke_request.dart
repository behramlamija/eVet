import 'package:json_annotation/json_annotation.dart';

part 'promjena_lozinke_request.g.dart';

@JsonSerializable(explicitToJson: true)
class PromjenaLozinkeRequest {
  final String staraLozinka;
  final String novaLozinka;
  final String novaLozinkaPotvrda;

  const PromjenaLozinkeRequest({
    required this.staraLozinka,
    required this.novaLozinka,
    required this.novaLozinkaPotvrda,
  });

  factory PromjenaLozinkeRequest.fromJson(Map<String, dynamic> json) => _$PromjenaLozinkeRequestFromJson(json);

  Map<String, dynamic> toJson() => _$PromjenaLozinkeRequestToJson(this);
}
