import 'package:json_annotation/json_annotation.dart';

part 'obaveze.g.dart';

@JsonSerializable(explicitToJson: true)
class Obaveze {
  final int obavezaId;
  final String naslov;
  final String? opis;
  final DateTime datumVrijeme;
  final bool jeZavrsena;

  const Obaveze({
    required this.obavezaId,
    required this.naslov,
    this.opis,
    required this.datumVrijeme,
    this.jeZavrsena = false,
  });

  factory Obaveze.fromJson(Map<String, dynamic> json) => _$ObavezeFromJson(json);

  Map<String, dynamic> toJson() => _$ObavezeToJson(this);
}
