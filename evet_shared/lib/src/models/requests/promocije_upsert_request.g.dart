// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'promocije_upsert_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

PromocijeUpsertRequest _$PromocijeUpsertRequestFromJson(
  Map<String, dynamic> json,
) => PromocijeUpsertRequest(
  naziv: json['naziv'] as String,
  opis: json['opis'] as String?,
  kod: json['kod'] as String,
  popust: (json['popust'] as num).toDouble(),
  datumPocetka: json['datumPocetka'] == null
      ? null
      : DateTime.parse(json['datumPocetka'] as String),
  datumKraja: json['datumKraja'] == null
      ? null
      : DateTime.parse(json['datumKraja'] as String),
  uslugaId: (json['uslugaId'] as num).toInt(),
  jeAktivna: json['jeAktivna'] as bool? ?? false,
);

Map<String, dynamic> _$PromocijeUpsertRequestToJson(
  PromocijeUpsertRequest instance,
) => <String, dynamic>{
  'naziv': instance.naziv,
  'opis': instance.opis,
  'kod': instance.kod,
  'popust': instance.popust,
  'datumPocetka': instance.datumPocetka?.toIso8601String(),
  'datumKraja': instance.datumKraja?.toIso8601String(),
  'uslugaId': instance.uslugaId,
  'jeAktivna': instance.jeAktivna,
};
