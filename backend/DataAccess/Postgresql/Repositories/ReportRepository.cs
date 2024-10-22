using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Data;
using JustLabel.Data.Converters;

namespace JustLabel.Repositories;

public class ReportRepository : IReportRepository
{
    private AppDbContext _context;
    private readonly ILogger _logger;

    public ReportRepository(AppDbContext context)
    {
        _context = context;
        _logger = Log.ForContext<ReportRepository>();
    }

    public void Create(ReportModel model)
    {
        _logger.Debug($"Attempt to add a report for ID{model.MarkedId}");
        model.LoadDatetime = DateTime.Now;
        _context.Reports.Add(ReportConverter.CoreToDbModel(model));
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
