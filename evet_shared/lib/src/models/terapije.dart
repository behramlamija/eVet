import 'package:json_annotation/json_annotation.dart';
import '../utils/json_converters.dart';

part 'terapije.g.dart';

@JsonSerializable(explicitToJson: true)
class Terapije {
  final int terapijaId;
  final int ljubimacId;
  final String ljubimacIme;
  final int veterinarId;
  final String veterinarIme;
  final String veterinarPrezime;
  final int? terminId;
  final String? terminSifra;
  final String dijagnoza;
  final String? lijek;
  final String? doza;
  final String? napomena;
  final DateTime datumTerapije;
  final int? trajanjeDana;
  final bool jeZavrsena;
  @NullableDateOnlyConverter()
  final DateTime? datumKontrolnog;

  const Terapije({
    required this.terapijaId,
    required this.ljubimacId,
    required this.ljubimacIme,
    required this.veterinarId,
    required this.veterinarIme,
    required this.veterinarPrezime,
    this.terminId,
    this.terminSifra,
    required this.dijagnoza,
    this.lijek,
    this.doza,
    this.napomena,
    required this.datumTerapije,
    this.trajanjeDana,
    this.jeZavrsena = false,
    this.datumKontrolnog,
  });

  factory Terapije.fromJson(Map<String, dynamic> json) => _$TerapijeFromJson(json);

  Map<String, dynamic> toJson() => _$TerapijeToJson(this);
}
