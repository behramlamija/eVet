// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'terapije_upsert_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

TerapijeUpsertRequest _$TerapijeUpsertRequestFromJson(
  Map<String, dynamic> json,
) => TerapijeUpsertRequest(
  ljubimacId: (json['ljubimacId'] as num).toInt(),
  terminId: (json['terminId'] as num?)?.toInt(),
  dijagnoza: json['dijagnoza'] as String,
  lijek: json['lijek'] as String?,
  doza: json['doza'] as String?,
  napomena: json['napomena'] as String?,
  trajanjeDana: (json['trajanjeDana'] as num?)?.toInt(),
  datumKontrolnog: const NullableDateOnlyConverter().fromJson(
    json['datumKontrolnog'] as String?,
  ),
);

Map<String, dynamic> _$TerapijeUpsertRequestToJson(
  TerapijeUpsertRequest instance,
) => <String, dynamic>{
  'ljubimacId': instance.ljubimacId,
  'terminId': instance.terminId,
  'dijagnoza': instance.dijagnoza,
  'lijek': instance.lijek,
  'doza': instance.doza,
  'napomena': instance.napomena,
  'trajanjeDana': instance.trajanjeDana,
  'datumKontrolnog': const NullableDateOnlyConverter().toJson(
    instance.datumKontrolnog,
  ),
};
