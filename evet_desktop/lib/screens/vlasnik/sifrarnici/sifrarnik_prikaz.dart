import 'dart:async';

import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';

/// Opis jednog šifrarnika: kako se čita, prikazuje i koje su akcije dozvoljene.
class SifrarnikKonfiguracija<T> {
  const SifrarnikKonfiguracija({
    required this.naziv,
    required this.provider,
    required this.id,
    required this.naslov,
    required this.otvoriFormu,
    required this.opisZapisa,
    this.podnaslov,
    this.slika,
    this.razlogBezDodavanja,
    this.razlogBezBrisanja,
  });

  final String naziv;
  final BaseProvider<T> provider;
  final int Function(T) id;
  final String Function(T) naslov;
  final String? Function(T)? podnaslov;
  final Widget Function(T)? slika;

  /// Forma za dodavanje (zapis je null) ili uređivanje; vraća true ako je nešto sačuvano.
  final Future<bool> Function(BuildContext context, T? zapis) otvoriFormu;

  /// Npr. "grad Mostar" - koristi se u porukama potvrde i uspjeha.
  final String Function(T) opisZapisa;

  /// Ako je postavljen, dodavanje nije dozvoljeno i ovo je objašnjenje za korisnika.
  final String? razlogBezDodavanja;

  /// Ako je postavljen, brisanje nije dozvoljeno i ovo je objašnjenje za korisnika.
  final String? razlogBezBrisanja;
}

/// Lista jednog šifrarnika s pretragom, paginacijom i CRUD akcijama.
class SifrarnikPrikaz<T> extends StatefulWidget {
  const SifrarnikPrikaz({super.key, required this.konfiguracija});

  final SifrarnikKonfiguracija<T> konfiguracija;

  @override
  State<SifrarnikPrikaz<T>> createState() => _SifrarnikPrikazState<T>();
}

class _SifrarnikPrikazState<T> extends State<SifrarnikPrikaz<T>> {
  static const _velicinaStranice = 10;

  final _pretraga = TextEditingController();
  Timer? _debounce;
  int _stranica = 0;
  late Future<PagedResult<T>> _zapisi;

  SifrarnikKonfiguracija<T> get _k => widget.konfiguracija;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  @override
  void dispose() {
    _debounce?.cancel();
    _pretraga.dispose();
    super.dispose();
  }

  void _ucitaj() => _zapisi = _k.provider.get(page: _stranica, pageSize: _velicinaStranice, filter: {'Naziv': _pretraga.text.trim()});

  void _osvjezi({bool naPocetak = false}) => setState(() {
        if (naPocetak) _stranica = 0;
        _ucitaj();
      });

  Future<void> _forma([T? zapis]) async {
    if (await _k.otvoriFormu(context, zapis)) _osvjezi(naPocetak: zapis == null);
  }

  Future<void> _obrisi(T zapis) async {
    final opis = _k.opisZapisa(zapis);
    final potvrda = await potvrdiAkciju(context, naslov: 'Brisanje', poruka: 'Obrisati $opis?', potvrda: 'Obriši', opasna: true);
    if (!potvrda || !mounted) return;
    if (await izvrsiAkciju(context, () => _k.provider.delete(_k.id(zapis)), uspjeh: 'Zapis ($opis) je obrisan.')) _osvjezi();
  }

  @override
  Widget build(BuildContext context) {
    final mozeDodati = _k.razlogBezDodavanja == null;
    final mozeBrisati = _k.razlogBezBrisanja == null;

    return Padding(
      padding: const EdgeInsets.all(24),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            children: [
              Expanded(
                child: TextField(
                  controller: _pretraga,
                  decoration: const InputDecoration(hintText: 'Pretraga po nazivu', prefixIcon: Icon(Icons.search), isDense: true),
                  onChanged: (_) {
                    _debounce?.cancel();
                    _debounce = Timer(const Duration(milliseconds: 400), () => _osvjezi(naPocetak: true));
                  },
                ),
              ),
              const SizedBox(width: 16),
              Tooltip(
                message: _k.razlogBezDodavanja ?? '',
                child: FilledButton.icon(onPressed: mozeDodati ? () => _forma() : null, icon: const Icon(Icons.add), label: const Text('Dodaj')),
              ),
            ],
          ),
          if (!mozeDodati || !mozeBrisati) ...[
            const SizedBox(height: 12),
            Row(
              children: [
                const Icon(Icons.info_outline, size: 18, color: EvetBoje.tekstSivi),
                const SizedBox(width: 8),
                Expanded(child: Text(_k.razlogBezDodavanja ?? _k.razlogBezBrisanja!, style: const TextStyle(color: EvetBoje.tekstSivi))),
              ],
            ),
          ],
          const SizedBox(height: 12),
          Expanded(
            child: FutureBuilder<PagedResult<T>>(
              future: _zapisi,
              builder: (context, snapshot) {
                if (snapshot.hasError) return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: _osvjezi);
                if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
                final rezultat = snapshot.data!;
                return Column(
                  children: [
                    Expanded(
                      child: Card(
                        child: rezultat.resultList.isEmpty
                            ? const Center(child: Text('Nema zapisa za zadanu pretragu.'))
                            : ListView.separated(
                                itemCount: rezultat.resultList.length,
                                separatorBuilder: (_, _) => const Divider(height: 1),
                                itemBuilder: (_, i) {
                                  final z = rezultat.resultList[i];
                                  final podnaslov = _k.podnaslov?.call(z);
                                  return ListTile(
                                    leading: _k.slika?.call(z),
                                    title: Text(_k.naslov(z)),
                                    subtitle: podnaslov == null ? null : Text(podnaslov),
                                    trailing: Row(
                                      mainAxisSize: MainAxisSize.min,
                                      children: [
                                        IconButton(tooltip: 'Uredi', onPressed: () => _forma(z), icon: const Icon(Icons.edit_outlined)),
                                        Tooltip(
                                          message: _k.razlogBezBrisanja ?? 'Obriši',
                                          child: IconButton(onPressed: mozeBrisati ? () => _obrisi(z) : null, icon: const Icon(Icons.delete_outline)),
                                        ),
                                      ],
                                    ),
                                  );
                                },
                              ),
                      ),
                    ),
                    Paginacija(
                      stranica: _stranica,
                      ukupno: rezultat.count,
                      velicinaStranice: _velicinaStranice,
                      onPromjena: (s) => setState(() {
                        _stranica = s;
                        _ucitaj();
                      }),
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
