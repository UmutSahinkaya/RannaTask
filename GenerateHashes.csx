using BCrypt.Net;

// Bu dosyayı çalıştırarak doğru BCrypt hash'lerini alabilirsiniz
// dotnet run --project HashGenerator.csproj

Console.WriteLine("=== BCrypt Password Hash Generator ===\n");

var passwords = new Dictionary<string, string>
{
    { "admin", "admin123" },
    { "paneluser", "panel123" },
    { "testuser", "test123" },
    { "demouser", "demo123" }
};

foreach (var pair in passwords)
{
    string hash = BCrypt.Net.BCrypt.HashPassword(pair.Value);
    Console.WriteLine($"Username: {pair.Key}");
    Console.WriteLine($"Password: {pair.Value}");
    Console.WriteLine($"Hash: {hash}");
    Console.WriteLine($"Verify: {BCrypt.Net.BCrypt.Verify(pair.Value, hash)}");
    Console.WriteLine();
}

Console.WriteLine("\n=== Configuration Format ===\n");
Console.WriteLine("// Copy these to your configuration files:");
Console.WriteLine();

foreach (var pair in passwords)
{
    string hash = BCrypt.Net.BCrypt.HashPassword(pair.Value);
    Console.WriteLine($"// {pair.Key} | Password: {pair.Value}");
    Console.WriteLine($"PasswordHash = \"{hash}\",");
    Console.WriteLine();
}
