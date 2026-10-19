import 'package:json_annotation/json_annotation.dart';

part 'recenzija_odgovor_request.g.dart';

@JsonSerializable(explicitToJson: true)
class RecenzijaOdgovorRequest {
  final String komentar;

  const RecenzijaOdgovorRequest({
    required this.komentar,
  });

  factory RecenzijaOdgovorRequest.fromJson(Map<String, dynamic> json) => _$RecenzijaOdgovorRequestFromJson(json);

  Map<String, dynamic> toJson() => _$RecenzijaOdgovorRequestToJson(this);
}
