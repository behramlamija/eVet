import 'package:json_annotation/json_annotation.dart';

part 'promjena_statusa_request.g.dart';

@JsonSerializable(explicitToJson: true)
class PromjenaStatusaRequest {
  final String razlog;

  const PromjenaStatusaRequest({
    required this.razlog,
  });

  factory PromjenaStatusaRequest.fromJson(Map<String, dynamic> json) => _$PromjenaStatusaRequestFromJson(json);

  Map<String, dynamic> toJson() => _$PromjenaStatusaRequestToJson(this);
}
