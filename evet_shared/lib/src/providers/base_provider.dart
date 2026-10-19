import 'package:flutter/foundation.dart';

import '../api/api_client.dart';
import '../models/paged_result.dart';

/// Zajednička logika za list, detalj i CRUD endpointe jednog resursa.
abstract class BaseProvider<T> extends ChangeNotifier {
  BaseProvider(this.api, this.endpoint);

  static const defaultPageSize = 20;

  /// Mora odgovarati BaseSearchObject.MaxPageSize na backendu.
  static const maxPageSize = 100;

  final ApiClient api;
  final String endpoint;

  T fromJson(Map<String, dynamic> json);

  Future<PagedResult<T>> get({Map<String, dynamic>? filter, int page = 0, int pageSize = defaultPageSize}) async {
    final json = await api.get(endpoint, query: {...?filter, 'Page': page, 'PageSize': pageSize});
    return PagedResult.fromJson(json as Map<String, dynamic>, fromJson);
  }

  Future<T> getById(int id) async => fromJson(await api.get('$endpoint/$id') as Map<String, dynamic>);

  Future<T> insert(Map<String, dynamic> request) async {
    final rezultat = fromJson(await api.post(endpoint, body: request) as Map<String, dynamic>);
    notifyListeners();
    return rezultat;
  }

  Future<T> update(int id, Map<String, dynamic> request) async {
    final rezultat = fromJson(await api.put('$endpoint/$id', body: request) as Map<String, dynamic>);
    notifyListeners();
    return rezultat;
  }

  Future<void> delete(int id) async {
    await api.delete('$endpoint/$id');
    notifyListeners();
  }
}
