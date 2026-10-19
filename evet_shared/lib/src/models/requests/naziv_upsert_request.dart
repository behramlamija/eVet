import 'package:json_annotation/json_annotation.dart';

part 'naziv_upsert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class NazivUpsertRequest {
  final String naziv;

  const NazivUpsertRequest({
    required this.naziv,
  });

  factory NazivUpsertRequest.fromJson(Map<String, dynamic> json) => _$NazivUpsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$NazivUpsertRequestToJson(this);
}
