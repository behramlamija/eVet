// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'preporuke_usluga.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

PreporukeUsluga _$PreporukeUslugaFromJson(Map<String, dynamic> json) =>
    PreporukeUsluga(
      usluga: Usluge.fromJson(json['usluga'] as Map<String, dynamic>),
      uslugaIzvorId: (json['uslugaIzvorId'] as num).toInt(),
      uslugaIzvorNaziv: json['uslugaIzvorNaziv'] as String,
      brojZajednickihTermina: (json['brojZajednickihTermina'] as num).toInt(),
      brojTerminaIzvora: (json['brojTerminaIzvora'] as num).toInt(),
      pouzdanost: (json['pouzdanost'] as num).toDouble(),
      objasnjenje: json['objasnjenje'] as String,
    );

Map<String, dynamic> _$PreporukeUslugaToJson(PreporukeUsluga instance) =>
    <String, dynamic>{
      'usluga': instance.usluga.toJson(),
      'uslugaIzvorId': instance.uslugaIzvorId,
      'uslugaIzvorNaziv': instance.uslugaIzvorNaziv,
      'brojZajednickihTermina': instance.brojZajednickihTermina,
      'brojTerminaIzvora': instance.brojTerminaIzvora,
      'pouzdanost': instance.pouzdanost,
      'objasnjenje': instance.objasnjenje,
    };
