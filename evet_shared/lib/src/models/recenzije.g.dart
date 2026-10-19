// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'recenzije.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Recenzije _$RecenzijeFromJson(Map<String, dynamic> json) => Recenzije(
  recenzijaId: (json['recenzijaId'] as num).toInt(),
  terminId: (json['terminId'] as num).toInt(),
  terminSifra: json['terminSifra'] as String,
  korisnikId: (json['korisnikId'] as num).toInt(),
  korisnikIme: json['korisnikIme'] as String,
  korisnikPrezime: json['korisnikPrezime'] as String,
  veterinarId: (json['veterinarId'] as num).toInt(),
  veterinarIme: json['veterinarIme'] as String,
  veterinarPrezime: json['veterinarPrezime'] as String,
  uslugaPozitivna: json['uslugaPozitivna'] as bool? ?? false,
  veterinarPozitivan: json['veterinarPozitivan'] as bool? ?? false,
  komentar: json['komentar'] as String?,
  datumDodavanja: DateTime.parse(json['datumDodavanja'] as String),
  odgovori:
      (json['odgovori'] as List<dynamic>?)
          ?.map((e) => RecenzijaOdgovori.fromJson(e as Map<String, dynamic>))
          .toList() ??
      const [],
);

Map<String, dynamic> _$RecenzijeToJson(Recenzije instance) => <String, dynamic>{
  'recenzijaId': instance.recenzijaId,
  'terminId': instance.terminId,
  'terminSifra': instance.terminSifra,
  'korisnikId': instance.korisnikId,
  'korisnikIme': instance.korisnikIme,
  'korisnikPrezime': instance.korisnikPrezime,
  'veterinarId': instance.veterinarId,
  'veterinarIme': instance.veterinarIme,
  'veterinarPrezime': instance.veterinarPrezime,
  'uslugaPozitivna': instance.uslugaPozitivna,
  'veterinarPozitivan': instance.veterinarPozitivan,
  'komentar': instance.komentar,
  'datumDodavanja': instance.datumDodavanja.toIso8601String(),
  'odgovori': instance.odgovori.map((e) => e.toJson()).toList(),
};
