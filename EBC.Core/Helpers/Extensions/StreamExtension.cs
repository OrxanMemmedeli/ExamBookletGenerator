using Microsoft.AspNetCore.Http;

namespace EBC.Core.Helpers.Extensions;

/// <summary>
/// Stream obyektləri üçün genişləndirici metodlar sinfi.
/// </summary>
public static class StreamExtension
{
    /// <summary>
    /// Verilən axının sonuna qədər oxuyur və məzmununu bayt massivində qaytarır.
    /// </summary>
    /// <param name="stream">Oxunacaq giriş axını.</param>
    /// <param name="bufferSize">Oxumaq üçün istifadə olunan bufferin ölçüsü. Standart olaraq 4096 bayt təyin edilir.</param>
    /// <returns>Axının məzmununu saxlayan bayt massivi.</returns>
    public static byte[] ReadStreamToTheEnd(this Stream stream, int bufferSize = 4096)
    {
        // Null yoxlaması əlavə olundu
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));


        long originalPosition = 0;


        // Əgər axın axtarış dəstəyinə malikdirsə, başlanğıc mövqeyini yadda saxlayırıq və mövqeyi sıfıra təyin edirik.
        if (stream.CanSeek)
        {
            originalPosition = stream.Position;
            stream.Position = 0;
        }

        try
        {
            byte[] readBuffer = new byte[bufferSize];
            int totalBytesRead = 0;
            int bytesRead;

            // Axını oxumağa davam edirik
            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                // Əgər buffer doludursa, onu iki dəfə böyüdürük
                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        byte[] temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            // Dəqiq ölçülü bir final buffer yaradılır
            if (readBuffer.Length != totalBytesRead)
            {
                byte[] buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                return buffer;
            }

            return readBuffer;
        }
        finally
        {
            // Axının mövqeyi başlanğıca qaytarılır (əgər mümkündürsə)
            if (stream.CanSeek)
            {
                stream.Position = originalPosition;
            }
        }
    }


    /// <summary>
    /// Bayt massivini <see cref="IFormFile"/> tipinə çevirir.
    /// </summary>
    /// <param name="bytes">Çevriləcək bayt massivi.</param>
    /// <param name="fileName">Faylın adı.</param>
    /// <param name="contentType">Faylın məzmun növü (content type).</param>
    /// <returns>Bayt massivini təmsil edən <see cref="IFormFile"/> obyekti.</returns>
    public static IFormFile ConvertToFormFile(this byte[] bytes, string fileName, string contentType)
    {
        // Null yoxlamaları əlavə olundu
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentNullException.ThrowIfNull(fileName, nameof(fileName));
        ArgumentNullException.ThrowIfNull(contentType, nameof(contentType));

        var stream = new MemoryStream(bytes);
        return new FormFile(stream, 0, bytes.Length, fileName, fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }
}






/*






                                Numune KOD
[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    /// <summary>
    /// Uploads a file and reads its content into a byte array.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <param name="bufferSize">The buffer size to use for reading the file. Default is 4096 bytes.</param>
    /// <returns>Returns the size of the uploaded file in bytes.</returns>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] int bufferSize = 4096)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ReadStreamToTheEnd(bufferSize);
            
            // Here, you can process the byte array as needed
            // For example, save it to the database or another storage
            
            return Ok(new { FileSize = fileBytes.Length });
        }
    }

    -------------------------

    /// <summary>
    /// Uploads a file, converts it to a byte array, and then converts it back to an IFormFile.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <param name="bufferSize">The buffer size to use for reading the file. Default is 4096 bytes.</param>
    /// <returns>Returns the size of the uploaded file and the file name.</returns>
    [HttpPost("process-file")]
    public async Task<IActionResult> ProcessFile(IFormFile file, [FromQuery] int bufferSize = 4096)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        // 1. Read the file to a byte array
        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ReadStreamToTheEnd(bufferSize);
        }

        // 2. Convert the byte array back to IFormFile
        var newFormFile = fileBytes.ToFormFile(file.FileName, file.ContentType);

        // 3. Process the new IFormFile (example: saving it to a temporary directory)
        var filePath = Path.Combine(Path.GetTempPath(), newFormFile.FileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await newFormFile.CopyToAsync(fileStream);
        }

        return Ok(new { FileSize = fileBytes.Length, FileName = newFormFile.FileName });
    }
}
*/