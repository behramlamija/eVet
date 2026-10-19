// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'termini.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Termini _$TerminiFromJson(Map<String, dynamic> json) => Termini(
  terminId: (json['terminId'] as num).toInt(),
  sifra: json['sifra'] as String,
  korisnikId: (json['korisnikId'] as num).toInt(),
  korisnikIme: json['korisnikIme'] as String,
  korisnikPrezime: json['korisnikPrezime'] as String,
  veterinarId: (json['veterinarId'] as num).toInt(),
  veterinarIme: json['veterinarIme'] as String,
  veterinarPrezime: json['veterinarPrezime'] as String,
  ljubimacId: (json['ljubimacId'] as num).toInt(),
  ljubimacIme: json['ljubimacIme'] as String,
  datumTermina: const DateOnlyConverter().fromJson(
    json['datumTermina'] as String,
  ),
  vrijemePocetka: json['vrijemePocetka'] as String,
  vrijemeKraja: json['vrijemeKraja'] as String,
  ukupnoTrajanje: (json['ukupnoTrajanje'] as num).toInt(),
  iznosPopusta: (json['iznosPopusta'] as num).toDouble(),
  cijenaPrevoza: (json['cijenaPrevoza'] as num).toDouble(),
  ukupnaCijena: (json['ukupnaCijena'] as num).toDouble(),
  status: $enumDecode(_$StatusTerminaEnumMap, json['status']),
  nacinPlacanjaId: (json['nacinPlacanjaId'] as num).toInt(),
  nacinPlacanjaNaziv: json['nacinPlacanjaNaziv'] as String,
  jePlacen: json['jePlacen'] as bool? ?? false,
  jeRefundiran: json['jeRefundiran'] as bool? ?? false,
  jePrevoz: json['jePrevoz'] as bool? ?? false,
  adresaPrevoza: json['adresaPrevoza'] as String?,
  gradPrevozaId: (json['gradPrevozaId'] as num?)?.toInt(),
  gradPrevozaNaziv: json['gradPrevozaNaziv'] as String?,
  datumKreiranja: DateTime.parse(json['datumKreiranja'] as String),
  datumPromjeneStatusa: json['datumPromjeneStatusa'] == null
      ? null
      : DateTime.parse(json['datumPromjeneStatusa'] as String),
  statusPromijenioIme: json['statusPromijenioIme'] as String?,
  statusPromijenioPrezime: json['statusPromijenioPrezime'] as String?,
  razlogPromjeneStatusa: json['razlogPromjeneStatusa'] as String?,
  imaRecenziju: json['imaRecenziju'] as bool? ?? false,
  stavke:
      (json['stavke'] as List<dynamic>?)
          ?.map((e) => StavkeTermina.fromJson(e as Map<String, dynamic>))
          .toList() ??
      const [],
);

Map<String, dynamic> _$TerminiToJson(Termini instance) => <String, dynamic>{
  'terminId': instance.terminId,
  'sifra': instance.sifra,
  'korisnikId': instance.korisnikId,
  'korisnikIme': instance.korisnikIme,
  'korisnikPrezime': instance.korisnikPrezime,
  'veterinarId': instance.veterinarId,
  'veterinarIme': instance.veterinarIme,
  'veterinarPrezime': instance.veterinarPrezime,
  'ljubimacId': instance.ljubimacId,
  'ljubimacIme': instance.ljubimacIme,
  'datumTermina': const DateOnlyConverter().toJson(instance.datumTermina),
  'vrijemePocetka': instance.vrijemePocetka,
  'vrijemeKraja': instance.vrijemeKraja,
  'ukupnoTrajanje': instance.ukupnoTrajanje,
  'iznosPopusta': instance.iznosPopusta,
  'cijenaPrevoza': instance.cijenaPrevoza,
  'ukupnaCijena': instance.ukupnaCijena,
  'status': _$StatusTerminaEnumMap[instance.status]!,
  'nacinPlacanjaId': instance.nacinPlacanjaId,
  'nacinPlacanjaNaziv': instance.nacinPlacanjaNaziv,
  'jePlacen': instance.jePlacen,
  'jeRefundiran': instance.jeRefundiran,
  'jePrevoz': instance.jePrevoz,
  'adresaPrevoza': instance.adresaPrevoza,
  'gradPrevozaId': instance.gradPrevozaId,
  'gradPrevozaNaziv': instance.gradPrevozaNaziv,
  'datumKreiranja': instance.datumKreiranja.toIso8601String(),
  'datumPromjeneStatusa': instance.datumPromjeneStatusa?.toIso8601String(),
  'statusPromijenioIme': instance.statusPromijenioIme,
  'statusPromijenioPrezime': instance.statusPromijenioPrezime,
  'razlogPromjeneStatusa': instance.razlogPromjeneStatusa,
  'imaRecenziju': instance.imaRecenziju,
  'stavke': instance.stavke.map((e) => e.toJson()).toList(),
};

const _$StatusTerminaEnumMap = {
  StatusTermina.naCekanju: 'NaCekanju',
  StatusTermina.potvrdjen: 'Potvrdjen',
  StatusTermina.zavrsen: 'Zavrsen',
  StatusTermina.otkazan: 'Otkazan',
  StatusTermina.odbijen: 'Odbijen',
};
