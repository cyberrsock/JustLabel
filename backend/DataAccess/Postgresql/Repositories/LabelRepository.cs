using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Data;
using JustLabel.Data.Converters;

namespace JustLabel.Repositories;

public class LabelRepository : ILabelRepository
{
    private AppDbContext _context;
    private readonly ILogger _logger;

    public LabelRepository(AppDbContext context)
    {
        _context = context;
        _logger = Log.ForContext<LabelRepository>();
    }

    public int Add(LabelModel model)
    {
        _logger.Debug($"Attempt to add a label {model.Title}");
        _context.Labels.Add(LabelConverter.CoreToDbModel(model));
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
