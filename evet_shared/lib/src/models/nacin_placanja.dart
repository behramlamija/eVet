import 'package:json_annotation/json_annotation.dart';

part 'nacin_placanja.g.dart';

@JsonSerializable(explicitToJson: true)
class NacinPlacanja {
  final int nacinPlacanjaId;
  final String naziv;

  const NacinPlacanja({
    required this.nacinPlacanjaId,
    required this.naziv,
  });

  factory NacinPlacanja.fromJson(Map<String, dynamic> json) => _$NacinPlacanjaFromJson(json);

  Map<String, dynamic> toJson() => _$NacinPlacanjaToJson(this);
}
