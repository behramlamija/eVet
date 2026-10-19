// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'korisnik_osnovno.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

KorisnikOsnovno _$KorisnikOsnovnoFromJson(Map<String, dynamic> json) =>
    KorisnikOsnovno(
      korisnikId: (json['korisnikId'] as num).toInt(),
      ime: json['ime'] as String,
      prezime: json['prezime'] as String,
      email: json['email'] as String,
      telefon: json['telefon'] as String?,
      zvanje: json['zvanje'] as String?,
      specijalnostNaziv: json['specijalnostNaziv'] as String?,
      imaSliku: json['imaSliku'] as bool? ?? false,
    );

Map<String, dynamic> _$KorisnikOsnovnoToJson(KorisnikOsnovno instance) =>
    <String, dynamic>{
      'korisnikId': instance.korisnikId,
      'ime': instance.ime,
      'prezime': instance.prezime,
      'email': instance.email,
      'telefon': instance.telefon,
      'zvanje': instance.zvanje,
      'specijalnostNaziv': instance.specijalnostNaziv,
      'imaSliku': instance.imaSliku,
    };
