import 'package:json_annotation/json_annotation.dart';
import '../utils/json_converters.dart';
import 'status_termina.dart';
import 'stavke_termina.dart';

part 'termini.g.dart';

@JsonSerializable(explicitToJson: true)
class Termini {
  final int terminId;
  final String sifra;
  final int korisnikId;
  final String korisnikIme;
  final String korisnikPrezime;
  final int veterinarId;
  final String veterinarIme;
  final String veterinarPrezime;
  final int ljubimacId;
  final String ljubimacIme;
  @DateOnlyConverter()
  final DateTime datumTermina;
  final String vrijemePocetka;
  final String vrijemeKraja;
  final int ukupnoTrajanje;
  final double iznosPopusta;
  final double cijenaPrevoza;
  final double ukupnaCijena;
  final StatusTermina status;
  final int nacinPlacanjaId;
  final String nacinPlacanjaNaziv;
  final bool jePlacen;
  final bool jePrevoz;
  final String? adresaPrevoza;
  final int? gradPrevozaId;
  final String? gradPrevozaNaziv;
  final DateTime datumKreiranja;
  final DateTime? datumPromjeneStatusa;
  final String? statusPromijenioIme;
  final String? statusPromijenioPrezime;
  final String? razlogPromjeneStatusa;
  final bool imaRecenziju;
  final List<StavkeTermina> stavke;

  const Termini({
    required this.terminId,
    required this.sifra,
    required this.korisnikId,
    required this.korisnikIme,
    required this.korisnikPrezime,
    required this.veterinarId,
    required this.veterinarIme,
    required this.veterinarPrezime,
    required this.ljubimacId,
    required this.ljubimacIme,
    required this.datumTermina,
    required this.vrijemePocetka,
    required this.vrijemeKraja,
    required this.ukupnoTrajanje,
    required this.iznosPopusta,
    required this.cijenaPrevoza,
    required this.ukupnaCijena,
    required this.status,
    required this.nacinPlacanjaId,
    required this.nacinPlacanjaNaziv,
    this.jePlacen = false,
    this.jePrevoz = false,
    this.adresaPrevoza,
    this.gradPrevozaId,
    this.gradPrevozaNaziv,
    required this.datumKreiranja,
    this.datumPromjeneStatusa,
    this.statusPromijenioIme,
    this.statusPromijenioPrezime,
    this.razlogPromjeneStatusa,
    this.imaRecenziju = false,
    this.stavke = const [],
  });

  factory Termini.fromJson(Map<String, dynamic> json) => _$TerminiFromJson(json);

  Map<String, dynamic> toJson() => _$TerminiToJson(this);
}
