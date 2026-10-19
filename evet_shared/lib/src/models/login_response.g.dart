// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'login_response.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

LoginResponse _$LoginResponseFromJson(Map<String, dynamic> json) =>
    LoginResponse(
      token: json['token'] as String,
      datumIsteka: DateTime.parse(json['datumIsteka'] as String),
      korisnik: Korisnici.fromJson(json['korisnik'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$LoginResponseToJson(LoginResponse instance) =>
    <String, dynamic>{
      'token': instance.token,
      'datumIsteka': instance.datumIsteka.toIso8601String(),
      'korisnik': instance.korisnik.toJson(),
    };
