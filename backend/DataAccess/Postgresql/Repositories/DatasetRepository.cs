using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Data;
using JustLabel.Data.Converters;

namespace JustLabel.Repositories;

public class DatasetRepository : IDatasetRepository
{
    private AppDbContext _context;
    private readonly ILogger _logger;

    public DatasetRepository(AppDbContext context)
    {
        _context = context;
        _logger = Log.ForContext<DatasetRepository>();
    }

    public int Add(DatasetModel model)
    {
        _logger.Debug($"Attempt to add a dataset {model.Title}");
        model.LoadDatetime = DateTime.Now;
        _context.Datasets.Add(DatasetConverter.CoreToDbModel(model));
        _context.SaveChanges();
        var latestDataset = _context.Datasets.OrderByDescending(d => d.LoadDatetime).FirstOrDefault();
        _logger.Debug($"Dataset {model.Title} successfully added");
        return latestDataset.Id;
    }

    public void Delete(int id)
    {
        _logger.Debug($"Attempt to delete a dataset ID{id}");
        var user = _context.Datasets.FirstOrDefault(u => u.Id == id);
        if (user is not null)
        {
            _context.Datasets.Remove(user);
            _context.SaveChanges();
        }
        _logger.Debug($"Dataset ID{id} successfully deleted");
    }

    public DatasetModel? Get(int id)
    {
        _logger.Debug($"Attempt to get a dataset ID{id}");
        DatasetModel res = DatasetConverter.DbToCoreModel(_context.Datasets.FirstOrDefault(u => u.Id == id));
        _logger.Debug($"Dataset ID{id} successfully got");
        return res;
    }

    public List<DatasetModel> GetAll()
    {
        _logger.Debug($"Attempt to delete datasets");
        var datasetsDbModels = _context.Datasets.ToList();
        List<DatasetModel> res = datasetsDbModels.Select(model => DatasetConverter.DbToCoreModel(model)).ToList();
        _logger.Debug($"Datasets successfully got");
        return res;
    }
}
