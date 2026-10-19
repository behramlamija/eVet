// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'promocije.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Promocije _$PromocijeFromJson(Map<String, dynamic> json) => Promocije(
  promocijaId: (json['promocijaId'] as num).toInt(),
  naziv: json['naziv'] as String,
  opis: json['opis'] as String?,
  kod: json['kod'] as String,
  popust: (json['popust'] as num).toDouble(),
  datumPocetka: DateTime.parse(json['datumPocetka'] as String),
  datumKraja: DateTime.parse(json['datumKraja'] as String),
  uslugaId: (json['uslugaId'] as num).toInt(),
  uslugaNaziv: json['uslugaNaziv'] as String,
  uslugaCijena: (json['uslugaCijena'] as num).toDouble(),
  jeAktivna: json['jeAktivna'] as bool? ?? false,
);

Map<String, dynamic> _$PromocijeToJson(Promocije instance) => <String, dynamic>{
  'promocijaId': instance.promocijaId,
  'naziv': instance.naziv,
  'opis': instance.opis,
  'kod': instance.kod,
  'popust': instance.popust,
  'datumPocetka': instance.datumPocetka.toIso8601String(),
  'datumKraja': instance.datumKraja.toIso8601String(),
  'uslugaId': instance.uslugaId,
  'uslugaNaziv': instance.uslugaNaziv,
  'uslugaCijena': instance.uslugaCijena,
  'jeAktivna': instance.jeAktivna,
};
