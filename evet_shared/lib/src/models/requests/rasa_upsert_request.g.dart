// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'rasa_upsert_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

RasaUpsertRequest _$RasaUpsertRequestFromJson(Map<String, dynamic> json) =>
    RasaUpsertRequest(
      naziv: json['naziv'] as String,
      vrstaLjubimcaId: (json['vrstaLjubimcaId'] as num).toInt(),
    );

Map<String, dynamic> _$RasaUpsertRequestToJson(RasaUpsertRequest instance) =>
    <String, dynamic>{
      'naziv': instance.naziv,
      'vrstaLjubimcaId': instance.vrstaLjubimcaId,
    };
