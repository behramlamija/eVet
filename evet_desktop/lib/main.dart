import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:provider/provider.dart';

import 'layouts/master_screen.dart';
import 'screens/login_screen.dart';

/// Adresa API-ja: flutter run --dart-define=API_BASE_URL=http://localhost:5103
const apiBaseUrl = String.fromEnvironment('API_BASE_URL', defaultValue: 'http://localhost:5103');

void main() {
  final api = ApiClient(baseUrl: apiBaseUrl);

  runApp(
    MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (_) => AuthProvider(api, dozvoljeneUloge: const [Uloge.vlasnik, Uloge.veterinar])),
        ChangeNotifierProvider(create: (_) => ObavijestiProvider(api)),
        ChangeNotifierProvider(create: (_) => TerminiProvider(api)),
        ChangeNotifierProvider(create: (_) => LjubimciProvider(api)),
        ChangeNotifierProvider(create: (_) => TerapijeProvider(api)),
        ChangeNotifierProvider(create: (_) => ObavezeProvider(api)),
        ChangeNotifierProvider(create: (_) => KorisniciProvider(api)),
        ChangeNotifierProvider(create: (_) => KlijentiProvider(api)),
        ChangeNotifierProvider(create: (_) => VeterinariProvider(api)),
        ChangeNotifierProvider(create: (_) => RecenzijeProvider(api)),
        ChangeNotifierProvider(create: (_) => UslugeProvider(api)),
        ChangeNotifierProvider(create: (_) => VrsteUslugaProvider(api)),
        ChangeNotifierProvider(create: (_) => PromocijeProvider(api)),
        ChangeNotifierProvider(create: (_) => GradoviProvider(api)),
        ChangeNotifierProvider(create: (_) => RaseProvider(api)),
        ChangeNotifierProvider(create: (_) => VrsteLjubimacaProvider(api)),
        ChangeNotifierProvider(create: (_) => SpecijalnostiProvider(api)),
        ChangeNotifierProvider(create: (_) => UlogeProvider(api)),
        ChangeNotifierProvider(create: (_) => NaciniPlacanjaProvider(api)),
        Provider(create: (_) => StatistikaProvider(api)),
        Provider(create: (_) => ProfilProvider(api)),
        Provider(create: (_) => SlikeProvider(api)),
      ],
      child: const EvetDesktopApp(),
    ),
  );
}

class EvetDesktopApp extends StatelessWidget {
  const EvetDesktopApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'PetCity',
      debugShowCheckedModeBanner: false,
      theme: EvetTheme.svijetla,
      locale: const Locale('bs'),
      supportedLocales: const [Locale('bs')],
      localizationsDelegates: GlobalMaterialLocalizations.delegates,
      home: Consumer<AuthProvider>(
        builder: (context, auth, _) => auth.jePrijavljen ? const MasterScreen() : const LoginScreen(),
      ),
    );
  }
}
