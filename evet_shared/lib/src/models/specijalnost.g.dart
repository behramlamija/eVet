// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'specijalnost.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Specijalnost _$SpecijalnostFromJson(Map<String, dynamic> json) => Specijalnost(
  specijalnostId: (json['specijalnostId'] as num).toInt(),
  naziv: json['naziv'] as String,
);

Map<String, dynamic> _$SpecijalnostToJson(Specijalnost instance) =>
    <String, dynamic>{
      'specijalnostId': instance.specijalnostId,
      'naziv': instance.naziv,
    };
