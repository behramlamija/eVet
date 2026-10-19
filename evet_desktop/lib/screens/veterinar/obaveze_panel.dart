import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Podsjetnik veterinara: obaveze za odabrani dan, s dodavanjem, označavanjem i brisanjem.
class ObavezePanel extends StatefulWidget {
  const ObavezePanel({super.key, required this.datum});

  final DateTime datum;

  @override
  State<ObavezePanel> createState() => _ObavezePanelState();
}

class _ObavezePanelState extends State<ObavezePanel> {
  late Future<PagedResult<Obaveze>> _obaveze;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  @override
  void didUpdateWidget(ObavezePanel oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (oldWidget.datum != widget.datum) _ucitaj();
  }

  void _ucitaj() {
    _obaveze = context.read<ObavezeProvider>().get(filter: {'Datum': widget.datum}, pageSize: BaseProvider.maxPageSize);
  }

  Future<void> _promijeniStatus(Obaveze o, bool zavrsena) async {
    final ok = await izvrsiAkciju(
      context,
      () => context.read<ObavezeProvider>().update(o.obavezaId, ObavezeUpsertRequest(naslov: o.naslov, opis: o.opis, datumVrijeme: o.datumVrijeme, jeZavrsena: zavrsena).toJson()),
      uspjeh: zavrsena ? 'Obaveza "${o.naslov}" je označena kao završena.' : 'Obaveza "${o.naslov}" je ponovo otvorena.',
    );
    if (ok) setState(_ucitaj);
  }

  Future<void> _obrisi(Obaveze o) async {
    final potvrda = await potvrdiAkciju(context, naslov: 'Brisanje obaveze', poruka: 'Obrisati obavezu "${o.naslov}"?', potvrda: 'Obriši', opasna: true);
    if (!potvrda || !mounted) return;
    if (await izvrsiAkciju(context, () => context.read<ObavezeProvider>().delete(o.obavezaId), uspjeh: 'Obaveza "${o.naslov}" je obrisana.')) {
      setState(_ucitaj);
    }
  }

  Future<void> _dodaj() async {
    final dodano = await showDialog<bool>(context: context, builder: (_) => _ObavezaForma(datum: widget.datum));
    if (dodano == true) setState(_ucitaj);
  }

  @override
  Widget build(BuildContext context) {
    return Card(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          ListTile(
            title: const Text('Nadolazeće obaveze', style: TextStyle(fontWeight: FontWeight.w600)),
            trailing: IconButton(tooltip: 'Dodaj obavezu', onPressed: _dodaj, icon: const Icon(Icons.add)),
          ),
          const Divider(height: 1),
          Expanded(
            child: FutureBuilder<PagedResult<Obaveze>>(
              future: _obaveze,
              builder: (context, snapshot) {
                if (snapshot.hasError) return Padding(padding: const EdgeInsets.all(8), child: PorukaGreske(snapshot.error.toString()));
                if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
                final obaveze = [...snapshot.data!.resultList]..sort((a, b) => a.datumVrijeme.compareTo(b.datumVrijeme));
                if (obaveze.isEmpty) return const Center(child: Text('Nema obaveza za ovaj dan.'));
                return ListView(
                  children: [
                    for (final o in obaveze)
                      CheckboxListTile(
                        value: o.jeZavrsena,
                        controlAffinity: ListTileControlAffinity.leading,
                        onChanged: (v) => _promijeniStatus(o, v ?? false),
                        title: Text(o.naslov, style: TextStyle(decoration: o.jeZavrsena ? TextDecoration.lineThrough : null)),
                        subtitle: Text([formatVrijeme(TimeOfDay.fromDateTime(o.datumVrijeme).format24()), if (o.opis != null) o.opis!].join(' • ')),
                        secondary: IconButton(tooltip: 'Obriši obavezu', onPressed: () => _obrisi(o), icon: const Icon(Icons.delete_outline)),
                      ),
                  ],
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}

extension on TimeOfDay {
  String format24() => '${hour.toString().padLeft(2, '0')}:${minute.toString().padLeft(2, '0')}';
}

class _ObavezaForma extends StatefulWidget {
  const _ObavezaForma({required this.datum});

  final DateTime datum;

  @override
  State<_ObavezaForma> createState() => _ObavezaFormaState();
}

class _ObavezaFormaState extends State<_ObavezaForma> {
  final _formKey = GlobalKey<FormState>();
  final _naslov = TextEditingController();
  final _opis = TextEditingController();
  late DateTime _datum = widget.datum;
  TimeOfDay? _vrijeme;
  bool _snima = false;
  String? _greska;

  @override
  void dispose() {
    _naslov.dispose();
    _opis.dispose();
    super.dispose();
  }

  Future<void> _snimi() async {
    if (!_formKey.currentState!.validate()) return;
    setState(() {
      _snima = true;
      _greska = null;
    });
    try {
      await context.read<ObavezeProvider>().insert(ObavezeUpsertRequest(
            naslov: _naslov.text.trim(),
            opis: _opis.text.trim().isEmpty ? null : _opis.text.trim(),
            datumVrijeme: DateTime(_datum.year, _datum.month, _datum.day, _vrijeme!.hour, _vrijeme!.minute),
          ).toJson());
      if (!mounted) return;
      prikaziUspjeh(context, 'Obaveza "${_naslov.text.trim()}" je dodana u podsjetnik.');
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
      naslov: 'Nova obaveza',
      sirina: 460,
      child: Form(
        key: _formKey,
        autovalidateMode: AutovalidateMode.onUserInteraction,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            TextFormField(controller: _naslov, decoration: const InputDecoration(labelText: 'Naslov'), validator: Validatori.tekst('Naslov')),
            const SizedBox(height: 16),
            TextFormField(
              controller: _opis,
              maxLines: 2,
              decoration: const InputDecoration(labelText: 'Opis (opcionalno)'),
              validator: Validatori.tekst('Opis', obavezan: false, maksimalnaDuzina: 1000),
            ),
            const SizedBox(height: 16),
            Row(
              children: [
                Expanded(
                  child: OutlinedButton.icon(
                    icon: const Icon(Icons.calendar_today_outlined),
                    label: Text(formatDatum(_datum)),
                    onPressed: () async {
                      final d = await showDatePicker(context: context, initialDate: _datum, firstDate: DateTime(2020), lastDate: DateTime.now().add(const Duration(days: 365)));
                      if (d != null) setState(() => _datum = d);
                    },
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: FormField<TimeOfDay>(
                    validator: (_) => _vrijeme == null ? 'Odaberite vrijeme obaveze.' : null,
                    builder: (state) => Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        OutlinedButton.icon(
                          icon: const Icon(Icons.schedule),
                          label: Text(_vrijeme?.format24() ?? 'Odaberite vrijeme'),
                          onPressed: () async {
                            final v = await showTimePicker(context: context, initialTime: _vrijeme ?? const TimeOfDay(hour: 9, minute: 0));
                            if (v != null) {
                              setState(() => _vrijeme = v);
                              state.didChange(v);
                            }
                          },
                        ),
                        if (state.errorText != null)
                          Padding(
                            padding: const EdgeInsets.only(top: 4, left: 12),
                            child: Text(state.errorText!, style: TextStyle(color: Theme.of(context).colorScheme.error, fontSize: 12)),
                          ),
                      ],
                    ),
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
