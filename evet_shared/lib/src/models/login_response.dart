import 'package:json_annotation/json_annotation.dart';
import 'korisnici.dart';

part 'login_response.g.dart';

@JsonSerializable(explicitToJson: true)
class LoginResponse {
  final String token;
  final DateTime datumIsteka;
  final Korisnici korisnik;

  const LoginResponse({
    required this.token,
    required this.datumIsteka,
    required this.korisnik,
  });

  factory LoginResponse.fromJson(Map<String, dynamic> json) => _$LoginResponseFromJson(json);

  Map<String, dynamic> toJson() => _$LoginResponseToJson(this);
}
