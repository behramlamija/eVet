// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'aktivirane_promocije.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

AktiviranePromocije _$AktiviranePromocijeFromJson(Map<String, dynamic> json) =>
    AktiviranePromocije(
      aktiviranaPromocijaId: (json['aktiviranaPromocijaId'] as num).toInt(),
      promocijaId: (json['promocijaId'] as num).toInt(),
      promocijaNaziv: json['promocijaNaziv'] as String,
      promocijaKod: json['promocijaKod'] as String,
      promocijaPopust: (json['promocijaPopust'] as num).toDouble(),
      promocijaDatumKraja: DateTime.parse(
        json['promocijaDatumKraja'] as String,
      ),
      uslugaNaziv: json['uslugaNaziv'] as String,
      iskoristena: json['iskoristena'] as bool? ?? false,
      datumAktiviranja: DateTime.parse(json['datumAktiviranja'] as String),
    );

Map<String, dynamic> _$AktiviranePromocijeToJson(
  AktiviranePromocije instance,
) => <String, dynamic>{
  'aktiviranaPromocijaId': instance.aktiviranaPromocijaId,
  'promocijaId': instance.promocijaId,
  'promocijaNaziv': instance.promocijaNaziv,
  'promocijaKod': instance.promocijaKod,
  'promocijaPopust': instance.promocijaPopust,
  'promocijaDatumKraja': instance.promocijaDatumKraja.toIso8601String(),
  'uslugaNaziv': instance.uslugaNaziv,
  'iskoristena': instance.iskoristena,
  'datumAktiviranja': instance.datumAktiviranja.toIso8601String(),
};
