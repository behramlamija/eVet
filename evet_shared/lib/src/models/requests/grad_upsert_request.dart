import 'package:json_annotation/json_annotation.dart';

part 'grad_upsert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class GradUpsertRequest {
  final String naziv;
  final String postanskiBroj;

  const GradUpsertRequest({
    required this.naziv,
    required this.postanskiBroj,
  });

  factory GradUpsertRequest.fromJson(Map<String, dynamic> json) => _$GradUpsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$GradUpsertRequestToJson(this);
}
