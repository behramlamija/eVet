import 'package:json_annotation/json_annotation.dart';
import '../utils/json_converters.dart';
import 'uloga.dart';

part 'korisnici.g.dart';

@JsonSerializable(explicitToJson: true)
class Korisnici {
  final int korisnikId;
  final String ime;
  final String prezime;
  final String korisnickoIme;
  final String email;
  final String? telefon;
  final bool imaSliku;
  final bool jeAktivan;
  final DateTime datumRegistracije;
  @NullableDateOnlyConverter()
  final DateTime? datumRodjenja;
  final int? gradRodjenjaId;
  final String? gradRodjenjaNaziv;
  @NullableDateOnlyConverter()
  final DateTime? datumZaposlenja;
  final int? godineIskustva;
  final String? zvanje;
  final int? specijalnostId;
  final String? specijalnostNaziv;
  final List<Uloga> uloge;

  const Korisnici({
    required this.korisnikId,
    required this.ime,
    required this.prezime,
    required this.korisnickoIme,
    required this.email,
    this.telefon,
    this.imaSliku = false,
    this.jeAktivan = false,
    required this.datumRegistracije,
    this.datumRodjenja,
    this.gradRodjenjaId,
    this.gradRodjenjaNaziv,
    this.datumZaposlenja,
    this.godineIskustva,
    this.zvanje,
    this.specijalnostId,
    this.specijalnostNaziv,
    this.uloge = const [],
  });

  factory Korisnici.fromJson(Map<String, dynamic> json) => _$KorisniciFromJson(json);

  Map<String, dynamic> toJson() => _$KorisniciToJson(this);
}
