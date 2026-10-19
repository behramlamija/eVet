import 'package:json_annotation/json_annotation.dart';

part 'usluge_upsert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class UslugeUpsertRequest {
  final String naziv;
  final String? opis;
  final double cijena;
  final int trajanje;
  final int vrstaId;

  const UslugeUpsertRequest({
    required this.naziv,
    this.opis,
    required this.cijena,
    required this.trajanje,
    required this.vrstaId,
  });

  factory UslugeUpsertRequest.fromJson(Map<String, dynamic> json) => _$UslugeUpsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$UslugeUpsertRequestToJson(this);
}
