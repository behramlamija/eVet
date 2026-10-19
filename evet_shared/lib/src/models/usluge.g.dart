// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'usluge.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Usluge _$UslugeFromJson(Map<String, dynamic> json) => Usluge(
  uslugaId: (json['uslugaId'] as num).toInt(),
  naziv: json['naziv'] as String,
  opis: json['opis'] as String?,
  cijena: (json['cijena'] as num).toDouble(),
  trajanje: (json['trajanje'] as num).toInt(),
  vrstaId: (json['vrstaId'] as num).toInt(),
  vrstaNaziv: json['vrstaNaziv'] as String,
  datumObjavljivanja: DateTime.parse(json['datumObjavljivanja'] as String),
  imaSliku: json['imaSliku'] as bool? ?? false,
);

Map<String, dynamic> _$UslugeToJson(Usluge instance) => <String, dynamic>{
  'uslugaId': instance.uslugaId,
  'naziv': instance.naziv,
  'opis': instance.opis,
  'cijena': instance.cijena,
  'trajanje': instance.trajanje,
  'vrstaId': instance.vrstaId,
  'vrstaNaziv': instance.vrstaNaziv,
  'datumObjavljivanja': instance.datumObjavljivanja.toIso8601String(),
  'imaSliku': instance.imaSliku,
};
