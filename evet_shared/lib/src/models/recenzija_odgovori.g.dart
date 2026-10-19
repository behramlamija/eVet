// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'recenzija_odgovori.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

RecenzijaOdgovori _$RecenzijaOdgovoriFromJson(Map<String, dynamic> json) =>
    RecenzijaOdgovori(
      recenzijaOdgovorId: (json['recenzijaOdgovorId'] as num).toInt(),
      korisnikIme: json['korisnikIme'] as String,
      korisnikPrezime: json['korisnikPrezime'] as String,
      komentar: json['komentar'] as String,
      datumDodavanja: DateTime.parse(json['datumDodavanja'] as String),
    );

Map<String, dynamic> _$RecenzijaOdgovoriToJson(RecenzijaOdgovori instance) =>
    <String, dynamic>{
      'recenzijaOdgovorId': instance.recenzijaOdgovorId,
      'korisnikIme': instance.korisnikIme,
      'korisnikPrezime': instance.korisnikPrezime,
      'komentar': instance.komentar,
      'datumDodavanja': instance.datumDodavanja.toIso8601String(),
    };
