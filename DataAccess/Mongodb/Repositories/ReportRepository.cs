using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.DataMongoDb;
using JustLabel.DataMongoDb.Converters;

namespace JustLabel.Repositories.MongoDb;

public class ReportRepositoryMongoDb : IReportRepository
{
    private AppDbContextMongoDb _context;
    private readonly ILogger _logger;

    public ReportRepositoryMongoDb(AppDbContextMongoDb context)
    {
        _context = context;
        _logger = Log.ForContext<ReportRepositoryMongoDb>();
    }

    public void Create(ReportModel model)
    {
        _logger.Debug($"Attempt to add a report for ID{model.MarkedId}");
        model.LoadDatetime = DateTime.Now;
        bool g = _context.Reports.Any();
        int newId = g ? _context.Reports.Select(u => u.Id).Max() : 0;
        model.Id = newId + 1;
        var gg = ReportConverter.CoreToDbModel(model);
        _context.Reports.Add(gg);
        _context.SaveChanges();
        _logger.Debug($"Report for ID{model.MarkedId} successfully added");
    }

    public List<ReportModel> GetAll()
    {
        _logger.Debug($"Attempt to get reports");
        var reportDbModels = _context.Reports.ToList();
        List<ReportModel> res = reportDbModels.Select(model => ReportConverter.DbToCoreModel(model)).ToList();
        _logger.Debug($"Reports successfully got");
        return res;
    }
}
