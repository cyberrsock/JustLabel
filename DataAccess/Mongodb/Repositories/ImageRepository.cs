using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.DataMongoDb;
using JustLabel.DataMongoDb.Converters;

namespace JustLabel.Repositories.MongoDb;

public class ImageRepositoryMongoDb : IImageRepository
{
    private AppDbContextMongoDb _context;
    private readonly ILogger _logger;

    public ImageRepositoryMongoDb(AppDbContextMongoDb context)
    {
        _context = context;
        _logger = Log.ForContext<ImageRepositoryMongoDb>();
    }

    public void Add(ImageModel model)
    {
        _logger.Debug($"Attempt to add a image of dataset ID{model.DatasetId}");
        bool g = _context.Images.Any();
        int newId = g ? _context.Images.Select(u => u.Id).Max() : 0;
        model.Id = newId + 1;
        var gg = ImageConverter.CoreToDbModel(model);
        _context.Images.Add(gg);
        _context.SaveChanges();
        _logger.Debug($"Image of dataset ID{model.DatasetId} successfully added");
    }

    public ImageModel? Get(int id)
    {
        _logger.Debug($"Attempt to get a image of dataset ID{id}");
        var gg = _context.Images.FirstOrDefault(u => u.Id == id);
        ImageModel res = ImageConverter.DbToCoreModel(gg);
        _logger.Debug($"Image of dataset ID{id} successfully got");
        return res;
    }

    public List<ImageModel> GetAll(int id)
    {
        _logger.Debug($"Attempt to get images");
        var imagesDbModels = _context.Images.Where(u => u.DatasetId == id).ToList();
        List<ImageModel> res = imagesDbModels.Select(model => ImageConverter.DbToCoreModel(model)).ToList();
        _logger.Debug($"Images successfully got");
        return res;
    }
}
