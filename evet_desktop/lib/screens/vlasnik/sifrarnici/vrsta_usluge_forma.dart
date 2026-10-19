import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Forma vrste usluge: naziv, oznake za statistiku (operacija, vakcinacija) i slika kategorije.
Future<bool> otvoriFormuVrsteUsluge(BuildContext context, VrstaUsluge? vrsta) async {
  final rezultat = await showDialog<bool>(context: context, builder: (_) => _VrstaUslugeForma(vrsta: vrsta));
  return rezultat ?? false;
}

class _VrstaUslugeForma extends StatefulWidget {
  const _VrstaUslugeForma({required this.vrsta});

  final VrstaUsluge? vrsta;

  @override
  State<_VrstaUslugeForma> createState() => _VrstaUslugeFormaState();
}

class _VrstaUslugeFormaState extends State<_VrstaUslugeForma> {
  final _formKey = GlobalKey<FormState>();
  late final _naziv = TextEditingController(text: widget.vrsta?.naziv);
  late bool _jeOperacija = widget.vrsta?.jeOperacija ?? false;
  late bool _jeVakcinacija = widget.vrsta?.jeVakcinacija ?? false;
  OdabranaSlika? _novaSlika;
  bool _snima = false;
  String? _greska;

  @override
  void dispose() {
    _naziv.dispose();
    super.dispose();
  }

  Future<void> _odaberiSliku() async {
    try {
      final slika = await odaberiSliku();
      if (slika != null) setState(() => _novaSlika = slika);
    } on ApiException catch (e) {
      setState(() => _greska = e.poruka);
    }
  }

  Future<void> _snimi() async {
    setState(() => _greska = null);
    if (!_formKey.currentState!.validate()) return;

    final request = VrstaUslugeUpsertRequest(naziv: _naziv.text.trim(), jeOperacija: _jeOperacija, jeVakcinacija: _jeVakcinacija).toJson();
    setState(() => _snima = true);
    try {
      final vrste = context.read<VrsteUslugaProvider>();
      final sacuvana = widget.vrsta == null ? await vrste.insert(request) : await vrste.update(widget.vrsta!.vrstaId, request);
      if (_novaSlika case final slika?) {
        if (!mounted) return;
        await context.read<SlikeProvider>().postavi(TipSlike.vrstaUsluge, sacuvana.vrstaId, bytes: slika.bytes, nazivDatoteke: slika.naziv, contentType: slika.contentType);
      }
      if (!mounted) return;
      prikaziUspjeh(context, widget.vrsta == null ? 'Vrsta usluge "${sacuvana.naziv}" je dodana.' : 'Vrsta usluge "${sacuvana.naziv}" je sačuvana.');
      Navigator.pop(context, true);
    } on ApiException catch (e) {
      setState(() => _greska = e.poruka);
    } finally {
      if (mounted) setState(() => _snima = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return FormaDijalog(
      naslov: widget.vrsta == null ? 'Nova vrsta usluge' : 'Uređivanje vrste usluge',
      sirina: 480,
      child: Form(
        key: _formKey,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            TextFormField(controller: _naziv, decoration: const InputDecoration(labelText: 'Naziv'), validator: Validatori.tekst('Naziv', maksimalnaDuzina: 100)),
            const SizedBox(height: 8),
            SwitchListTile(
              contentPadding: EdgeInsets.zero,
              title: const Text('Operacija'),
              subtitle: const Text('Usluge ove vrste se broje kao operacije u statistici.'),
              value: _jeOperacija,
              onChanged: (v) => setState(() => _jeOperacija = v),
            ),
            SwitchListTile(
              contentPadding: EdgeInsets.zero,
              title: const Text('Vakcinacija'),
              subtitle: const Text('Koristi se za filter vakcinisanih pacijenata.'),
              value: _jeVakcinacija,
              onChanged: (v) => setState(() => _jeVakcinacija = v),
            ),
            const SizedBox(height: 8),
            Row(
              children: [
                OutlinedButton.icon(onPressed: _odaberiSliku, icon: const Icon(Icons.image_outlined), label: const Text('Odaberi sliku')),
                const SizedBox(width: 12),
                Expanded(
                  child: Text(
                    _novaSlika?.naziv ?? (widget.vrsta?.imaSliku == true ? 'Slika je postavljena.' : 'Slika nije odabrana (opcionalno).'),
                    overflow: TextOverflow.ellipsis,
                    style: const TextStyle(color: EvetBoje.tekstSivi),
                  ),
                ),
              ],
            ),
            if (_greska != null) ...[const SizedBox(height: 16), PorukaGreske(_greska!)],
            const SizedBox(height: 24),
            FilledButton(onPressed: _snima ? null : _snimi, child: const Text('Sačuvaj')),
          ],
        ),
      ),
    );
  }
}
