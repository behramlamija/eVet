// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'ljubimci.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Ljubimci _$LjubimciFromJson(Map<String, dynamic> json) => Ljubimci(
  ljubimacId: (json['ljubimacId'] as num).toInt(),
  korisnikId: (json['korisnikId'] as num).toInt(),
  korisnikIme: json['korisnikIme'] as String,
  korisnikPrezime: json['korisnikPrezime'] as String,
  ime: json['ime'] as String,
  rasaId: (json['rasaId'] as num).toInt(),
  rasaNaziv: json['rasaNaziv'] as String,
  vrstaLjubimcaId: (json['vrstaLjubimcaId'] as num).toInt(),
  vrstaLjubimcaNaziv: json['vrstaLjubimcaNaziv'] as String,
  datumRodjenja: const NullableDateOnlyConverter().fromJson(
    json['datumRodjenja'] as String?,
  ),
  spol: $enumDecode(_$SpolEnumMap, json['spol']),
  tezina: (json['tezina'] as num?)?.toDouble(),
  napomena: json['napomena'] as String?,
  imaSliku: json['imaSliku'] as bool? ?? false,
);

Map<String, dynamic> _$LjubimciToJson(Ljubimci instance) => <String, dynamic>{
  'ljubimacId': instance.ljubimacId,
  'korisnikId': instance.korisnikId,
  'korisnikIme': instance.korisnikIme,
  'korisnikPrezime': instance.korisnikPrezime,
  'ime': instance.ime,
  'rasaId': instance.rasaId,
  'rasaNaziv': instance.rasaNaziv,
  'vrstaLjubimcaId': instance.vrstaLjubimcaId,
  'vrstaLjubimcaNaziv': instance.vrstaLjubimcaNaziv,
  'datumRodjenja': const NullableDateOnlyConverter().toJson(
    instance.datumRodjenja,
  ),
  'spol': _$SpolEnumMap[instance.spol]!,
  'tezina': instance.tezina,
  'napomena': instance.napomena,
  'imaSliku': instance.imaSliku,
};

const _$SpolEnumMap = {Spol.muski: 'Muski', Spol.zenski: 'Zenski'};
