import 'package:json_annotation/json_annotation.dart';

part 'error_response.g.dart';

@JsonSerializable(explicitToJson: true)
class ErrorResponse {
  final String poruka;
  final String? traceId;

  const ErrorResponse({
    required this.poruka,
    this.traceId,
  });

  factory ErrorResponse.fromJson(Map<String, dynamic> json) => _$ErrorResponseFromJson(json);

  Map<String, dynamic> toJson() => _$ErrorResponseToJson(this);
}
