import 'package:json_annotation/json_annotation.dart';

part 'recenzija_insert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class RecenzijaInsertRequest {
  final int terminId;
  final bool uslugaPozitivna;
  final bool veterinarPozitivan;
  final String? komentar;

  const RecenzijaInsertRequest({
    required this.terminId,
    this.uslugaPozitivna = false,
    this.veterinarPozitivan = false,
    this.komentar,
  });

  factory RecenzijaInsertRequest.fromJson(Map<String, dynamic> json) => _$RecenzijaInsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$RecenzijaInsertRequestToJson(this);
}
