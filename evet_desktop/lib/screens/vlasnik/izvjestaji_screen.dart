import 'dart:typed_data';

import 'package:evet_shared/evet_shared.dart';
import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:printing/printing.dart';
import 'package:provider/provider.dart';

import '../../izvjestaji/pdf_izvjestaji.dart';

/// PDF izvještaji za preuzimanje i ispis (upute 2.2: minimalno dva izvještaja).
class IzvjestajiScreen extends StatefulWidget {
  const IzvjestajiScreen({super.key});

  @override
  State<IzvjestajiScreen> createState() => _IzvjestajiScreenState();
}

class _IzvjestajiScreenState extends State<IzvjestajiScreen> {
  int _godina = DateTime.now().year;
  bool _generise = false;

  Future<Uint8List> _izvjestajPoslovanja() async {
    final statistika = context.read<StatistikaProvider>();
    final od = DateTime(_godina);
    final doDatuma = DateTime(_godina, 12, 31);
    final poslovanjeZahtjev = statistika.poslovanje(datumOd: od, datumDo: doDatuma);
    final poMjesecimaZahtjev = statistika.poPeriodima(GrupisanjeStatistike.mjesec, godina: _godina);
    return PdfIzvjestaji.poslovanje(godina: _godina, s: await poslovanjeZahtjev, poMjesecima: await poMjesecimaZahtjev);
  }

  Future<Uint8List> _izvjestajVeterinara() async {
    final rezultat = await context.read<StatistikaProvider>().veterinari(pageSize: BaseProvider.maxPageSize);
    return PdfIzvjestaji.veterinari(rezultat.resultList);
  }

  Future<void> _ispis(String naziv, Future<Uint8List> Function() generisi) async {
    await _uz(() async {
      final bytes = await generisi();
      await Printing.layoutPdf(name: naziv, onLayout: (_) async => bytes);
    });
  }

  Future<void> _preuzmi(String nazivDatoteke, Future<Uint8List> Function() generisi) async {
    await _uz(() async {
      final bytes = await generisi();
      final putanja = await FilePicker.saveFile(
        fileName: nazivDatoteke,
        bytes: bytes,
        mimeType: 'application/pdf',
        dialogTitle: 'Sačuvaj izvještaj',
        type: FileType.custom,
        allowedExtensions: const ['pdf'],
      );
      if (putanja != null && mounted) prikaziUspjeh(context, 'Izvještaj je sačuvan: ${putanja.toFilePath()}');
    });
  }

  Future<void> _uz(Future<void> Function() akcija) async {
    setState(() => _generise = true);
    try {
      await akcija();
    } on ApiException catch (e) {
      if (mounted) prikaziGresku(context, e.poruka);
    } finally {
      if (mounted) setState(() => _generise = false);
    }
  }

  Widget _kartica({required IconData ikona, required String naslov, required String opis, Widget? parametri, required String naziv, required String datoteka, required Future<Uint8List> Function() generisi}) {
    return SizedBox(
      width: 440,
      child: Card(
        child: Padding(
          padding: const EdgeInsets.all(20),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(children: [Icon(ikona, color: EvetBoje.crvena), const SizedBox(width: 8), Expanded(child: Text(naslov, style: Theme.of(context).textTheme.titleMedium))]),
              const SizedBox(height: 8),
              Text(opis, style: const TextStyle(color: EvetBoje.tekstSivi)),
              if (parametri != null) ...[const SizedBox(height: 12), parametri],
              const SizedBox(height: 16),
              Row(
                children: [
                  FilledButton.icon(onPressed: _generise ? null : () => _preuzmi(datoteka, generisi), icon: const Icon(Icons.download), label: const Text('Preuzmi PDF')),
                  const SizedBox(width: 12),
                  OutlinedButton.icon(onPressed: _generise ? null : () => _ispis(naziv, generisi), icon: const Icon(Icons.print_outlined), label: const Text('Pregled i ispis')),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final trenutnaGodina = DateTime.now().year;

    return ListView(
      padding: const EdgeInsets.all(32),
      children: [
        Text('IZVJEŠTAJI', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
        const SizedBox(height: 16),
        if (_generise) const Padding(padding: EdgeInsets.only(bottom: 12), child: LinearProgressIndicator()),
        Wrap(
          spacing: 24,
          runSpacing: 24,
          children: [
            _kartica(
              ikona: Icons.query_stats,
              naslov: 'Izvještaj o poslovanju',
              opis: 'Ključni pokazatelji (termini, operacije, prihod, novi klijenti) i pregled po mjesecima za odabranu godinu.',
              parametri: DropdownButtonFormField<int>(
                initialValue: _godina,
                decoration: const InputDecoration(labelText: 'Godina', isDense: true),
                items: [for (var g = trenutnaGodina; g >= trenutnaGodina - 5; g--) DropdownMenuItem(value: g, child: Text('$g'))],
                onChanged: (g) => setState(() => _godina = g ?? _godina),
              ),
              naziv: 'Izvještaj o poslovanju $_godina',
              datoteka: 'izvjestaj-poslovanje-$_godina.pdf',
              generisi: _izvjestajPoslovanja,
            ),
            _kartica(
              ikona: Icons.groups_outlined,
              naslov: 'Izvještaj o učinku veterinara',
              opis: 'Za svakog veterinara: specijalnost, status, broj termina, završeni termini i ocjene klijenata.',
              naziv: 'Izvještaj o veterinarima',
              datoteka: 'izvjestaj-veterinari.pdf',
              generisi: _izvjestajVeterinara,
            ),
          ],
        ),
      ],
    );
  }
}
