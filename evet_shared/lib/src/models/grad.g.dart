// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'grad.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Grad _$GradFromJson(Map<String, dynamic> json) => Grad(
  gradId: (json['gradId'] as num).toInt(),
  naziv: json['naziv'] as String,
  postanskiBroj: json['postanskiBroj'] as String,
);

Map<String, dynamic> _$GradToJson(Grad instance) => <String, dynamic>{
  'gradId': instance.gradId,
  'naziv': instance.naziv,
  'postanskiBroj': instance.postanskiBroj,
};
