using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Miski.Application.Services;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default);
    bool FileExists(string fileUrl);
}

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(IConfiguration configuration)
    {
        // Lee la ruta desde appsettings según el ambiente
        _basePath = configuration["FileStorage:BasePath"]
                    ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        // Crear el directorio si no existe
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("El archivo no puede estar vacío");

        var folderPath = Path.Combine(_basePath, folder);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return $"/uploads/{folder}/{fileName}";
    }

    public Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return Task.CompletedTask;

        var filePath = Path.Combine(_basePath, fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }

    public bool FileExists(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return false;

        var filePath = Path.Combine(_basePath, fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        return File.Exists(filePath);
    }
}