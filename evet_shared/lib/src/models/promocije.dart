import 'package:json_annotation/json_annotation.dart';

part 'promocije.g.dart';

@JsonSerializable(explicitToJson: true)
class Promocije {
  final int promocijaId;
  final String naziv;
  final String? opis;
  final String kod;
  final double popust;
  final DateTime datumPocetka;
  final DateTime datumKraja;
  final int uslugaId;
  final String uslugaNaziv;
  final double uslugaCijena;
  final bool jeAktivna;

  const Promocije({
    required this.promocijaId,
    required this.naziv,
    this.opis,
    required this.kod,
    required this.popust,
    required this.datumPocetka,
    required this.datumKraja,
    required this.uslugaId,
    required this.uslugaNaziv,
    required this.uslugaCijena,
    this.jeAktivna = false,
  });

  factory Promocije.fromJson(Map<String, dynamic> json) => _$PromocijeFromJson(json);

  Map<String, dynamic> toJson() => _$PromocijeToJson(this);
}
