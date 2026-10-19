/// Greška koju vraća API. Poruke dolaze sa backenda i prikazuju se korisniku bez izmjene.
class ApiException implements Exception {
  final String poruka;
  final Map<String, List<String>> greske;
  final int? statusCode;

  const ApiException(this.poruka, {this.greske = const {}, this.statusCode});

  /// Validacijska greška za određeno polje (npr. 'Email'), za prikaz ispod kontrole.
  String? greskaZaPolje(String polje) {
    final kljuc = greske.keys.firstWhere(
      (k) => k.toLowerCase() == polje.toLowerCase(),
      orElse: () => '',
    );
    return kljuc.isEmpty ? null : greske[kljuc]!.join('\n');
  }

  @override
  String toString() => poruka;
}
