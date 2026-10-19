import 'dart:io';
import 'dart:typed_data';

import 'package:evet_shared/evet_shared.dart';
import 'package:pdf/pdf.dart';
import 'package:pdf/widgets.dart' as pw;

/// PDF izvještaji za vlasnika. Koristi se Windows sistemski font (Arial) zbog slova č, ć, đ, š, ž.
class PdfIzvjestaji {
  PdfIzvjestaji._();

  static const _mjeseci = ['Januar', 'Februar', 'Mart', 'April', 'Maj', 'Juni', 'Juli', 'August', 'Septembar', 'Oktobar', 'Novembar', 'Decembar'];
  static const _tamnoplava = PdfColor.fromInt(0xFF0D2C54);

  static Future<pw.ThemeData> _tema() async {
    final fontovi = Platform.environment['WINDIR'] ?? r'C:\Windows';
    final obicni = File('$fontovi\\Fonts\\arial.ttf');
    final podebljani = File('$fontovi\\Fonts\\arialbd.ttf');
    if (await obicni.exists() && await podebljani.exists()) {
      return pw.ThemeData.withFont(
        base: pw.Font.ttf((await obicni.readAsBytes()).buffer.asByteData()),
        bold: pw.Font.ttf((await podebljani.readAsBytes()).buffer.asByteData()),
      );
    }
    return pw.ThemeData();
  }

  static pw.Widget _zaglavlje(String naslov, String podnaslov) {
    return pw.Column(
      crossAxisAlignment: pw.CrossAxisAlignment.start,
      children: [
        pw.Text('PetCity veterinarska stanica', style: pw.TextStyle(fontSize: 11, color: PdfColors.grey700)),
        pw.SizedBox(height: 4),
        pw.Text(naslov, style: pw.TextStyle(fontSize: 20, fontWeight: pw.FontWeight.bold, color: _tamnoplava)),
        pw.Text(podnaslov, style: const pw.TextStyle(fontSize: 11)),
        pw.Divider(color: _tamnoplava),
      ],
    );
  }

  static pw.Widget _podnozje(pw.Context context) {
    return pw.Row(
      mainAxisAlignment: pw.MainAxisAlignment.spaceBetween,
      children: [
        pw.Text('Generisano: ${formatDatumVrijeme(DateTime.now())}', style: const pw.TextStyle(fontSize: 9, color: PdfColors.grey700)),
        pw.Text('Stranica ${context.pageNumber} od ${context.pagesCount}', style: const pw.TextStyle(fontSize: 9, color: PdfColors.grey700)),
      ],
    );
  }

  static pw.Widget _tabela(List<String> zaglavlja, List<List<String>> redovi) {
    return pw.TableHelper.fromTextArray(
      headers: zaglavlja,
      data: redovi,
      headerStyle: pw.TextStyle(fontWeight: pw.FontWeight.bold, color: PdfColors.white),
      headerDecoration: const pw.BoxDecoration(color: _tamnoplava),
      cellAlignment: pw.Alignment.centerLeft,
      oddRowDecoration: const pw.BoxDecoration(color: PdfColors.grey100),
      cellPadding: const pw.EdgeInsets.symmetric(horizontal: 6, vertical: 5),
    );
  }

  /// Izvještaj 1: pokazatelji poslovanja za godinu i pregled po mjesecima.
  static Future<Uint8List> poslovanje({required int godina, required StatistikaPoslovanja s, required List<StatistikaPeriod> poMjesecima}) async {
    final dokument = pw.Document(theme: await _tema(), title: 'Izvještaj o poslovanju $godina');
    final mjeseci = {for (final p in poMjesecima) p.mjesec: p};

    dokument.addPage(pw.MultiPage(
      pageFormat: PdfPageFormat.a4,
      margin: const pw.EdgeInsets.all(36),
      footer: _podnozje,
      build: (context) => [
        _zaglavlje('Izvještaj o poslovanju za $godina. godinu', 'Period: 01.01.$godina. - 31.12.$godina.'),
        pw.SizedBox(height: 12),
        pw.Text('Ključni pokazatelji', style: pw.TextStyle(fontSize: 14, fontWeight: pw.FontWeight.bold)),
        pw.SizedBox(height: 8),
        _tabela(['Pokazatelj', 'Vrijednost'], [
          ['Ukupan broj termina', '${s.brojTermina}'],
          ['Završeni termini', '${s.brojZavrsenihTermina}'],
          ['Otkazani i odbijeni termini', '${s.brojOtkazanihTermina}'],
          ['Pružene usluge', '${s.brojPruzenihUsluga}'],
          ['Broj operacija', '${s.brojOperacija}'],
          ['Novi klijenti', '${s.brojNovihKlijenata}'],
          ['Ukupni prihod', formatNovac(s.ukupniPrihod)],
          ['Najčešća usluga', s.najcescaUsluga ?? '-'],
          ['Najzastupljenija rasa', s.najzastupljenijaRasa ?? '-'],
          ['Veterinar s najviše završenih termina', s.topVeterinar ?? '-'],
        ]),
        pw.SizedBox(height: 20),
        pw.Text('Pregled po mjesecima', style: pw.TextStyle(fontSize: 14, fontWeight: pw.FontWeight.bold)),
        pw.SizedBox(height: 8),
        _tabela(['Mjesec', 'Termini', 'Operacije', 'Prihod'], [
          for (var m = 1; m <= 12; m++)
            [_mjeseci[m - 1], '${mjeseci[m]?.brojTermina ?? 0}', '${mjeseci[m]?.brojOperacija ?? 0}', formatNovac(mjeseci[m]?.prihod ?? 0)],
          [
            'Ukupno',
            '${poMjesecima.fold<int>(0, (z, p) => z + p.brojTermina)}',
            '${poMjesecima.fold<int>(0, (z, p) => z + p.brojOperacija)}',
            formatNovac(poMjesecima.fold<double>(0, (z, p) => z + p.prihod)),
          ],
        ]),
      ],
    ));

    return dokument.save();
  }

  /// Izvještaj 2: učinak veterinara (termini i recenzije klijenata).
  static Future<Uint8List> veterinari(List<StatistikaVeterinara> veterinari) async {
    final dokument = pw.Document(theme: await _tema(), title: 'Izvještaj o veterinarima');

    String procenat(StatistikaVeterinara v) {
      final ukupno = v.pozitivneRecenzije + v.negativneRecenzije;
      return ukupno == 0 ? '-' : '${(v.pozitivneRecenzije * 100 / ukupno).round()}%';
    }

    dokument.addPage(pw.MultiPage(
      pageFormat: PdfPageFormat.a4.landscape,
      margin: const pw.EdgeInsets.all(36),
      footer: _podnozje,
      build: (context) => [
        _zaglavlje('Izvještaj o učinku veterinara', 'Broj veterinara: ${veterinari.length}'),
        pw.SizedBox(height: 12),
        _tabela(
          ['R. br.', 'Veterinar', 'Specijalnost', 'Status', 'Termini', 'Završeni', 'Pozitivne recenzije', 'Negativne recenzije', '% pozitivnih'],
          [
            for (final (i, v) in veterinari.indexed)
              [
                '${i + 1}',
                '${v.ime} ${v.prezime}',
                v.specijalnostNaziv ?? '-',
                v.jeAktivan ? 'Aktivan' : 'Neaktivan',
                '${v.brojTermina}',
                '${v.brojZavrsenihTermina}',
                '${v.pozitivneRecenzije}',
                '${v.negativneRecenzije}',
                procenat(v),
              ],
          ],
        ),
      ],
    ));

    return dokument.save();
  }
}
