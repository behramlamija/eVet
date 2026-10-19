import 'package:json_annotation/json_annotation.dart';

part 'rasa.g.dart';

@JsonSerializable(explicitToJson: true)
class Rasa {
  final int rasaId;
  final String naziv;
  final int vrstaLjubimcaId;
  final String vrstaLjubimcaNaziv;

  const Rasa({
    required this.rasaId,
    required this.naziv,
    required this.vrstaLjubimcaId,
    required this.vrstaLjubimcaNaziv,
  });

  factory Rasa.fromJson(Map<String, dynamic> json) => _$RasaFromJson(json);

  Map<String, dynamic> toJson() => _$RasaToJson(this);
}
