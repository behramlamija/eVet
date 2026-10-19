import 'package:json_annotation/json_annotation.dart';
import 'usluge.dart';

part 'preporuke_usluga.g.dart';

@JsonSerializable(explicitToJson: true)
class PreporukeUsluga {
  final Usluge usluga;
  final int uslugaIzvorId;
  final String uslugaIzvorNaziv;
  final int brojZajednickihTermina;
  final int brojTerminaIzvora;
  final double pouzdanost;
  final String objasnjenje;

  const PreporukeUsluga({
    required this.usluga,
    required this.uslugaIzvorId,
    required this.uslugaIzvorNaziv,
    required this.brojZajednickihTermina,
    required this.brojTerminaIzvora,
    required this.pouzdanost,
    required this.objasnjenje,
  });

  factory PreporukeUsluga.fromJson(Map<String, dynamic> json) => _$PreporukeUslugaFromJson(json);

  Map<String, dynamic> toJson() => _$PreporukeUslugaToJson(this);
}
