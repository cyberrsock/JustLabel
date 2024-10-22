using Serilog;
using System.Collections.Generic;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;

namespace JustLabel.Services;

public class ReportService : IReportService
{
    private IReportRepository _reportRepository;
    private IMarkedRepository _markedRepository;
    private IUserRepository _userRepository;
    private readonly ILogger _logger;

    public ReportService(IReportRepository reportRepository, IMarkedRepository markedRepository, IUserRepository userRepository)
    {
        _reportRepository = reportRepository;
        _markedRepository = markedRepository;
        _userRepository = userRepository;
        _logger = Log.ForContext<ReportService>();
    }

    public void Create(ReportModel model)
    {
        _logger.Debug($"Attempt to create report for marked ID{model.Id}");

        if (_userRepository.GetUserById(model.CreatorId) is null)
        {
            _logger.Error($"Creator of report does not exist");
            throw new ReportException("CreatorId does not exist in the users list");
        }

        // if (_markedRepository.Get(model.MarkedId) is null)
        // {
        //     _logger.Error($"Marked of report does not exist");
        //     throw new ReportException("MarkedId does not exist in the marked list");
        // }

        _reportRepository.Create(model);

        _logger.Information($"New report for marked ID{model.MarkedId}");

        _logger.Debug($"Marked for image {model.MarkedId} successfully added");
    }

    public List<ReportModel> Get()
    {
        _logger.Debug($"Attempt to get reports");
        List<ReportModel> res = _reportRepository.GetAll();
        _logger.Debug($"Reports successfully got");
        return res;
    }
}
