import 'package:json_annotation/json_annotation.dart';

part 'pay_pal_narudzba.g.dart';

@JsonSerializable(explicitToJson: true)
class PayPalNarudzba {
  final int uplataId;
  final String orderId;
  final String approveUrl;
  final String returnUrl;
  final String cancelUrl;
  final double iznos;
  final String valuta;

  const PayPalNarudzba({
    required this.uplataId,
    required this.orderId,
    required this.approveUrl,
    required this.returnUrl,
    required this.cancelUrl,
    required this.iznos,
    required this.valuta,
  });

  factory PayPalNarudzba.fromJson(Map<String, dynamic> json) => _$PayPalNarudzbaFromJson(json);

  Map<String, dynamic> toJson() => _$PayPalNarudzbaToJson(this);
}
