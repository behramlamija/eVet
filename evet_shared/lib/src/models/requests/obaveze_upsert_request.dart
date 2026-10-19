import 'package:json_annotation/json_annotation.dart';

part 'obaveze_upsert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class ObavezeUpsertRequest {
  final String naslov;
  final String? opis;
  final DateTime? datumVrijeme;
  final bool jeZavrsena;

  const ObavezeUpsertRequest({
    required this.naslov,
    this.opis,
    this.datumVrijeme,
    this.jeZavrsena = false,
  });

  factory ObavezeUpsertRequest.fromJson(Map<String, dynamic> json) => _$ObavezeUpsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$ObavezeUpsertRequestToJson(this);
}
