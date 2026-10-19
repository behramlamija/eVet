import 'package:json_annotation/json_annotation.dart';

part 'reset_lozinke_request.g.dart';

@JsonSerializable(explicitToJson: true)
class ResetLozinkeRequest {
  final String email;
  final String kod;
  final String novaLozinka;
  final String novaLozinkaPotvrda;

  const ResetLozinkeRequest({
    required this.email,
    required this.kod,
    required this.novaLozinka,
    required this.novaLozinkaPotvrda,
  });

  factory ResetLozinkeRequest.fromJson(Map<String, dynamic> json) => _$ResetLozinkeRequestFromJson(json);

  Map<String, dynamic> toJson() => _$ResetLozinkeRequestToJson(this);
}
