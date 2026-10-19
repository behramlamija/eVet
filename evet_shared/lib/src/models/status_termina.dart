import 'package:json_annotation/json_annotation.dart';

@JsonEnum(fieldRename: FieldRename.pascal)
enum StatusTermina {
  naCekanju,
  potvrdjen,
  zavrsen,
  otkazan,
  odbijen,
}
