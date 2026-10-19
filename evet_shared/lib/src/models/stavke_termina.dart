import 'package:json_annotation/json_annotation.dart';

part 'stavke_termina.g.dart';

@JsonSerializable(explicitToJson: true)
class StavkeTermina {
  final int stavkaTerminaId;
  final int uslugaId;
  final String uslugaNaziv;
  final double cijena;

  const StavkeTermina({
    required this.stavkaTerminaId,
    required this.uslugaId,
    required this.uslugaNaziv,
    required this.cijena,
  });

  factory StavkeTermina.fromJson(Map<String, dynamic> json) => _$StavkeTerminaFromJson(json);

  Map<String, dynamic> toJson() => _$StavkeTerminaToJson(this);
}
