using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.DataMongoDb;
using JustLabel.DataMongoDb.Converters;

namespace JustLabel.Repositories.MongoDb;

public class LabelRepositoryMongoDb : ILabelRepository
{
    private AppDbContextMongoDb _context;
    private readonly ILogger _logger;

    public LabelRepositoryMongoDb(AppDbContextMongoDb context)
    {
        _context = context;
        _logger = Log.ForContext<LabelRepositoryMongoDb>();
    }

    public int Add(LabelModel model)
    {
        _logger.Debug($"Attempt to add a label {model.Title}");
        bool g = _context.Labels.Any();
        int newId = g ? _context.Labels.Select(u => u.Id).Max() : 0;
        model.Id = newId + 1;
        var gg = LabelConverter.CoreToDbModel(model);
        _context.Labels.Add(gg);
        _context.SaveChanges();

        var label = _context.Labels.FirstOrDefault(u => u.Title == model.Title);
        _logger.Debug($"Label {model.Title} successfully added");
        return label.Id;
    }

    public void Delete(int id)
    {
        _logger.Debug($"Attempt to delete a label ID{id}");
        var label = _context.Labels.FirstOrDefault(u => u.Id == id);
        if (label is not null)
        {
            _context.Labels.Remove(label);
            _context.SaveChanges();
        }
        _logger.Debug($"Label ID{id} successfully deleted");
    }

    public List<LabelModel> Get()
    {
        _logger.Debug($"Attempt to get labels");
        var labelDbModels = _context.Labels.ToList();
        List<LabelModel> res = labelDbModels.Select(model => LabelConverter.DbToCoreModel(model)).ToList();
        _logger.Debug($"Labels successfully got");
        return res;
    }
}
