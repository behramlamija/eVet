import 'package:file_picker/file_picker.dart';

import '../api/api_exception.dart';

class OdabranaSlika {
  const OdabranaSlika(this.bytes, this.naziv, this.contentType);

  final List<int> bytes;
  final String naziv;
  final String contentType;
}

const _maksimalnaVelicina = 2 * 1024 * 1024;
const _tipovi = {'jpg': 'image/jpeg', 'jpeg': 'image/jpeg', 'png': 'image/png', 'webp': 'image/webp'};

/// Otvara izbor slike (JPG, PNG, WEBP do 2 MB). Vraća null ako korisnik odustane.
/// Server dodatno provjerava stvarni sadržaj datoteke (magic bytes).
Future<OdabranaSlika?> odaberiSliku() async {
  final datoteka = await FilePicker.pickFile(type: FileType.custom, allowedExtensions: _tipovi.keys.toList());
  if (datoteka == null) return null;

  final tip = _tipovi[datoteka.extension?.toLowerCase()];
  if (tip == null) {
    throw const ApiException('Dozvoljeni formati slike su JPG, PNG i WEBP.');
  }
  if ((await datoteka.length() ?? 0) > _maksimalnaVelicina) {
    throw const ApiException('Slika može biti velika najviše 2 MB.');
  }
  final bytes = await datoteka.readAsBytes();
  return OdabranaSlika(bytes, datoteka.name, tip);
}
