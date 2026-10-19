// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'rasa.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Rasa _$RasaFromJson(Map<String, dynamic> json) => Rasa(
  rasaId: (json['rasaId'] as num).toInt(),
  naziv: json['naziv'] as String,
  vrstaLjubimcaId: (json['vrstaLjubimcaId'] as num).toInt(),
  vrstaLjubimcaNaziv: json['vrstaLjubimcaNaziv'] as String,
);

Map<String, dynamic> _$RasaToJson(Rasa instance) => <String, dynamic>{
  'rasaId': instance.rasaId,
  'naziv': instance.naziv,
  'vrstaLjubimcaId': instance.vrstaLjubimcaId,
  'vrstaLjubimcaNaziv': instance.vrstaLjubimcaNaziv,
};
