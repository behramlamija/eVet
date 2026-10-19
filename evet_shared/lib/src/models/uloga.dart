import 'package:json_annotation/json_annotation.dart';

part 'uloga.g.dart';

@JsonSerializable(explicitToJson: true)
class Uloga {
  final int ulogaId;
  final String naziv;
  final String? opis;

  const Uloga({
    required this.ulogaId,
    required this.naziv,
    this.opis,
  });

  factory Uloga.fromJson(Map<String, dynamic> json) => _$UlogaFromJson(json);

  Map<String, dynamic> toJson() => _$UlogaToJson(this);
}
