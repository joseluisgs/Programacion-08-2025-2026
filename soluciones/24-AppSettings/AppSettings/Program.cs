// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using AppSettings.Configuration;
using Microsoft.Extensions.Configuration;


Console.WriteLine("Hola AppSettings!");

Console.WriteLine("Cargando configuración desde appsettings.json con JsonSerializer...");
// Cargar usando Parser de Json
var json = File.ReadAllText("appsettings.json");
var config = JsonSerializer.Deserialize<Config>(json);

Console.WriteLine($"App: {config?.AppName ?? "Desconocido"} v{config?.Version ?? "Desconocida"}");
Console.WriteLine($"BD: {config?.Database.Host ?? "Desconocido"}:{config?.Database.Port ?? 0} ({config?.Database.Name ?? "Desconocida"})");
Console.WriteLine($"Logging: {config?.Logging.Level ?? "Desconocido"}");


Console.WriteLine("Cargando configuración desde appsettings.json con ConfigurationBuilder...");

// Usando ConfigurationBuilder
var conf = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", false, true)
    .Build();

// IMPORTANTE: Sin GetSection porque tu JSON no tiene esa jerarquía
var appConfig = conf.Get<Config>(); 

Console.WriteLine($"App: {appConfig?.AppName?? "Desconocido"} v{appConfig?.Version?? "Desconocida"}");
Console.WriteLine($"BD: {appConfig?.Database.Host?? "Desconocido"}:{appConfig?.Database.Port?? 0} ({appConfig?.Database.Name?? "Desconocida"})");
Console.WriteLine($"Logging: {appConfig?.Logging.Level?? "Desconocido"}");

// Puedo coger una sección concreta
var dbConfig = conf.GetSection("Database").Get<DatabaseConfig>();
Console.WriteLine($"BD: {dbConfig?.Host?? "Desconocido"}:{dbConfig?.Port?? 0} ({dbConfig?.Name?? "Desconocida"})");

// También puedo usar GetValue para valores simples
var appName = conf.GetValue<string>("AppName");
var logLevel = conf.GetValue<string>("Logging:Level");
Console.WriteLine($"App: {appName?? "Desconocido"}");
Console.WriteLine($"Logging: {logLevel?? "Desconocido"}");