// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'ljubimci_upsert_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

LjubimciUpsertRequest _$LjubimciUpsertRequestFromJson(
  Map<String, dynamic> json,
) => LjubimciUpsertRequest(
  ime: json['ime'] as String,
  rasaId: (json['rasaId'] as num).toInt(),
  datumRodjenja: const NullableDateOnlyConverter().fromJson(
    json['datumRodjenja'] as String?,
  ),
  spol: $enumDecode(_$SpolEnumMap, json['spol']),
  tezina: (json['tezina'] as num?)?.toDouble(),
  napomena: json['napomena'] as String?,
  vlasnikId: (json['vlasnikId'] as num?)?.toInt(),
);

Map<String, dynamic> _$LjubimciUpsertRequestToJson(
  LjubimciUpsertRequest instance,
) => <String, dynamic>{
  'ime': instance.ime,
  'rasaId': instance.rasaId,
  'datumRodjenja': const NullableDateOnlyConverter().toJson(
    instance.datumRodjenja,
  ),
  'spol': _$SpolEnumMap[instance.spol]!,
  'tezina': instance.tezina,
  'napomena': instance.napomena,
  'vlasnikId': instance.vlasnikId,
};

const _$SpolEnumMap = {Spol.muski: 'Muski', Spol.zenski: 'Zenski'};
