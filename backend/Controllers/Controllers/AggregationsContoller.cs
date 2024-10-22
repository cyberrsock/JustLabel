// using System;
// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using JustLabel.Models;
// using JustLabel.Services.Interfaces;
// using JustLabel.DTOModels;
// using JustLabel.Exceptions;
// using System.IO.Compression;
// using System.Linq;
// using System.Xml.Serialization;
// using System.IO;
// using Object = JustLabel.DTOModels.Object;
// using System.Text;

// namespace JustLabel.Controllers;

// [ApiController]
// [ApiVersion("1.0")]
// [Route("api/v{version:apiVersion}/[controller]")]
// public class AggregationsController : ControllerBase
// {
//     private IMarkedService _markedService;
//     private IDatasetService _datasetService;
//     private ILabelService _labelService;

//     public AggregationsController(IMarkedService markedService, IDatasetService datasetService, ILabelService labelService)
//     {
//         _markedService = markedService;
//         _datasetService = datasetService;
//         _labelService = labelService;
//     }

//     public static MemoryStream GenerateXmlFile(List<AggregatedModel> markedModels, ImageModel img, List<LabelModel> labels)
//     {
//         Annotation annotation = new Annotation
//         {
//             Folder = "images",
//             Filename = $"image{markedModels[0].ImageId}.jpg",
//             Size = new Size
//             {
//                 Width = img.Width,
//                 Height = img.Height
//             },
//             Objects = markedModels.Select(area => new Object
//             {
//                 Name = labels.Find(u => u.Id == area.LabelId).Title,
//                 Xmin = area.X1,
//                 Ymin = area.Y1,
//                 Xmax = area.X2,
//                 Ymax = area.Y2
//             }).ToList()
//         };

//         XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
//         ns.Add("", "");

//         XmlSerializer serializer = new(typeof(Annotation));

//         MemoryStream memoryStream = new MemoryStream();
//         using (StreamWriter writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
//         {
//             serializer.Serialize(writer, annotation, ns);
//         }
//         memoryStream.Position = 0;
//         return memoryStream;
//     }

//     [HttpGet("{datasetId}/{schemeId}")]
//     public IActionResult Get(int datasetId, int schemeId)
//     {
//         if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
//         {
//             return BadRequest("User ID not found in the HttpContext");
//         }

//         var (dataset, imagePaths) = _datasetService.Get(datasetId);
//         var res = _markedService.GetForAggr(userId, datasetId, schemeId);
//         var labels = _labelService.Get();
//         var groupedByImageId = res.GroupBy(x => x.ImageId).Select(group => group.ToList()).ToList();

//         using (MemoryStream archiveStream = new MemoryStream())
//         {
//             using (ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
//             {
//                 foreach (var group in groupedByImageId)
//                 {
//                     var img = imagePaths.Find(v => v.Id == group[0].ImageId);
//                     string fileExtension = Path.GetExtension(img.Path);
//                     string destinationImagePath = $"images/image_{img.Id}{fileExtension}";

//                     var imageEntry = archive.CreateEntry(destinationImagePath, CompressionLevel.Fastest);
//                     using (var entryStream = imageEntry.Open())
//                     using (var imageStream = new FileStream(img.Path, FileMode.Open, FileAccess.Read))
//                     {
//                         imageStream.CopyTo(entryStream);
//                     }

//                     using (var xmlContentStream = GenerateXmlFile(group, img, labels))
//                     {
//                         var xmlEntry = archive.CreateEntry($"annotations/masks_{img.Id}.xml", CompressionLevel.Fastest);
//                         using (var entryStream = xmlEntry.Open())
//                         {
//                             xmlContentStream.CopyTo(entryStream);
//                         }
//                     }
//                 }
//             }

//             archiveStream.Position = 0;
//             return File(archiveStream.ToArray(), "application/zip", "annotations.zip");
//         }
//     }

// }