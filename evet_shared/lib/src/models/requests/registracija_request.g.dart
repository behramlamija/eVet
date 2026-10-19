// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'registracija_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

RegistracijaRequest _$RegistracijaRequestFromJson(Map<String, dynamic> json) =>
    RegistracijaRequest(
      ime: json['ime'] as String,
      prezime: json['prezime'] as String,
      korisnickoIme: json['korisnickoIme'] as String,
      email: json['email'] as String,
      telefon: json['telefon'] as String?,
      lozinka: json['lozinka'] as String,
      lozinkaPotvrda: json['lozinkaPotvrda'] as String,
    );

Map<String, dynamic> _$RegistracijaRequestToJson(
  RegistracijaRequest instance,
) => <String, dynamic>{
  'ime': instance.ime,
  'prezime': instance.prezime,
  'korisnickoIme': instance.korisnickoIme,
  'email': instance.email,
  'telefon': instance.telefon,
  'lozinka': instance.lozinka,
  'lozinkaPotvrda': instance.lozinkaPotvrda,
};
