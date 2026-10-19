/// Nazivi uloga moraju odgovarati nazivima u seed podacima i [Authorize(Roles = ...)] na backendu.
class Uloge {
  Uloge._();

  static const vlasnik = 'Vlasnik';
  static const veterinar = 'Veterinar';
  static const klijent = 'Klijent';
}
