import 'package:json_annotation/json_annotation.dart';

part 'login_request.g.dart';

@JsonSerializable(explicitToJson: true)
class LoginRequest {
  final String korisnickoIme;
  final String lozinka;

  const LoginRequest({
    required this.korisnickoIme,
    required this.lozinka,
  });

  factory LoginRequest.fromJson(Map<String, dynamic> json) => _$LoginRequestFromJson(json);

  Map<String, dynamic> toJson() => _$LoginRequestToJson(this);
}
