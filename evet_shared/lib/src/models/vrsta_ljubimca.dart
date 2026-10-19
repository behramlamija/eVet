import 'package:json_annotation/json_annotation.dart';

part 'vrsta_ljubimca.g.dart';

@JsonSerializable(explicitToJson: true)
class VrstaLjubimca {
  final int vrstaLjubimcaId;
  final String naziv;

  const VrstaLjubimca({
    required this.vrstaLjubimcaId,
    required this.naziv,
  });

  factory VrstaLjubimca.fromJson(Map<String, dynamic> json) => _$VrstaLjubimcaFromJson(json);

  Map<String, dynamic> toJson() => _$VrstaLjubimcaToJson(this);
}
