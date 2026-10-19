// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'obaveze.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Obaveze _$ObavezeFromJson(Map<String, dynamic> json) => Obaveze(
  obavezaId: (json['obavezaId'] as num).toInt(),
  naslov: json['naslov'] as String,
  opis: json['opis'] as String?,
  datumVrijeme: DateTime.parse(json['datumVrijeme'] as String),
  jeZavrsena: json['jeZavrsena'] as bool? ?? false,
);

Map<String, dynamic> _$ObavezeToJson(Obaveze instance) => <String, dynamic>{
  'obavezaId': instance.obavezaId,
  'naslov': instance.naslov,
  'opis': instance.opis,
  'datumVrijeme': instance.datumVrijeme.toIso8601String(),
  'jeZavrsena': instance.jeZavrsena,
};
