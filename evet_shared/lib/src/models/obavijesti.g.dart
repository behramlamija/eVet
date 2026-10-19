// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'obavijesti.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Obavijesti _$ObavijestiFromJson(Map<String, dynamic> json) => Obavijesti(
  obavijestId: (json['obavijestId'] as num).toInt(),
  naslov: json['naslov'] as String,
  sadrzaj: json['sadrzaj'] as String,
  jePogledana: json['jePogledana'] as bool? ?? false,
  datumObavijesti: DateTime.parse(json['datumObavijesti'] as String),
);

Map<String, dynamic> _$ObavijestiToJson(Obavijesti instance) =>
    <String, dynamic>{
      'obavijestId': instance.obavijestId,
      'naslov': instance.naslov,
      'sadrzaj': instance.sadrzaj,
      'jePogledana': instance.jePogledana,
      'datumObavijesti': instance.datumObavijesti.toIso8601String(),
    };
