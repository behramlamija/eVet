import 'package:json_annotation/json_annotation.dart';
import '../../utils/json_converters.dart';

part 'korisnici_insert_request.g.dart';

@JsonSerializable(explicitToJson: true)
class KorisniciInsertRequest {
  final String ime;
  final String prezime;
  final String korisnickoIme;
  final String email;
  final String? telefon;
  final String lozinka;
  final String lozinkaPotvrda;
  final bool jeAktivan;
  final List<int> ulogeIds;
  @NullableDateOnlyConverter()
  final DateTime? datumRodjenja;
  final int? gradRodjenjaId;
  @NullableDateOnlyConverter()
  final DateTime? datumZaposlenja;
  final String? zvanje;
  final int? specijalnostId;

  const KorisniciInsertRequest({
    required this.ime,
    required this.prezime,
    required this.korisnickoIme,
    required this.email,
    this.telefon,
    required this.lozinka,
    required this.lozinkaPotvrda,
    this.jeAktivan = false,
    this.ulogeIds = const [],
    this.datumRodjenja,
    this.gradRodjenjaId,
    this.datumZaposlenja,
    this.zvanje,
    this.specijalnostId,
  });

  factory KorisniciInsertRequest.fromJson(Map<String, dynamic> json) => _$KorisniciInsertRequestFromJson(json);

  Map<String, dynamic> toJson() => _$KorisniciInsertRequestToJson(this);
}
