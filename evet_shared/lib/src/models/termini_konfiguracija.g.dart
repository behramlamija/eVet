// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'termini_konfiguracija.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

TerminiKonfiguracija _$TerminiKonfiguracijaFromJson(
  Map<String, dynamic> json,
) => TerminiKonfiguracija(
  cijenaPrevoza: (json['cijenaPrevoza'] as num).toDouble(),
  minimalnoSatiZaOtkazivanje: (json['minimalnoSatiZaOtkazivanje'] as num)
      .toInt(),
  maksimalnoDanaUnaprijed: (json['maksimalnoDanaUnaprijed'] as num).toInt(),
  radnoVrijemeOdSat: (json['radnoVrijemeOdSat'] as num).toInt(),
  radnoVrijemeDoSat: (json['radnoVrijemeDoSat'] as num).toInt(),
);

Map<String, dynamic> _$TerminiKonfiguracijaToJson(
  TerminiKonfiguracija instance,
) => <String, dynamic>{
  'cijenaPrevoza': instance.cijenaPrevoza,
  'minimalnoSatiZaOtkazivanje': instance.minimalnoSatiZaOtkazivanje,
  'maksimalnoDanaUnaprijed': instance.maksimalnoDanaUnaprijed,
  'radnoVrijemeOdSat': instance.radnoVrijemeOdSat,
  'radnoVrijemeDoSat': instance.radnoVrijemeDoSat,
};
