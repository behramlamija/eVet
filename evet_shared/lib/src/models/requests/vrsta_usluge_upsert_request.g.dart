// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'vrsta_usluge_upsert_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

VrstaUslugeUpsertRequest _$VrstaUslugeUpsertRequestFromJson(
  Map<String, dynamic> json,
) => VrstaUslugeUpsertRequest(
  naziv: json['naziv'] as String,
  jeOperacija: json['jeOperacija'] as bool? ?? false,
  jeVakcinacija: json['jeVakcinacija'] as bool? ?? false,
);

Map<String, dynamic> _$VrstaUslugeUpsertRequestToJson(
  VrstaUslugeUpsertRequest instance,
) => <String, dynamic>{
  'naziv': instance.naziv,
  'jeOperacija': instance.jeOperacija,
  'jeVakcinacija': instance.jeVakcinacija,
};
