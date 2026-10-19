import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:webview_flutter/webview_flutter.dart';

/// PayPal odobravanje plaćanja unutar aplikacije. Povratak na returnUrl ili cancelUrl zatvara ekran
/// (true = kupac je odobrio plaćanje, false = odustao). Naplatu nakon toga potvrđuje server.
class PayPalScreen extends StatefulWidget {
  const PayPalScreen({super.key, required this.narudzba});

  final PayPalNarudzba narudzba;

  @override
  State<PayPalScreen> createState() => _PayPalScreenState();
}

class _PayPalScreenState extends State<PayPalScreen> {
  late final WebViewController _controller;
  int _napredak = 0;

  @override
  void initState() {
    super.initState();
    _controller = WebViewController()
      ..setJavaScriptMode(JavaScriptMode.unrestricted)
      ..setNavigationDelegate(NavigationDelegate(
        onProgress: (p) => setState(() => _napredak = p),
        onNavigationRequest: (zahtjev) {
          if (zahtjev.url.startsWith(widget.narudzba.returnUrl)) {
            Navigator.pop(context, true);
            return NavigationDecision.prevent;
          }
          if (zahtjev.url.startsWith(widget.narudzba.cancelUrl)) {
            Navigator.pop(context, false);
            return NavigationDecision.prevent;
          }
          return NavigationDecision.navigate;
        },
      ));
    _ucitaj();
  }

  // Prethodna PayPal prijava se briše da bi kupac svaki put sam izabrao račun (i na dijeljenom uređaju).
  Future<void> _ucitaj() async {
    await WebViewCookieManager().clearCookies();
    await _controller.clearCache();
    await _controller.clearLocalStorage();
    if (mounted) await _controller.loadRequest(Uri.parse(widget.narudzba.approveUrl));
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('PayPal: ${widget.narudzba.iznos.toStringAsFixed(2)} ${widget.narudzba.valuta}'),
        leading: IconButton(icon: const Icon(Icons.close), tooltip: 'Odustani', onPressed: () => Navigator.pop(context, false)),
        bottom: _napredak < 100 ? PreferredSize(preferredSize: const Size.fromHeight(3), child: LinearProgressIndicator(value: _napredak / 100)) : null,
      ),
      body: WebViewWidget(controller: _controller),
    );
  }
}

/// Cijeli tok plaćanja: otvaranje PayPal narudžbe, odobravanje u aplikaciji i potvrda naplate na serveru.
/// Vraća ažurirani termin ako je plaćanje uspjelo, inače null (greška je već prikazana).
Future<Termini?> platiPayPalom(BuildContext context, Termini termin) async {
  final placanja = context.read<PlacanjaProvider>();
  try {
    final narudzba = await placanja.zapocniPayPal(termin.terminId);
    if (!context.mounted) return null;

    final odobreno = await Navigator.of(context).push<bool>(MaterialPageRoute(builder: (_) => PayPalScreen(narudzba: narudzba)));
    if (odobreno != true || !context.mounted) {
      if (context.mounted) prikaziGresku(context, 'Plaćanje nije završeno. Termin ${termin.sifra} možete platiti kasnije u detaljima termina.');
      return null;
    }

    final placen = await placanja.potvrdiPayPal(narudzba.uplataId);
    if (context.mounted) prikaziUspjeh(context, 'Termin ${placen.sifra} je plaćen.');
    return placen;
  } on ApiException catch (e) {
    if (context.mounted) prikaziGresku(context, e.poruka);
    return null;
  }
}
