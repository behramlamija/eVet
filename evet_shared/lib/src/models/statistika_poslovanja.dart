import 'package:json_annotation/json_annotation.dart';

part 'statistika_poslovanja.g.dart';

@JsonSerializable(explicitToJson: true)
class StatistikaPoslovanja {
  final int brojTermina;
  final int brojZavrsenihTermina;
  final int brojOtkazanihTermina;
  final int brojPruzenihUsluga;
  final double ukupniPrihod;
  final int brojOperacija;
  final int brojNovihKlijenata;
  final String? najcescaUsluga;
  final String? najzastupljenijaRasa;
  final String? topVeterinar;

  const StatistikaPoslovanja({
    required this.brojTermina,
    required this.brojZavrsenihTermina,
    required this.brojOtkazanihTermina,
    required this.brojPruzenihUsluga,
    required this.ukupniPrihod,
    required this.brojOperacija,
    required this.brojNovihKlijenata,
    this.najcescaUsluga,
    this.najzastupljenijaRasa,
    this.topVeterinar,
  });

  factory StatistikaPoslovanja.fromJson(Map<String, dynamic> json) => _$StatistikaPoslovanjaFromJson(json);

  Map<String, dynamic> toJson() => _$StatistikaPoslovanjaToJson(this);
}
