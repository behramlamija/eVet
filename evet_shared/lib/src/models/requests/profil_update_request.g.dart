// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'profil_update_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ProfilUpdateRequest _$ProfilUpdateRequestFromJson(Map<String, dynamic> json) =>
    ProfilUpdateRequest(
      ime: json['ime'] as String,
      prezime: json['prezime'] as String,
      email: json['email'] as String,
      telefon: json['telefon'] as String?,
    );

Map<String, dynamic> _$ProfilUpdateRequestToJson(
  ProfilUpdateRequest instance,
) => <String, dynamic>{
  'ime': instance.ime,
  'prezime': instance.prezime,
  'email': instance.email,
  'telefon': instance.telefon,
};
