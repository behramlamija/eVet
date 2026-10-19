// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'stavke_termina.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

StavkeTermina _$StavkeTerminaFromJson(Map<String, dynamic> json) =>
    StavkeTermina(
      stavkaTerminaId: (json['stavkaTerminaId'] as num).toInt(),
      uslugaId: (json['uslugaId'] as num).toInt(),
      uslugaNaziv: json['uslugaNaziv'] as String,
      cijena: (json['cijena'] as num).toDouble(),
    );

Map<String, dynamic> _$StavkeTerminaToJson(StavkeTermina instance) =>
    <String, dynamic>{
      'stavkaTerminaId': instance.stavkaTerminaId,
      'uslugaId': instance.uslugaId,
      'uslugaNaziv': instance.uslugaNaziv,
      'cijena': instance.cijena,
    };
