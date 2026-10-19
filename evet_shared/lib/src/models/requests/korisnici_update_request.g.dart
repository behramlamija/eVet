// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'korisnici_update_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

KorisniciUpdateRequest _$KorisniciUpdateRequestFromJson(
  Map<String, dynamic> json,
) => KorisniciUpdateRequest(
  ime: json['ime'] as String,
  prezime: json['prezime'] as String,
  email: json['email'] as String,
  telefon: json['telefon'] as String?,
  jeAktivan: json['jeAktivan'] as bool? ?? false,
  ulogeIds:
      (json['ulogeIds'] as List<dynamic>?)
          ?.map((e) => (e as num).toInt())
          .toList() ??
      const [],
  lozinka: json['lozinka'] as String?,
  lozinkaPotvrda: json['lozinkaPotvrda'] as String?,
  datumRodjenja: const NullableDateOnlyConverter().fromJson(
    json['datumRodjenja'] as String?,
  ),
  gradRodjenjaId: (json['gradRodjenjaId'] as num?)?.toInt(),
  datumZaposlenja: const NullableDateOnlyConverter().fromJson(
    json['datumZaposlenja'] as String?,
  ),
  zvanje: json['zvanje'] as String?,
  specijalnostId: (json['specijalnostId'] as num?)?.toInt(),
);

Map<String, dynamic> _$KorisniciUpdateRequestToJson(
  KorisniciUpdateRequest instance,
) => <String, dynamic>{
  'ime': instance.ime,
  'prezime': instance.prezime,
  'email': instance.email,
  'telefon': instance.telefon,
  'jeAktivan': instance.jeAktivan,
  'ulogeIds': instance.ulogeIds,
  'lozinka': instance.lozinka,
  'lozinkaPotvrda': instance.lozinkaPotvrda,
  'datumRodjenja': const NullableDateOnlyConverter().toJson(
    instance.datumRodjenja,
  ),
  'gradRodjenjaId': instance.gradRodjenjaId,
  'datumZaposlenja': const NullableDateOnlyConverter().toJson(
    instance.datumZaposlenja,
  ),
  'zvanje': instance.zvanje,
  'specijalnostId': instance.specijalnostId,
};
