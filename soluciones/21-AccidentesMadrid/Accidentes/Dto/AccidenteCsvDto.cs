namespace Accidentes.Dto;

public sealed record AccidenteCsvDto(
    string NumExpediente,
    string Fecha,
    string Hora,
    string Localizacion,
    string Numero,
    string CodDistrito,
    string Distrito,
    string TipoAccidente,
    string EstadoMeteorologico,
    string TipoVehiculo,
    string TipoPersona,
    string RangoEdad,
    string Sexo,
    string CodLesividad,
    string Lesividad,
    string CoordenadaXUtm,
    string CoordenadaYUtm,
    string PositivaAlcohol,
    string PositivaDroga
);