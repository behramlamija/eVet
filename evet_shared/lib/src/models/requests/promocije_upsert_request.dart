import 'package:json_annotation/json_annotation.dart';

part 'promocije_upsert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class PromocijeUpsertRequest {
  final String naziv;
  final String? opis;
  final String kod;
  final double popust;
  final DateTime? datumPocetka;
  final DateTime? datumKraja;
  final int uslugaId;
  final bool jeAktivna;

  const PromocijeUpsertRequest({
    required this.naziv,
    this.opis,
    required this.kod,
    required this.popust,
    this.datumPocetka,
    this.datumKraja,
    required this.uslugaId,
    this.jeAktivna = false,
  });

  factory PromocijeUpsertRequest.fromJson(Map<String, dynamic> json) => _$PromocijeUpsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$PromocijeUpsertRequestToJson(this);
}
