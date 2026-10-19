// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'grad_upsert_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

GradUpsertRequest _$GradUpsertRequestFromJson(Map<String, dynamic> json) =>
    GradUpsertRequest(
      naziv: json['naziv'] as String,
      postanskiBroj: json['postanskiBroj'] as String,
    );

Map<String, dynamic> _$GradUpsertRequestToJson(GradUpsertRequest instance) =>
    <String, dynamic>{
      'naziv': instance.naziv,
      'postanskiBroj': instance.postanskiBroj,
    };
