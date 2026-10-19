// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'reset_lozinke_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ResetLozinkeRequest _$ResetLozinkeRequestFromJson(Map<String, dynamic> json) =>
    ResetLozinkeRequest(
      email: json['email'] as String,
      kod: json['kod'] as String,
      novaLozinka: json['novaLozinka'] as String,
      novaLozinkaPotvrda: json['novaLozinkaPotvrda'] as String,
    );

Map<String, dynamic> _$ResetLozinkeRequestToJson(
  ResetLozinkeRequest instance,
) => <String, dynamic>{
  'email': instance.email,
  'kod': instance.kod,
  'novaLozinka': instance.novaLozinka,
  'novaLozinkaPotvrda': instance.novaLozinkaPotvrda,
};
