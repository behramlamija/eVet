// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'usluge_upsert_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UslugeUpsertRequest _$UslugeUpsertRequestFromJson(Map<String, dynamic> json) =>
    UslugeUpsertRequest(
      naziv: json['naziv'] as String,
      opis: json['opis'] as String?,
      cijena: (json['cijena'] as num).toDouble(),
      trajanje: (json['trajanje'] as num).toInt(),
      vrstaId: (json['vrstaId'] as num).toInt(),
    );

Map<String, dynamic> _$UslugeUpsertRequestToJson(
  UslugeUpsertRequest instance,
) => <String, dynamic>{
  'naziv': instance.naziv,
  'opis': instance.opis,
  'cijena': instance.cijena,
  'trajanje': instance.trajanje,
  'vrstaId': instance.vrstaId,
};
