import 'package:json_annotation/json_annotation.dart';

part 'statistika_period.g.dart';

@JsonSerializable(explicitToJson: true)
class StatistikaPeriod {
  final int godina;
  final int? mjesec;
  final int brojTermina;
  final int brojOperacija;
  final double prihod;

  const StatistikaPeriod({
    required this.godina,
    this.mjesec,
    required this.brojTermina,
    required this.brojOperacija,
    required this.prihod,
  });

  factory StatistikaPeriod.fromJson(Map<String, dynamic> json) => _$StatistikaPeriodFromJson(json);

  Map<String, dynamic> toJson() => _$StatistikaPeriodToJson(this);
}
