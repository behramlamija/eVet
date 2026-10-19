import '../models/models.dart';

/// Nazivi enum vrijednosti za prikaz korisniku.
String nazivStatusa(StatusTermina status) => switch (status) {
      StatusTermina.naCekanju => 'Na čekanju',
      StatusTermina.potvrdjen => 'Potvrđen',
      StatusTermina.zavrsen => 'Završen',
      StatusTermina.otkazan => 'Otkazan',
      StatusTermina.odbijen => 'Odbijen',
    };

String nazivSpola(Spol spol) => switch (spol) {
      Spol.muski => 'Muški',
      Spol.zenski => 'Ženski',
    };
