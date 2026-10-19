import 'package:json_annotation/json_annotation.dart';

part 'recenzija_odgovori.g.dart';

@JsonSerializable(explicitToJson: true)
class RecenzijaOdgovori {
  final int recenzijaOdgovorId;
  final String korisnikIme;
  final String korisnikPrezime;
  final String komentar;
  final DateTime datumDodavanja;

  const RecenzijaOdgovori({
    required this.recenzijaOdgovorId,
    required this.korisnikIme,
    required this.korisnikPrezime,
    required this.komentar,
    required this.datumDodavanja,
  });

  factory RecenzijaOdgovori.fromJson(Map<String, dynamic> json) => _$RecenzijaOdgovoriFromJson(json);

  Map<String, dynamic> toJson() => _$RecenzijaOdgovoriToJson(this);
}
