using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

public class ExcelUploadRequest
{
    [Required(ErrorMessage = "fileType alanı zorunludur.")]
    [FromForm(Name = "fileType")]
    public string FileType { get; set; }

    [Required(ErrorMessage = "file dosyası zorunludur.")]
    [FromForm(Name = "file")]
    public IFormFile File { get; set; }

    [FromForm(Name = "isOverwrite")]
    public bool IsOverwrite { get; set; }    
}
