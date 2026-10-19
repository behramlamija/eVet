import 'package:json_annotation/json_annotation.dart';
import '../../utils/json_converters.dart';

part 'terapije_upsert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class TerapijeUpsertRequest {
  final int ljubimacId;
  final int? terminId;
  final String dijagnoza;
  final String? lijek;
  final String? doza;
  final String? napomena;
  final int? trajanjeDana;
  @NullableDateOnlyConverter()
  final DateTime? datumKontrolnog;

  const TerapijeUpsertRequest({
    required this.ljubimacId,
    this.terminId,
    required this.dijagnoza,
    this.lijek,
    this.doza,
    this.napomena,
    this.trajanjeDana,
    this.datumKontrolnog,
  });

  factory TerapijeUpsertRequest.fromJson(Map<String, dynamic> json) => _$TerapijeUpsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$TerapijeUpsertRequestToJson(this);
}
