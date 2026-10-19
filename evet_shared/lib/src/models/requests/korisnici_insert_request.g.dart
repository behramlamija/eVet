// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'korisnici_insert_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

KorisniciInsertRequest _$KorisniciInsertRequestFromJson(
  Map<String, dynamic> json,
) => KorisniciInsertRequest(
  ime: json['ime'] as String,
  prezime: json['prezime'] as String,
  korisnickoIme: json['korisnickoIme'] as String,
  email: json['email'] as String,
  telefon: json['telefon'] as String?,
  lozinka: json['lozinka'] as String,
  lozinkaPotvrda: json['lozinkaPotvrda'] as String,
  jeAktivan: json['jeAktivan'] as bool? ?? false,
  ulogeIds:
      (json['ulogeIds'] as List<dynamic>?)
          ?.map((e) => (e as num).toInt())
          .toList() ??
      const [],
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

Map<String, dynamic> _$KorisniciInsertRequestToJson(
  KorisniciInsertRequest instance,
) => <String, dynamic>{
  'ime': instance.ime,
  'prezime': instance.prezime,
  'korisnickoIme': instance.korisnickoIme,
  'email': instance.email,
  'telefon': instance.telefon,
  'lozinka': instance.lozinka,
  'lozinkaPotvrda': instance.lozinkaPotvrda,
  'jeAktivan': instance.jeAktivan,
  'ulogeIds': instance.ulogeIds,
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
