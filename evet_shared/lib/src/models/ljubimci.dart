import 'package:json_annotation/json_annotation.dart';
import '../utils/json_converters.dart';
import 'spol.dart';

part 'ljubimci.g.dart';

@JsonSerializable(explicitToJson: true)
class Ljubimci {
  final int ljubimacId;
  final int korisnikId;
  final String korisnikIme;
  final String korisnikPrezime;
  final String ime;
  final int rasaId;
  final String rasaNaziv;
  final int vrstaLjubimcaId;
  final String vrstaLjubimcaNaziv;
  @NullableDateOnlyConverter()
  final DateTime? datumRodjenja;
  final Spol spol;
  final double? tezina;
  final String? napomena;
  final bool imaSliku;

  const Ljubimci({
    required this.ljubimacId,
    required this.korisnikId,
    required this.korisnikIme,
    required this.korisnikPrezime,
    required this.ime,
    required this.rasaId,
    required this.rasaNaziv,
    required this.vrstaLjubimcaId,
    required this.vrstaLjubimcaNaziv,
    this.datumRodjenja,
    required this.spol,
    this.tezina,
    this.napomena,
    this.imaSliku = false,
  });

  factory Ljubimci.fromJson(Map<String, dynamic> json) => _$LjubimciFromJson(json);

  Map<String, dynamic> toJson() => _$LjubimciToJson(this);
}
