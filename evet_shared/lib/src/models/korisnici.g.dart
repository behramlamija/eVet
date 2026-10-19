// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'korisnici.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Korisnici _$KorisniciFromJson(Map<String, dynamic> json) => Korisnici(
  korisnikId: (json['korisnikId'] as num).toInt(),
  ime: json['ime'] as String,
  prezime: json['prezime'] as String,
  korisnickoIme: json['korisnickoIme'] as String,
  email: json['email'] as String,
  telefon: json['telefon'] as String?,
  imaSliku: json['imaSliku'] as bool? ?? false,
  jeAktivan: json['jeAktivan'] as bool? ?? false,
  datumRegistracije: DateTime.parse(json['datumRegistracije'] as String),
  datumRodjenja: const NullableDateOnlyConverter().fromJson(
    json['datumRodjenja'] as String?,
  ),
  gradRodjenjaId: (json['gradRodjenjaId'] as num?)?.toInt(),
  gradRodjenjaNaziv: json['gradRodjenjaNaziv'] as String?,
  datumZaposlenja: const NullableDateOnlyConverter().fromJson(
    json['datumZaposlenja'] as String?,
  ),
  godineIskustva: (json['godineIskustva'] as num?)?.toInt(),
  zvanje: json['zvanje'] as String?,
  specijalnostId: (json['specijalnostId'] as num?)?.toInt(),
  specijalnostNaziv: json['specijalnostNaziv'] as String?,
  uloge:
      (json['uloge'] as List<dynamic>?)
          ?.map((e) => Uloga.fromJson(e as Map<String, dynamic>))
          .toList() ??
      const [],
);

Map<String, dynamic> _$KorisniciToJson(Korisnici instance) => <String, dynamic>{
  'korisnikId': instance.korisnikId,
  'ime': instance.ime,
  'prezime': instance.prezime,
  'korisnickoIme': instance.korisnickoIme,
  'email': instance.email,
  'telefon': instance.telefon,
  'imaSliku': instance.imaSliku,
  'jeAktivan': instance.jeAktivan,
  'datumRegistracije': instance.datumRegistracije.toIso8601String(),
  'datumRodjenja': const NullableDateOnlyConverter().toJson(
    instance.datumRodjenja,
  ),
  'gradRodjenjaId': instance.gradRodjenjaId,
  'gradRodjenjaNaziv': instance.gradRodjenjaNaziv,
  'datumZaposlenja': const NullableDateOnlyConverter().toJson(
    instance.datumZaposlenja,
  ),
  'godineIskustva': instance.godineIskustva,
  'zvanje': instance.zvanje,
  'specijalnostId': instance.specijalnostId,
  'specijalnostNaziv': instance.specijalnostNaziv,
  'uloge': instance.uloge.map((e) => e.toJson()).toList(),
};
