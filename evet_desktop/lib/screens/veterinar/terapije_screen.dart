import 'dart:async';

import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'terapije_tabela.dart';

/// Pregled svih terapija s pretragom po dijagnozi, lijeku ili pacijentu.
/// Nova terapija se propisuje iz kartona pacijenta.
class TerapijeScreen extends StatefulWidget {
  const TerapijeScreen({super.key});

  @override
  State<TerapijeScreen> createState() => _TerapijeScreenState();
}

class _TerapijeScreenState extends State<TerapijeScreen> {
  static const _velicinaStranice = 15;

  final _pretraga = TextEditingController();
  Timer? _debounce;
  int _stranica = 0;
  late Future<PagedResult<Terapije>> _terapije;

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

  void _ucitaj() {
    _terapije = context.read<TerapijeProvider>().get(page: _stranica, pageSize: _velicinaStranice, filter: {'FTS': _pretraga.text.trim()});
  }

  @override
  Widget build(BuildContext context) {
    return ListView(
      padding: const EdgeInsets.all(32),
      children: [
        Text('TERAPIJE', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
        const SizedBox(height: 4),
        const Text('Nova terapija se dodaje iz kartona pacijenta (Pacijenti → odabir pacijenta).', style: TextStyle(color: EvetBoje.tekstSivi)),
        const SizedBox(height: 12),
        TextField(
          controller: _pretraga,
          decoration: const InputDecoration(hintText: 'Pretražite po dijagnozi, lijeku ili imenu pacijenta', prefixIcon: Icon(Icons.search)),
          onChanged: (_) {
            _debounce?.cancel();
            _debounce = Timer(const Duration(milliseconds: 400), () => setState(() {
                  _stranica = 0;
                  _ucitaj();
                }));
          },
        ),
        const SizedBox(height: 12),
        FutureBuilder<PagedResult<Terapije>>(
          future: _terapije,
          builder: (context, snapshot) {
            if (snapshot.hasError) return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj));
            if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
            final rezultat = snapshot.data!;
            return Column(
              children: [
                TerapijeTabela(terapije: rezultat.resultList, onPromjena: () => setState(_ucitaj)),
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
      ],
    );
  }
}
