// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'statistika_period.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

StatistikaPeriod _$StatistikaPeriodFromJson(Map<String, dynamic> json) =>
    StatistikaPeriod(
      godina: (json['godina'] as num).toInt(),
      mjesec: (json['mjesec'] as num?)?.toInt(),
      brojTermina: (json['brojTermina'] as num).toInt(),
      brojOperacija: (json['brojOperacija'] as num).toInt(),
      prihod: (json['prihod'] as num).toDouble(),
    );

Map<String, dynamic> _$StatistikaPeriodToJson(StatistikaPeriod instance) =>
    <String, dynamic>{
      'godina': instance.godina,
      'mjesec': instance.mjesec,
      'brojTermina': instance.brojTermina,
      'brojOperacija': instance.brojOperacija,
      'prihod': instance.prihod,
    };
