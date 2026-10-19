// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'pay_pal_narudzba.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

PayPalNarudzba _$PayPalNarudzbaFromJson(Map<String, dynamic> json) =>
    PayPalNarudzba(
      uplataId: (json['uplataId'] as num).toInt(),
      orderId: json['orderId'] as String,
      approveUrl: json['approveUrl'] as String,
      returnUrl: json['returnUrl'] as String,
      cancelUrl: json['cancelUrl'] as String,
      iznos: (json['iznos'] as num).toDouble(),
      valuta: json['valuta'] as String,
    );

Map<String, dynamic> _$PayPalNarudzbaToJson(PayPalNarudzba instance) =>
    <String, dynamic>{
      'uplataId': instance.uplataId,
      'orderId': instance.orderId,
      'approveUrl': instance.approveUrl,
      'returnUrl': instance.returnUrl,
      'cancelUrl': instance.cancelUrl,
      'iznos': instance.iznos,
      'valuta': instance.valuta,
    };
