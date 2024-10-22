using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Data;
using JustLabel.Data.Converters;

namespace JustLabel.Repositories;

public class ImageRepository : IImageRepository
{
    private AppDbContext _context;
    private readonly ILogger _logger;

    public ImageRepository(AppDbContext context)
    {
        _context = context;
        _logger = Log.ForContext<ImageRepository>();
    }

    public void Add(ImageModel model)
    {
        _logger.Debug($"Attempt to add a image of dataset ID{model.DatasetId}");
        _context.Images.Add(ImageConverter.CoreToDbModel(model));
        _context.SaveChanges();
        _logger.Debug($"Image of dataset ID{model.DatasetId} successfully added");
    }

    public ImageModel? Get(int id)
    {
        _logger.Debug($"Attempt to get a image of dataset ID{id}");
        ImageModel res = ImageConverter.DbToCoreModel(_context.Images.FirstOrDefault(u => u.Id == id));
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
