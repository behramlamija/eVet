import 'package:json_annotation/json_annotation.dart';
import '../../utils/json_converters.dart';

part 'korisnici_update_request.g.dart';

@JsonSerializable(explicitToJson: true)
class KorisniciUpdateRequest {
  final String ime;
  final String prezime;
  final String email;
  final String? telefon;
  final bool jeAktivan;
  final List<int> ulogeIds;
  final String? lozinka;
  final String? lozinkaPotvrda;
  @NullableDateOnlyConverter()
  final DateTime? datumRodjenja;
  final int? gradRodjenjaId;
  @NullableDateOnlyConverter()
  final DateTime? datumZaposlenja;
  final String? zvanje;
  final int? specijalnostId;

  const KorisniciUpdateRequest({
    required this.ime,
    required this.prezime,
    required this.email,
    this.telefon,
    this.jeAktivan = false,
    this.ulogeIds = const [],
    this.lozinka,
    this.lozinkaPotvrda,
    this.datumRodjenja,
    this.gradRodjenjaId,
    this.datumZaposlenja,
    this.zvanje,
    this.specijalnostId,
  });

  factory KorisniciUpdateRequest.fromJson(Map<String, dynamic> json) => _$KorisniciUpdateRequestFromJson(json);

  Map<String, dynamic> toJson() => _$KorisniciUpdateRequestToJson(this);
}
