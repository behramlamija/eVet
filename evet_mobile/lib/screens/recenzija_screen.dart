import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Ocjena usluge i veterinara palcem gore ili dolje (mockup sa slike 10).
class RecenzijaScreen extends StatefulWidget {
  const RecenzijaScreen({super.key, required this.termin});

  final Termini termin;

  @override
  State<RecenzijaScreen> createState() => _RecenzijaScreenState();
}

class _RecenzijaScreenState extends State<RecenzijaScreen> {
  final _komentar = TextEditingController();
  final _formKey = GlobalKey<FormState>();
  bool? _usluga;
  bool? _veterinar;
  bool _salje = false;
  String? _greska;

  @override
  void dispose() {
    _komentar.dispose();
    super.dispose();
  }

  Future<void> _posalji() async {
    if (_usluga == null || _veterinar == null) {
      setState(() => _greska = 'Ocijenite i uslugu i veterinara (palac gore ili dolje).');
      return;
    }
    if (!_formKey.currentState!.validate()) return;

    setState(() {
      _salje = true;
      _greska = null;
    });
    try {
      await context.read<RecenzijeProvider>().ostavi(RecenzijaInsertRequest(
            terminId: widget.termin.terminId,
            uslugaPozitivna: _usluga!,
            veterinarPozitivan: _veterinar!,
            komentar: _komentar.text.trim().isEmpty ? null : _komentar.text.trim(),
          ));
      if (!mounted) return;
      prikaziUspjeh(context, 'Hvala! Vaša recenzija za termin ${widget.termin.sifra} je zabilježena.');
      Navigator.pop(context, true);
    } on ApiException catch (e) {
      setState(() => _greska = e.poruka);
    } finally {
      if (mounted) setState(() => _salje = false);
    }
  }

  Widget _ocjena({required String naslov, required String podnaslov, required IconData ikona, required bool? vrijednost, required ValueChanged<bool> promjena}) {
    Widget palac(bool pozitivno) {
      final odabran = vrijednost == pozitivno;
      return IconButton.filled(
        tooltip: pozitivno ? 'Pozitivno' : 'Negativno',
        iconSize: 32,
        style: IconButton.styleFrom(backgroundColor: odabran ? Colors.white : Colors.white.withValues(alpha: 0.35)),
        onPressed: () => promjena(pozitivno),
        icon: Icon(pozitivno ? Icons.thumb_up_alt : Icons.thumb_down_alt, color: odabran ? (pozitivno ? Colors.green : EvetBoje.crvena) : Colors.black54),
      );
    }

    return Column(
      children: [
        Text(naslov, style: const TextStyle(color: Colors.white70, fontSize: 22)),
        Text(podnaslov, style: const TextStyle(color: Colors.white, fontSize: 24, fontWeight: FontWeight.bold)),
        const SizedBox(height: 16),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            palac(false),
            CircleAvatar(radius: 44, backgroundColor: Colors.white, child: Icon(ikona, size: 44, color: EvetBoje.tamnoplava)),
            palac(true),
          ],
        ),
      ],
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: EvetBoje.crvena,
      appBar: AppBar(backgroundColor: EvetBoje.crvena, foregroundColor: Colors.white, title: const Text('Recenzija')),
      body: Form(
        key: _formKey,
        autovalidateMode: AutovalidateMode.onUserInteraction,
        child: ListView(
          padding: const EdgeInsets.all(24),
          children: [
            _ocjena(naslov: 'Ocijenite naše', podnaslov: 'usluge!', ikona: Icons.medical_services_outlined, vrijednost: _usluga, promjena: (v) => setState(() => _usluga = v)),
            const SizedBox(height: 32),
            _ocjena(
              naslov: 'Ocijenite našeg',
              podnaslov: 'veterinara!',
              ikona: Icons.person,
              vrijednost: _veterinar,
              promjena: (v) => setState(() => _veterinar = v),
            ),
            const SizedBox(height: 8),
            Text('Dr. ${widget.termin.veterinarIme} ${widget.termin.veterinarPrezime}', textAlign: TextAlign.center, style: const TextStyle(color: Colors.white)),
            const SizedBox(height: 24),
            TextFormField(
              controller: _komentar,
              maxLines: 3,
              decoration: const InputDecoration(labelText: 'Komentar (opcionalno)'),
              validator: Validatori.tekst('Komentar', obavezan: false, maksimalnaDuzina: 1000),
            ),
            if (_greska != null) ...[const SizedBox(height: 16), Container(color: Colors.white, child: PorukaGreske(_greska!))],
            const SizedBox(height: 24),
            FilledButton(
              style: FilledButton.styleFrom(backgroundColor: Colors.white, foregroundColor: EvetBoje.crvena),
              onPressed: _salje ? null : _posalji,
              child: const Text('Pošalji recenziju'),
            ),
          ],
        ),
      ),
    );
  }
}
