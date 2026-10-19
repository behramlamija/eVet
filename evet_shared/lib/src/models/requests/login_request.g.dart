// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'login_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

LoginRequest _$LoginRequestFromJson(Map<String, dynamic> json) => LoginRequest(
  korisnickoIme: json['korisnickoIme'] as String,
  lozinka: json['lozinka'] as String,
);

Map<String, dynamic> _$LoginRequestToJson(LoginRequest instance) =>
    <String, dynamic>{
      'korisnickoIme': instance.korisnickoIme,
      'lozinka': instance.lozinka,
    };
