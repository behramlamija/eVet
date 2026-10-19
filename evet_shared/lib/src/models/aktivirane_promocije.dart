import 'package:json_annotation/json_annotation.dart';

part 'aktivirane_promocije.g.dart';

@JsonSerializable(explicitToJson: true)
class AktiviranePromocije {
  final int aktiviranaPromocijaId;
  final int promocijaId;
  final String promocijaNaziv;
  final String promocijaKod;
  final double promocijaPopust;
  final DateTime promocijaDatumKraja;
  final int uslugaId;
  final String uslugaNaziv;
  final bool iskoristena;
  final DateTime datumAktiviranja;

  const AktiviranePromocije({
    required this.aktiviranaPromocijaId,
    required this.promocijaId,
    required this.promocijaNaziv,
    required this.promocijaKod,
    required this.promocijaPopust,
    required this.promocijaDatumKraja,
    required this.uslugaId,
    required this.uslugaNaziv,
    this.iskoristena = false,
    required this.datumAktiviranja,
  });

  factory AktiviranePromocije.fromJson(Map<String, dynamic> json) => _$AktiviranePromocijeFromJson(json);

  Map<String, dynamic> toJson() => _$AktiviranePromocijeToJson(this);
}
