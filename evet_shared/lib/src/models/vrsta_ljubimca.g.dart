// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'vrsta_ljubimca.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

VrstaLjubimca _$VrstaLjubimcaFromJson(Map<String, dynamic> json) =>
    VrstaLjubimca(
      vrstaLjubimcaId: (json['vrstaLjubimcaId'] as num).toInt(),
      naziv: json['naziv'] as String,
    );

Map<String, dynamic> _$VrstaLjubimcaToJson(VrstaLjubimca instance) =>
    <String, dynamic>{
      'vrstaLjubimcaId': instance.vrstaLjubimcaId,
      'naziv': instance.naziv,
    };
