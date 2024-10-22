using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.DataMongoDb;
using JustLabel.DataMongoDb.Converters;

namespace JustLabel.Repositories.MongoDb;

public class DatasetRepositoryMongoDb : IDatasetRepository
{
    private AppDbContextMongoDb _context;
    private readonly ILogger _logger;

    public DatasetRepositoryMongoDb(AppDbContextMongoDb context)
    {
        _context = context;
        _logger = Log.ForContext<DatasetRepositoryMongoDb>();
    }

    public int Add(DatasetModel model)
    {
        _logger.Debug($"Attempt to add a dataset {model.Title}");
        model.LoadDatetime = DateTime.Now;
        bool g = _context.Datasets.Any();
        int newId = g ? _context.Datasets.Select(u => u.Id).Max() : 0;
        model.Id = newId + 1;
        var gg = DatasetConverter.CoreToDbModel(model);
        _context.Datasets.Add(gg);
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
        var gg = _context.Datasets.FirstOrDefault(u => u.Id == id);
        DatasetModel res = DatasetConverter.DbToCoreModel(gg);
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
