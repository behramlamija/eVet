import 'package:json_annotation/json_annotation.dart';

part 'specijalnost.g.dart';

@JsonSerializable(explicitToJson: true)
class Specijalnost {
  final int specijalnostId;
  final String naziv;

  const Specijalnost({
    required this.specijalnostId,
    required this.naziv,
  });

  factory Specijalnost.fromJson(Map<String, dynamic> json) => _$SpecijalnostFromJson(json);

  Map<String, dynamic> toJson() => _$SpecijalnostToJson(this);
}
