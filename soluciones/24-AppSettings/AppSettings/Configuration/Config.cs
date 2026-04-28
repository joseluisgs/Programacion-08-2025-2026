namespace AppSettings.Configuration;

public record Config(
    string AppName,
    string Version,
    DatabaseConfig Database,
    LoggingConfig Logging
);

public record DatabaseConfig(string Host, int Port, string Name);
public record LoggingConfig(string Level);




