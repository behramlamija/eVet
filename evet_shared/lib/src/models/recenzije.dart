import 'package:json_annotation/json_annotation.dart';
import 'recenzija_odgovori.dart';

part 'recenzije.g.dart';

@JsonSerializable(explicitToJson: true)
class Recenzije {
  final int recenzijaId;
  final int terminId;
  final String terminSifra;
  final int korisnikId;
  final String korisnikIme;
  final String korisnikPrezime;
  final int veterinarId;
  final String veterinarIme;
  final String veterinarPrezime;
  final bool uslugaPozitivna;
  final bool veterinarPozitivan;
  final String? komentar;
  final DateTime datumDodavanja;
  final List<RecenzijaOdgovori> odgovori;

  const Recenzije({
    required this.recenzijaId,
    required this.terminId,
    required this.terminSifra,
    required this.korisnikId,
    required this.korisnikIme,
    required this.korisnikPrezime,
    required this.veterinarId,
    required this.veterinarIme,
    required this.veterinarPrezime,
    this.uslugaPozitivna = false,
    this.veterinarPozitivan = false,
    this.komentar,
    required this.datumDodavanja,
    this.odgovori = const [],
  });

  factory Recenzije.fromJson(Map<String, dynamic> json) => _$RecenzijeFromJson(json);

  Map<String, dynamic> toJson() => _$RecenzijeToJson(this);
}
