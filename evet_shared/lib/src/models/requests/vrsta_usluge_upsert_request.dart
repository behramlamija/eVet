import 'package:json_annotation/json_annotation.dart';

part 'vrsta_usluge_upsert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class VrstaUslugeUpsertRequest {
  final String naziv;
  final bool jeOperacija;
  final bool jeVakcinacija;

  const VrstaUslugeUpsertRequest({
    required this.naziv,
    this.jeOperacija = false,
    this.jeVakcinacija = false,
  });

  factory VrstaUslugeUpsertRequest.fromJson(Map<String, dynamic> json) => _$VrstaUslugeUpsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$VrstaUslugeUpsertRequestToJson(this);
}
