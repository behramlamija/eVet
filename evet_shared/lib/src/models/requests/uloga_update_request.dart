import 'package:json_annotation/json_annotation.dart';

part 'uloga_update_request.g.dart';

@JsonSerializable(explicitToJson: true)
class UlogaUpdateRequest {
  final String? opis;

  const UlogaUpdateRequest({
    this.opis,
  });

  factory UlogaUpdateRequest.fromJson(Map<String, dynamic> json) => _$UlogaUpdateRequestFromJson(json);

  Map<String, dynamic> toJson() => _$UlogaUpdateRequestToJson(this);
}
