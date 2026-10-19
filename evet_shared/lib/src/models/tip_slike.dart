import 'package:json_annotation/json_annotation.dart';

@JsonEnum(fieldRename: FieldRename.pascal)
enum TipSlike {
  korisnik,
  ljubimac,
  usluga,
  vrstaUsluge,
}
