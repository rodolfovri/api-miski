using Microsoft.AspNetCore.Http;

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
    
    public LocalFileStorageService()
    {
        // Ruta base para guardar archivos en la máquina local
        // Por ejemplo: C:\MiskiFiles\
        _basePath = Path.Combine("C:", "MiskiFiles");
        
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

        // Crear carpeta si no existe
        var folderPath = Path.Combine(_basePath, folder);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Generar nombre único para el archivo
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(folderPath, fileName);

        // Guardar el archivo
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        // Retornar la URL relativa o ruta del archivo
        return $"/{folder}/{fileName}";
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

// Implementación para servidor (Azure Blob Storage, AWS S3, etc.)
// Comentado para uso futuro
/*
public class CloudFileStorageService : IFileStorageService
{
    // TODO: Implementar para Azure Blob Storage o AWS S3
    // private readonly BlobServiceClient _blobServiceClient;
    
    public CloudFileStorageService()
    {
        // Configuración de Azure o AWS
        // _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar subida a blob storage
        throw new NotImplementedException();
    }

    public async Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar eliminación de blob storage
        throw new NotImplementedException();
    }

    public bool FileExists(string fileUrl)
    {
        // TODO: Implementar verificación en blob storage
        throw new NotImplementedException();
    }
}
*/
