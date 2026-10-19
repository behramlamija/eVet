import 'package:intl/intl.dart';

final _datumApi = DateFormat('yyyy-MM-dd');
final _datum = DateFormat('dd.MM.yyyy.');
final _datumVrijeme = DateFormat('dd.MM.yyyy. HH:mm');
final _novac = NumberFormat.currency(locale: 'bs', symbol: 'KM', decimalDigits: 2);

/// Format koji API očekuje za DateOnly vrijednosti.
String formatDatumApi(DateTime datum) => _datumApi.format(datum);

String formatDatum(DateTime? datum) => datum == null ? '-' : _datum.format(datum.toLocal());

String formatDatumVrijeme(DateTime? datum) => datum == null ? '-' : _datumVrijeme.format(datum.toLocal());

String formatNovac(num iznos) => _novac.format(iznos);

/// API šalje vrijeme kao "HH:mm:ss"; korisniku se prikazuje "HH:mm".
String formatVrijeme(String vrijeme) => vrijeme.length >= 5 ? vrijeme.substring(0, 5) : vrijeme;
