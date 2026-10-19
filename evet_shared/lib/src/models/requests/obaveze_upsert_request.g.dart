// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'obaveze_upsert_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ObavezeUpsertRequest _$ObavezeUpsertRequestFromJson(
  Map<String, dynamic> json,
) => ObavezeUpsertRequest(
  naslov: json['naslov'] as String,
  opis: json['opis'] as String?,
  datumVrijeme: json['datumVrijeme'] == null
      ? null
      : DateTime.parse(json['datumVrijeme'] as String),
  jeZavrsena: json['jeZavrsena'] as bool? ?? false,
);

Map<String, dynamic> _$ObavezeUpsertRequestToJson(
  ObavezeUpsertRequest instance,
) => <String, dynamic>{
  'naslov': instance.naslov,
  'opis': instance.opis,
  'datumVrijeme': instance.datumVrijeme?.toIso8601String(),
  'jeZavrsena': instance.jeZavrsena,
};
