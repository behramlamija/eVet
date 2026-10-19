/// Odgovor list endpointa: ukupan broj zapisa i zapisi tražene stranice.
class PagedResult<T> {
  final int count;
  final List<T> resultList;

  const PagedResult({required this.count, required this.resultList});

  factory PagedResult.fromJson(Map<String, dynamic> json, T Function(Map<String, dynamic>) fromJsonT) {
    return PagedResult(
      count: json['count'] as int,
      resultList: (json['resultList'] as List).map((e) => fromJsonT(e as Map<String, dynamic>)).toList(),
    );
  }
}
