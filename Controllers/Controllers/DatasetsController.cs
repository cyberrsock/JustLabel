using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using JustLabel.Models;
using JustLabel.DTOModels;
using JustLabel.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace JustLabel.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DatasetsController : ControllerBase
{
    private IDatasetService _datasetService;

    public DatasetsController(IDatasetService datasetService)
    {
        _datasetService = datasetService;
    }

    private static bool IsImageFile(string fileName)
    {
        string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tif", ".tiff", ".webp", ".svg", ".ico" };
        string extension = Path.GetExtension(fileName).ToLower();
        return validExtensions.Contains(extension);
    }

    [HttpPost]
    public object Create([FromForm] DatasetArchiveDTOModel model)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        if (model.Archive == null || model.Archive.Length <= 0)
        {
            return BadRequest("Bad archive");
        }

        using var stream = new MemoryStream();
        model.Archive.CopyTo(stream);

        using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read);
        bool allImages = true;
        foreach (var entry in zipArchive.Entries)
        {
            if (!IsImageFile(entry.FullName))
            {
                allImages = false;
                break;
            }
        }

        if (!allImages)
        {
            return BadRequest("Not all archive files are images");
        }

        string folderName = Guid.NewGuid().ToString();
        string folderPath = Path.Combine("../DataAccess/Datasets", folderName);
        Directory.CreateDirectory(folderPath);

        List<ImageModel> images = new();

        DatasetModel dataset = new()
        {
            Title = model.Title,
            Description = model.Description,
            CreatorId = userId
        };

        foreach (var entry in zipArchive.Entries)
        {
            string filePath = Path.Combine(folderPath, entry.FullName);
            using var entryStream = entry.Open();
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                entryStream.CopyTo(fileStream);
                Image img = Image.FromStream(fileStream);
                ImageModel image = new()
                {
                    Path = filePath,
                    Width = img.Width,
                    Height = img.Height
                };
                images.Add(image);
            }

        }

        int newId;

        try
        {
            try
            {
                newId = _datasetService.Create(dataset, images);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        catch (Exception ex)
        {
            Directory.Delete(folderPath, true);
            return StatusCode(500, ex.Message);
        }

        return Ok(newId);
    }

    [HttpGet("{id}")]
    public object Get(int id)
    {
        var (dataset, imagePaths) = _datasetService.Get(id);
        if (imagePaths == null || !imagePaths.Any())
        {
            return NotFound("No images found for the specified ID");
        }


        List<ArchiveInfo> imagesData = new();
        foreach (var imagePath in imagePaths)
        {
            byte[] imageData = System.IO.File.ReadAllBytes(imagePath.Path);
            imagesData.Add(new ArchiveInfo { Id = imagePath.Id, Image = imageData });
        }

        DatasetDTOModel to_dataset = new()
        {
            Title = dataset.Title,
            Description = dataset.Description,
            Archive = imagesData
        };

        return to_dataset;
    }

    [HttpGet]
    public object GetAll()
    {
        return _datasetService.Get();
    }

    // [HttpPatch("{id}")]
    // public object GetImage(int id)
    // {
    //     return Ok(_datasetService.WhichImage(id));
    // }

    [HttpDelete("{id}")]
    public object Delete(int id)
    {
        _datasetService.Delete(id);

        return Ok();
    }
}
