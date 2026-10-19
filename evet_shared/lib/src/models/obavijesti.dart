import 'package:json_annotation/json_annotation.dart';

part 'obavijesti.g.dart';

@JsonSerializable(explicitToJson: true)
class Obavijesti {
  final int obavijestId;
  final String naslov;
  final String sadrzaj;
  final bool jePogledana;
  final DateTime datumObavijesti;

  const Obavijesti({
    required this.obavijestId,
    required this.naslov,
    required this.sadrzaj,
    this.jePogledana = false,
    required this.datumObavijesti,
  });

  factory Obavijesti.fromJson(Map<String, dynamic> json) => _$ObavijestiFromJson(json);

  Map<String, dynamic> toJson() => _$ObavijestiToJson(this);
}
