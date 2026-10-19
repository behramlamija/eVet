import 'dart:async';

import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'usluga_detalji_screen.dart';

/// Početni ekran: pozdrav, pretraga, kategorije usluga i najnovije usluge (mockup sa slike 8).
class PocetnaScreen extends StatefulWidget {
  const PocetnaScreen({super.key});

  @override
  State<PocetnaScreen> createState() => _PocetnaScreenState();
}

class _PocetnaScreenState extends State<PocetnaScreen> {
  final _pretraga = TextEditingController();
  Timer? _debounce;
  int? _vrstaId;
  late Future<PagedResult<VrstaUsluge>> _vrste;
  late Future<PagedResult<Usluge>> _usluge;

  @override
  void initState() {
    super.initState();
    _vrste = context.read<VrsteUslugaProvider>().get(pageSize: BaseProvider.maxPageSize);
    _ucitajUsluge();
  }

  @override
  void dispose() {
    _debounce?.cancel();
    _pretraga.dispose();
    super.dispose();
  }

  void _ucitajUsluge() {
    _usluge = context.read<UslugeProvider>().get(filter: {'FTS': _pretraga.text.trim(), 'VrstaId': _vrstaId});
  }

  void _pretrazi(String _) {
    _debounce?.cancel();
    _debounce = Timer(const Duration(milliseconds: 400), () => setState(_ucitajUsluge));
  }

  @override
  Widget build(BuildContext context) {
    final korisnik = context.watch<AuthProvider>().korisnik!;
    final slike = context.read<SlikeProvider>();

    return RefreshIndicator(
      onRefresh: () async {
        setState(_ucitajUsluge);
        await _usluge;
      },
      child: ListView(
        padding: const EdgeInsets.all(16),
        children: [
          Row(
            children: [
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text('Pozdrav ${korisnik.ime}!', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
                    const Text('Zakoračite u PetCity!', style: TextStyle(color: EvetBoje.tekstSivi)),
                  ],
                ),
              ),
              EvetSlika(slike: slike, tip: TipSlike.korisnik, id: korisnik.korisnikId, imaSliku: korisnik.imaSliku, zamjenskaIkona: Icons.person),
            ],
          ),
          const SizedBox(height: 16),
          TextField(
            controller: _pretraga,
            onChanged: _pretrazi,
            decoration: const InputDecoration(hintText: 'Pretražite naše usluge', prefixIcon: Icon(Icons.search)),
          ),
          const SizedBox(height: 16),
          SizedBox(
            height: 48,
            child: FutureBuilder<PagedResult<VrstaUsluge>>(
              future: _vrste,
              builder: (context, snapshot) {
                final vrste = snapshot.data?.resultList ?? const <VrstaUsluge>[];
                return ListView(
                  scrollDirection: Axis.horizontal,
                  children: [
                    for (final vrsta in [null, ...vrste])
                      Padding(
                        padding: const EdgeInsets.only(right: 8),
                        child: ChoiceChip(
                          label: Text(vrsta?.naziv ?? 'Sve'),
                          selected: _vrstaId == vrsta?.vrstaId,
                          onSelected: (_) => setState(() {
                            _vrstaId = vrsta?.vrstaId;
                            _ucitajUsluge();
                          }),
                        ),
                      ),
                  ],
                );
              },
            ),
          ),
          const SizedBox(height: 16),
          Text('Najnovije', style: Theme.of(context).textTheme.titleLarge?.copyWith(fontWeight: FontWeight.bold)),
          const SizedBox(height: 8),
          FutureBuilder<PagedResult<Usluge>>(
            future: _usluge,
            builder: (context, snapshot) {
              if (snapshot.hasError) {
                return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitajUsluge));
              }
              if (!snapshot.hasData) {
                return const Padding(padding: EdgeInsets.all(24), child: Center(child: CircularProgressIndicator()));
              }
              final usluge = snapshot.data!.resultList;
              if (usluge.isEmpty) {
                return const Padding(padding: EdgeInsets.all(24), child: Center(child: Text('Nema usluga za zadanu pretragu.')));
              }
              return Column(
                children: [
                  for (final usluga in usluge)
                    Card(
                      child: ListTile(
                        onTap: () => Navigator.of(context).push(MaterialPageRoute(builder: (_) => UslugaDetaljiScreen(usluga: usluga))),
                        leading: EvetSlika(slike: slike, tip: TipSlike.usluga, id: usluga.uslugaId, imaSliku: usluga.imaSliku, zamjenskaIkona: Icons.medical_services_outlined),
                        title: Text(usluga.naziv, style: const TextStyle(fontWeight: FontWeight.w600)),
                        subtitle: Text(usluga.opis ?? usluga.vrstaNaziv, maxLines: 2, overflow: TextOverflow.ellipsis),
                        trailing: Text(formatNovac(usluga.cijena), style: const TextStyle(fontWeight: FontWeight.bold)),
                      ),
                    ),
                ],
              );
            },
          ),
        ],
      ),
    );
  }
}
