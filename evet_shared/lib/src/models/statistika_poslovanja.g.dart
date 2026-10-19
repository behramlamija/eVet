// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'statistika_poslovanja.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

StatistikaPoslovanja _$StatistikaPoslovanjaFromJson(
  Map<String, dynamic> json,
) => StatistikaPoslovanja(
  brojTermina: (json['brojTermina'] as num).toInt(),
  brojZavrsenihTermina: (json['brojZavrsenihTermina'] as num).toInt(),
  brojOtkazanihTermina: (json['brojOtkazanihTermina'] as num).toInt(),
  brojPruzenihUsluga: (json['brojPruzenihUsluga'] as num).toInt(),
  ukupniPrihod: (json['ukupniPrihod'] as num).toDouble(),
  brojOperacija: (json['brojOperacija'] as num).toInt(),
  brojNovihKlijenata: (json['brojNovihKlijenata'] as num).toInt(),
  najcescaUsluga: json['najcescaUsluga'] as String?,
  najzastupljenijaRasa: json['najzastupljenijaRasa'] as String?,
  topVeterinar: json['topVeterinar'] as String?,
);

Map<String, dynamic> _$StatistikaPoslovanjaToJson(
  StatistikaPoslovanja instance,
) => <String, dynamic>{
  'brojTermina': instance.brojTermina,
  'brojZavrsenihTermina': instance.brojZavrsenihTermina,
  'brojOtkazanihTermina': instance.brojOtkazanihTermina,
  'brojPruzenihUsluga': instance.brojPruzenihUsluga,
  'ukupniPrihod': instance.ukupniPrihod,
  'brojOperacija': instance.brojOperacija,
  'brojNovihKlijenata': instance.brojNovihKlijenata,
  'najcescaUsluga': instance.najcescaUsluga,
  'najzastupljenijaRasa': instance.najzastupljenijaRasa,
  'topVeterinar': instance.topVeterinar,
};
