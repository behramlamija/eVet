import 'package:json_annotation/json_annotation.dart';

part 'rasa_upsert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class RasaUpsertRequest {
  final String naziv;
  final int vrstaLjubimcaId;

  const RasaUpsertRequest({
    required this.naziv,
    required this.vrstaLjubimcaId,
  });

  factory RasaUpsertRequest.fromJson(Map<String, dynamic> json) => _$RasaUpsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$RasaUpsertRequestToJson(this);
}
