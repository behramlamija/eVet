import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../providers/korpa_provider.dart';
import '../widgets/preporuke_sekcija.dart';
import 'zakazivanje_screen.dart';

/// Korpa za rezervaciju (mockup sa slike 9): usluge, kupon, prevoz i ukupni troškovi.
class KorpaScreen extends StatefulWidget {
  const KorpaScreen({super.key});

  @override
  State<KorpaScreen> createState() => _KorpaScreenState();
}

class _KorpaScreenState extends State<KorpaScreen> {
  final _formKey = GlobalKey<FormState>();
  late final _adresa = TextEditingController(text: context.read<KorpaProvider>().adresaPrevoza);
  late final Future<TerminiKonfiguracija> _konfiguracija = context.read<TerminiProvider>().konfiguracija();
  late final Future<PagedResult<Grad>> _gradovi = context.read<GradoviProvider>().get(pageSize: BaseProvider.maxPageSize);

  @override
  void dispose() {
    _adresa.dispose();
    super.dispose();
  }

  Future<void> _odaberiKupon() async {
    final korpa = context.read<KorpaProvider>();
    final PagedResult<AktiviranePromocije> kuponi;
    try {
      kuponi = await context.read<PromocijeProvider>().moje(pageSize: BaseProvider.maxPageSize);
    } on ApiException catch (e) {
      if (mounted) prikaziGresku(context, e.poruka);
      return;
    }
    if (!mounted) return;

    final primjenjivi = kuponi.resultList.where((k) => !k.iskoristena && korpa.sadrzi(k.uslugaId)).toList();
    final odabran = await showModalBottomSheet<AktiviranePromocije?>(
      context: context,
      builder: (context) => SafeArea(
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              Text('Moji kuponi', style: Theme.of(context).textTheme.titleLarge),
              const SizedBox(height: 8),
              if (primjenjivi.isEmpty)
                const Padding(
                  padding: EdgeInsets.symmetric(vertical: 16),
                  child: Text('Nemate aktiviranih kupona za usluge u korpi. Promocije možete aktivirati u Profilu, u sekciji Promocije.'),
                ),
              for (final k in primjenjivi)
                ListTile(
                  leading: CircleAvatar(backgroundColor: EvetBoje.crvena, child: Text('${k.promocijaPopust.round()}%', style: const TextStyle(color: Colors.white, fontSize: 12))),
                  title: Text(k.promocijaNaziv),
                  subtitle: Text('${k.uslugaNaziv} • važi do ${formatDatum(k.promocijaDatumKraja)}'),
                  onTap: () => Navigator.pop(context, k),
                ),
              if (korpa.kupon != null) TextButton(onPressed: () => Navigator.pop(context, null), child: const Text('Ukloni kupon')),
            ],
          ),
        ),
      ),
    );
    if (!mounted) return;
    if (odabran != null || korpa.kupon != null) korpa.postaviKupon(odabran);
  }

  void _nastavi(KorpaProvider korpa) {
    if (korpa.jePrevoz && !_formKey.currentState!.validate()) return;
    korpa.postaviPrevoz(jePrevoz: korpa.jePrevoz, adresa: _adresa.text.trim(), gradId: korpa.gradPrevozaId);
    Navigator.of(context).push(MaterialPageRoute(builder: (_) => const ZakazivanjeScreen()));
  }

  Widget _red(String naziv, String vrijednost, {bool istaknuto = false}) {
    final stil = TextStyle(fontSize: istaknuto ? 20 : 15, fontWeight: istaknuto ? FontWeight.bold : FontWeight.normal);
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4),
      child: Row(children: [Text(naziv, style: stil), const Spacer(), Text(vrijednost, style: stil)]),
    );
  }

  @override
  Widget build(BuildContext context) {
    final korpa = context.watch<KorpaProvider>();
    final slike = context.read<SlikeProvider>();

    if (korpa.jePrazna) {
      return const Center(
        child: Padding(
          padding: EdgeInsets.all(32),
          child: Text('Korpa je prazna. Dodajte usluge sa početnog ekrana.', textAlign: TextAlign.center),
        ),
      );
    }

    return FutureBuilder<TerminiKonfiguracija>(
      future: _konfiguracija,
      builder: (context, snapshot) {
        if (snapshot.hasError) return Padding(padding: const EdgeInsets.all(16), child: PorukaGreske(snapshot.error.toString()));
        if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
        final tarifa = snapshot.data!.cijenaPrevoza;

        return Form(
          key: _formKey,
          autovalidateMode: AutovalidateMode.onUserInteraction,
          child: ListView(
            padding: const EdgeInsets.all(16),
            children: [
              Text('Korpa', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
              const SizedBox(height: 12),
              for (final u in korpa.stavke)
                Card(
                  child: ListTile(
                    leading: EvetSlika(slike: slike, tip: TipSlike.usluga, id: u.uslugaId, imaSliku: u.imaSliku, zamjenskaIkona: Icons.medical_services_outlined),
                    title: Text(u.naziv),
                    subtitle: Text(formatNovac(u.cijena), style: const TextStyle(fontWeight: FontWeight.bold)),
                    trailing: IconButton(
                      tooltip: 'Ukloni iz korpe',
                      icon: const Icon(Icons.close, color: EvetBoje.crvena),
                      onPressed: () => korpa.ukloni(u.uslugaId),
                    ),
                  ),
                ),
              const SizedBox(height: 16),
              PreporukeSekcija(uslugeIds: [for (final u in korpa.stavke) u.uslugaId], sakrijAkoPrazno: true),
              const SizedBox(height: 12),
              FilledButton.icon(
                style: FilledButton.styleFrom(backgroundColor: EvetBoje.crvena),
                onPressed: _odaberiKupon,
                icon: const Icon(Icons.local_offer_outlined),
                label: Text(korpa.kupon == null ? 'Iskoristi kupone' : 'Kupon: ${korpa.kupon!.promocijaKod}'),
              ),
              const SizedBox(height: 12),
              SwitchListTile(
                contentPadding: EdgeInsets.zero,
                title: const Text('Prevoz ljubimca'),
                subtitle: Text('Preuzimanje i vraćanje ljubimca: ${formatNovac(tarifa)}'),
                value: korpa.jePrevoz,
                onChanged: (v) => korpa.postaviPrevoz(jePrevoz: v),
              ),
              if (korpa.jePrevoz) ...[
                TextFormField(
                  controller: _adresa,
                  decoration: const InputDecoration(labelText: 'Adresa preuzimanja'),
                  validator: Validatori.tekst('Adresa preuzimanja'),
                ),
                const SizedBox(height: 12),
                FutureBuilder<PagedResult<Grad>>(
                  future: _gradovi,
                  builder: (context, g) => DropdownButtonFormField<int>(
                    initialValue: korpa.gradPrevozaId,
                    decoration: const InputDecoration(labelText: 'Grad'),
                    items: [for (final grad in g.data?.resultList ?? const <Grad>[]) DropdownMenuItem(value: grad.gradId, child: Text(grad.naziv))],
                    onChanged: (v) => korpa.postaviPrevoz(jePrevoz: true, gradId: v),
                    validator: (v) => Validatori.odabir(v, 'Grad'),
                  ),
                ),
              ],
              const Divider(height: 32),
              _red('Troškovi usluga', formatNovac(korpa.iznosUsluga)),
              if (korpa.iznosPopusta > 0) _red('Popust (${korpa.kupon!.promocijaKod})', '-${formatNovac(korpa.iznosPopusta)}'),
              _red('Troškovi prevoza', formatNovac(korpa.cijenaPrevoza(tarifa))),
              const SizedBox(height: 8),
              _red('Ukupno', formatNovac(korpa.ukupno(tarifa)), istaknuto: true),
              const SizedBox(height: 24),
              FilledButton(onPressed: () => _nastavi(korpa), child: const Text('Nastavi na zakazivanje')),
            ],
          ),
        );
      },
    );
  }
}
