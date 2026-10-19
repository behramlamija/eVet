import 'package:json_annotation/json_annotation.dart';
import '../../utils/json_converters.dart';
import '../spol.dart';

part 'ljubimci_upsert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class LjubimciUpsertRequest {
  final String ime;
  final int rasaId;
  @NullableDateOnlyConverter()
  final DateTime? datumRodjenja;
  final Spol spol;
  final double? tezina;
  final String? napomena;
  final int? vlasnikId;

  const LjubimciUpsertRequest({
    required this.ime,
    required this.rasaId,
    this.datumRodjenja,
    required this.spol,
    this.tezina,
    this.napomena,
    this.vlasnikId,
  });

  factory LjubimciUpsertRequest.fromJson(Map<String, dynamic> json) => _$LjubimciUpsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$LjubimciUpsertRequestToJson(this);
}
