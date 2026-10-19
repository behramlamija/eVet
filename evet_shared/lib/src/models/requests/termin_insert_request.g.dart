// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'termin_insert_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

TerminInsertRequest _$TerminInsertRequestFromJson(Map<String, dynamic> json) =>
    TerminInsertRequest(
      veterinarId: (json['veterinarId'] as num).toInt(),
      ljubimacId: (json['ljubimacId'] as num).toInt(),
      datumTermina: const NullableDateOnlyConverter().fromJson(
        json['datumTermina'] as String?,
      ),
      vrijemePocetka: json['vrijemePocetka'] as String?,
      uslugeIds:
          (json['uslugeIds'] as List<dynamic>?)
              ?.map((e) => (e as num).toInt())
              .toList() ??
          const [],
      nacinPlacanjaId: (json['nacinPlacanjaId'] as num).toInt(),
      jePrevoz: json['jePrevoz'] as bool? ?? false,
      adresaPrevoza: json['adresaPrevoza'] as String?,
      gradPrevozaId: (json['gradPrevozaId'] as num?)?.toInt(),
      kodPromocije: json['kodPromocije'] as String?,
    );

Map<String, dynamic> _$TerminInsertRequestToJson(
  TerminInsertRequest instance,
) => <String, dynamic>{
  'veterinarId': instance.veterinarId,
  'ljubimacId': instance.ljubimacId,
  'datumTermina': const NullableDateOnlyConverter().toJson(
    instance.datumTermina,
  ),
  'vrijemePocetka': instance.vrijemePocetka,
  'uslugeIds': instance.uslugeIds,
  'nacinPlacanjaId': instance.nacinPlacanjaId,
  'jePrevoz': instance.jePrevoz,
  'adresaPrevoza': instance.adresaPrevoza,
  'gradPrevozaId': instance.gradPrevozaId,
  'kodPromocije': instance.kodPromocije,
};
