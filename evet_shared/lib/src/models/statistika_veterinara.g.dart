// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'statistika_veterinara.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

StatistikaVeterinara _$StatistikaVeterinaraFromJson(
  Map<String, dynamic> json,
) => StatistikaVeterinara(
  korisnikId: (json['korisnikId'] as num).toInt(),
  ime: json['ime'] as String,
  prezime: json['prezime'] as String,
  imaSliku: json['imaSliku'] as bool? ?? false,
  specijalnostNaziv: json['specijalnostNaziv'] as String?,
  jeAktivan: json['jeAktivan'] as bool? ?? false,
  brojTermina: (json['brojTermina'] as num).toInt(),
  brojZavrsenihTermina: (json['brojZavrsenihTermina'] as num).toInt(),
  pozitivneRecenzije: (json['pozitivneRecenzije'] as num).toInt(),
  negativneRecenzije: (json['negativneRecenzije'] as num).toInt(),
);

Map<String, dynamic> _$StatistikaVeterinaraToJson(
  StatistikaVeterinara instance,
) => <String, dynamic>{
  'korisnikId': instance.korisnikId,
  'ime': instance.ime,
  'prezime': instance.prezime,
  'imaSliku': instance.imaSliku,
  'specijalnostNaziv': instance.specijalnostNaziv,
  'jeAktivan': instance.jeAktivan,
  'brojTermina': instance.brojTermina,
  'brojZavrsenihTermina': instance.brojZavrsenihTermina,
  'pozitivneRecenzije': instance.pozitivneRecenzije,
  'negativneRecenzije': instance.negativneRecenzije,
};
