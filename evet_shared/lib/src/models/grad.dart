import 'package:json_annotation/json_annotation.dart';

part 'grad.g.dart';

@JsonSerializable(explicitToJson: true)
class Grad {
  final int gradId;
  final String naziv;
  final String postanskiBroj;

  const Grad({
    required this.gradId,
    required this.naziv,
    required this.postanskiBroj,
  });

  factory Grad.fromJson(Map<String, dynamic> json) => _$GradFromJson(json);

  Map<String, dynamic> toJson() => _$GradToJson(this);
}
