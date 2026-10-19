// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'vrsta_usluge.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

VrstaUsluge _$VrstaUslugeFromJson(Map<String, dynamic> json) => VrstaUsluge(
  vrstaId: (json['vrstaId'] as num).toInt(),
  naziv: json['naziv'] as String,
  jeOperacija: json['jeOperacija'] as bool? ?? false,
  jeVakcinacija: json['jeVakcinacija'] as bool? ?? false,
  imaSliku: json['imaSliku'] as bool? ?? false,
);

Map<String, dynamic> _$VrstaUslugeToJson(VrstaUsluge instance) =>
    <String, dynamic>{
      'vrstaId': instance.vrstaId,
      'naziv': instance.naziv,
      'jeOperacija': instance.jeOperacija,
      'jeVakcinacija': instance.jeVakcinacija,
      'imaSliku': instance.imaSliku,
    };
