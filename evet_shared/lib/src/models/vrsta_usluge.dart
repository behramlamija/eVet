import 'package:json_annotation/json_annotation.dart';

part 'vrsta_usluge.g.dart';

@JsonSerializable(explicitToJson: true)
class VrstaUsluge {
  final int vrstaId;
  final String naziv;
  final bool jeOperacija;
  final bool jeVakcinacija;
  final bool imaSliku;

  const VrstaUsluge({
    required this.vrstaId,
    required this.naziv,
    this.jeOperacija = false,
    this.jeVakcinacija = false,
    this.imaSliku = false,
  });

  factory VrstaUsluge.fromJson(Map<String, dynamic> json) => _$VrstaUslugeFromJson(json);

  Map<String, dynamic> toJson() => _$VrstaUslugeToJson(this);
}
