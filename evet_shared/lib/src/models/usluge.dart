import 'package:json_annotation/json_annotation.dart';

part 'usluge.g.dart';

@JsonSerializable(explicitToJson: true)
class Usluge {
  final int uslugaId;
  final String naziv;
  final String? opis;
  final double cijena;
  final int trajanje;
  final int vrstaId;
  final String vrstaNaziv;
  final DateTime datumObjavljivanja;
  final bool imaSliku;

  const Usluge({
    required this.uslugaId,
    required this.naziv,
    this.opis,
    required this.cijena,
    required this.trajanje,
    required this.vrstaId,
    required this.vrstaNaziv,
    required this.datumObjavljivanja,
    this.imaSliku = false,
  });

  factory Usluge.fromJson(Map<String, dynamic> json) => _$UslugeFromJson(json);

  Map<String, dynamic> toJson() => _$UslugeToJson(this);
}
