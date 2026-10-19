// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'recenzija_insert_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

RecenzijaInsertRequest _$RecenzijaInsertRequestFromJson(
  Map<String, dynamic> json,
) => RecenzijaInsertRequest(
  terminId: (json['terminId'] as num).toInt(),
  uslugaPozitivna: json['uslugaPozitivna'] as bool? ?? false,
  veterinarPozitivan: json['veterinarPozitivan'] as bool? ?? false,
  komentar: json['komentar'] as String?,
);

Map<String, dynamic> _$RecenzijaInsertRequestToJson(
  RecenzijaInsertRequest instance,
) => <String, dynamic>{
  'terminId': instance.terminId,
  'uslugaPozitivna': instance.uslugaPozitivna,
  'veterinarPozitivan': instance.veterinarPozitivan,
  'komentar': instance.komentar,
};
