import 'package:json_annotation/json_annotation.dart';
import '../../utils/json_converters.dart';

part 'termin_insert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class TerminInsertRequest {
  final int veterinarId;
  final int ljubimacId;
  @NullableDateOnlyConverter()
  final DateTime? datumTermina;
  final String? vrijemePocetka;
  final List<int> uslugeIds;
  final int nacinPlacanjaId;
  final bool jePrevoz;
  final String? adresaPrevoza;
  final int? gradPrevozaId;
  final String? kodPromocije;

  const TerminInsertRequest({
    required this.veterinarId,
    required this.ljubimacId,
    this.datumTermina,
    this.vrijemePocetka,
    this.uslugeIds = const [],
    required this.nacinPlacanjaId,
    this.jePrevoz = false,
    this.adresaPrevoza,
    this.gradPrevozaId,
    this.kodPromocije,
  });

  factory TerminInsertRequest.fromJson(Map<String, dynamic> json) => _$TerminInsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$TerminInsertRequestToJson(this);
}
