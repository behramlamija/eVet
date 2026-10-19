import 'package:json_annotation/json_annotation.dart';

import 'formatters.dart';

/// C# DateOnly se serijalizuje kao "yyyy-MM-dd", bez vremena i vremenske zone.
class DateOnlyConverter implements JsonConverter<DateTime, String> {
  const DateOnlyConverter();

  @override
  DateTime fromJson(String json) => DateTime.parse(json);

  @override
  String toJson(DateTime object) => formatDatumApi(object);
}

class NullableDateOnlyConverter implements JsonConverter<DateTime?, String?> {
  const NullableDateOnlyConverter();

  @override
  DateTime? fromJson(String? json) => json == null ? null : DateTime.parse(json);

  @override
  String? toJson(DateTime? object) => object == null ? null : formatDatumApi(object);
}
