import 'package:json_annotation/json_annotation.dart';

part 'termini_konfiguracija.g.dart';

@JsonSerializable(explicitToJson: true)
class TerminiKonfiguracija {
  final double cijenaPrevoza;
  final int minimalnoSatiZaOtkazivanje;
  final int maksimalnoDanaUnaprijed;
  final int radnoVrijemeOdSat;
  final int radnoVrijemeDoSat;

  const TerminiKonfiguracija({
    required this.cijenaPrevoza,
    required this.minimalnoSatiZaOtkazivanje,
    required this.maksimalnoDanaUnaprijed,
    required this.radnoVrijemeOdSat,
    required this.radnoVrijemeDoSat,
  });

  factory TerminiKonfiguracija.fromJson(Map<String, dynamic> json) => _$TerminiKonfiguracijaFromJson(json);

  Map<String, dynamic> toJson() => _$TerminiKonfiguracijaToJson(this);
}
