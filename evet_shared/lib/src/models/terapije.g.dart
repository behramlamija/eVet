// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'terapije.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Terapije _$TerapijeFromJson(Map<String, dynamic> json) => Terapije(
  terapijaId: (json['terapijaId'] as num).toInt(),
  ljubimacId: (json['ljubimacId'] as num).toInt(),
  ljubimacIme: json['ljubimacIme'] as String,
  veterinarId: (json['veterinarId'] as num).toInt(),
  veterinarIme: json['veterinarIme'] as String,
  veterinarPrezime: json['veterinarPrezime'] as String,
  terminId: (json['terminId'] as num?)?.toInt(),
  terminSifra: json['terminSifra'] as String?,
  dijagnoza: json['dijagnoza'] as String,
  lijek: json['lijek'] as String?,
  doza: json['doza'] as String?,
  napomena: json['napomena'] as String?,
  datumTerapije: DateTime.parse(json['datumTerapije'] as String),
  trajanjeDana: (json['trajanjeDana'] as num?)?.toInt(),
  jeZavrsena: json['jeZavrsena'] as bool? ?? false,
  datumKontrolnog: const NullableDateOnlyConverter().fromJson(
    json['datumKontrolnog'] as String?,
  ),
);

Map<String, dynamic> _$TerapijeToJson(Terapije instance) => <String, dynamic>{
  'terapijaId': instance.terapijaId,
  'ljubimacId': instance.ljubimacId,
  'ljubimacIme': instance.ljubimacIme,
  'veterinarId': instance.veterinarId,
  'veterinarIme': instance.veterinarIme,
  'veterinarPrezime': instance.veterinarPrezime,
  'terminId': instance.terminId,
  'terminSifra': instance.terminSifra,
  'dijagnoza': instance.dijagnoza,
  'lijek': instance.lijek,
  'doza': instance.doza,
  'napomena': instance.napomena,
  'datumTerapije': instance.datumTerapije.toIso8601String(),
  'trajanjeDana': instance.trajanjeDana,
  'jeZavrsena': instance.jeZavrsena,
  'datumKontrolnog': const NullableDateOnlyConverter().toJson(
    instance.datumKontrolnog,
  ),
};
