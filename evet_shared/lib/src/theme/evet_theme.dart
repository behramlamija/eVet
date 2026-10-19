import 'package:flutter/material.dart';

/// Boje i tema prema mockupima iz prijave (tamnoplava navigacija, crveni akcenat).
class EvetBoje {
  EvetBoje._();

  static const tamnoplava = Color(0xFF0D2C54);
  static const crvena = Color(0xFFE53935);
  static const pozadina = Color(0xFFF6F7FB);
  static const tekstSivi = Color(0xFF6B7280);
}

class EvetTheme {
  EvetTheme._();

  static ThemeData get svijetla {
    final shema = ColorScheme.fromSeed(
      seedColor: EvetBoje.tamnoplava,
      primary: EvetBoje.tamnoplava,
      secondary: EvetBoje.crvena,
      surface: Colors.white,
    );

    return ThemeData(
      colorScheme: shema,
      scaffoldBackgroundColor: EvetBoje.pozadina,
      appBarTheme: const AppBarTheme(backgroundColor: Colors.white, foregroundColor: EvetBoje.tamnoplava, elevation: 0),
      inputDecorationTheme: const InputDecorationTheme(
        border: OutlineInputBorder(),
        filled: true,
        fillColor: Colors.white,
        errorMaxLines: 3,
      ),
      filledButtonTheme: FilledButtonThemeData(
        style: FilledButton.styleFrom(
          backgroundColor: EvetBoje.tamnoplava,
          minimumSize: const Size(0, 48),
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
        ),
      ),
    );
  }
}
