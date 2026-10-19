import 'dart:async';
import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;
import 'package:http_parser/http_parser.dart';

import '../utils/formatters.dart';
import 'api_exception.dart';

/// Jedino mjesto koje komunicira s API-jem: dodaje token, gradi URL i pretvara greške u [ApiException].
class ApiClient {
  ApiClient({required String baseUrl, http.Client? client})
      : baseUrl = baseUrl.endsWith('/') ? baseUrl.substring(0, baseUrl.length - 1) : baseUrl,
        _client = client ?? http.Client();

  static const _timeout = Duration(seconds: 30);

  final String baseUrl;
  final http.Client _client;
  String? _token;

  /// Poziva se kada API vrati 401 (istekla ili poništena sesija), da aplikacija vrati korisnika na prijavu.
  void Function()? onUnauthorized;

  void setToken(String? token) => _token = token;

  Map<String, String> get authHeaders => {if (_token != null) HttpHeaders.authorizationHeader: 'Bearer $_token'};

  Map<String, String> get _jsonHeaders => {...authHeaders, HttpHeaders.contentTypeHeader: 'application/json'};

  Uri uri(String path, [Map<String, dynamic>? query]) {
    final parametri = <String, String>{};
    query?.forEach((kljuc, vrijednost) {
      if (vrijednost == null || (vrijednost is String && vrijednost.trim().isEmpty)) return;
      parametri[kljuc] = switch (vrijednost) {
        DateTime d => formatDatumApi(d),
        Enum e => e.name,
        _ => vrijednost.toString(),
      };
    });
    return Uri.parse('$baseUrl/$path').replace(queryParameters: parametri.isEmpty ? null : parametri);
  }

  Future<dynamic> get(String path, {Map<String, dynamic>? query}) =>
      _posalji(() => _client.get(uri(path, query), headers: _jsonHeaders));

  Future<dynamic> post(String path, {Object? body}) =>
      _posalji(() => _client.post(uri(path), headers: _jsonHeaders, body: body == null ? null : jsonEncode(body)));

  Future<dynamic> put(String path, {Object? body}) =>
      _posalji(() => _client.put(uri(path), headers: _jsonHeaders, body: body == null ? null : jsonEncode(body)));

  Future<dynamic> delete(String path) => _posalji(() => _client.delete(uri(path), headers: _jsonHeaders));

  Future<void> putMultipart(String path, {required String polje, required List<int> bytes, required String nazivDatoteke, required String contentType}) {
    return _posalji(() async {
      final zahtjev = http.MultipartRequest('PUT', uri(path))
        ..headers.addAll(authHeaders)
        ..files.add(http.MultipartFile.fromBytes(polje, bytes, filename: nazivDatoteke, contentType: MediaType.parse(contentType)));
      return http.Response.fromStream(await _client.send(zahtjev));
    });
  }

  Future<dynamic> _posalji(Future<http.Response> Function() zahtjev) async {
    final http.Response odgovor;
    try {
      odgovor = await zahtjev().timeout(_timeout);
    } on SocketException {
      throw const ApiException('Server nije dostupan. Provjerite internet konekciju i da li je API pokrenut.');
    } on TimeoutException {
      throw const ApiException('Server ne odgovara. Pokušajte ponovo za nekoliko trenutaka.');
    }
    return _obradi(odgovor);
  }

  dynamic _obradi(http.Response odgovor) {
    final tijelo = utf8.decode(odgovor.bodyBytes);

    if (odgovor.statusCode >= 200 && odgovor.statusCode < 300) {
      return tijelo.isEmpty ? null : jsonDecode(tijelo);
    }

    if (odgovor.statusCode == HttpStatus.unauthorized && _token != null) {
      onUnauthorized?.call();
    }

    // Backend uvijek vraća { poruka, greske, traceId }; poruka se prosljeđuje korisniku.
    try {
      final json = jsonDecode(tijelo) as Map<String, dynamic>;
      final greske = (json['greske'] as Map<String, dynamic>?)?.map(
            (k, v) => MapEntry(k, (v as List).map((e) => e.toString()).toList()),
          ) ??
          const <String, List<String>>{};
      throw ApiException(json['poruka'] as String? ?? 'Došlo je do greške.', greske: greske, statusCode: odgovor.statusCode);
    } on FormatException {
      throw ApiException('Došlo je do neočekivane greške (status ${odgovor.statusCode}).', statusCode: odgovor.statusCode);
    }
  }
}
