import 'package:json_annotation/json_annotation.dart';

part 'korisnik_osnovno.g.dart';

@JsonSerializable(explicitToJson: true)
class KorisnikOsnovno {
  final int korisnikId;
  final String ime;
  final String prezime;
  final String email;
  final String? telefon;
  final String? zvanje;
  final String? specijalnostNaziv;
  final bool imaSliku;

  const KorisnikOsnovno({
    required this.korisnikId,
    required this.ime,
    required this.prezime,
    required this.email,
    this.telefon,
    this.zvanje,
    this.specijalnostNaziv,
    this.imaSliku = false,
  });

  factory KorisnikOsnovno.fromJson(Map<String, dynamic> json) => _$KorisnikOsnovnoFromJson(json);

  Map<String, dynamic> toJson() => _$KorisnikOsnovnoToJson(this);
}
