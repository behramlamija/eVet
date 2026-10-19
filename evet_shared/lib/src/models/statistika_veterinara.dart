import 'package:json_annotation/json_annotation.dart';

part 'statistika_veterinara.g.dart';

@JsonSerializable(explicitToJson: true)
class StatistikaVeterinara {
  final int korisnikId;
  final String ime;
  final String prezime;
  final bool imaSliku;
  final String? specijalnostNaziv;
  final bool jeAktivan;
  final int brojTermina;
  final int brojZavrsenihTermina;
  final int pozitivneRecenzije;
  final int negativneRecenzije;

  const StatistikaVeterinara({
    required this.korisnikId,
    required this.ime,
    required this.prezime,
    this.imaSliku = false,
    this.specijalnostNaziv,
    this.jeAktivan = false,
    required this.brojTermina,
    required this.brojZavrsenihTermina,
    required this.pozitivneRecenzije,
    required this.negativneRecenzije,
  });

  factory StatistikaVeterinara.fromJson(Map<String, dynamic> json) => _$StatistikaVeterinaraFromJson(json);

  Map<String, dynamic> toJson() => _$StatistikaVeterinaraToJson(this);
}
