using Serilog;
using System.Linq;
using System.Collections.Generic;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;

namespace JustLabel.Services;

public class LabelService : ILabelService
{
    private ILabelRepository _labelRepository;
    private ISchemeRepository _schemeRepository;
    private readonly ILogger _logger;

    public LabelService(ILabelRepository labelRepository, ISchemeRepository schemeRepository)
    {
        _labelRepository = labelRepository;
        _schemeRepository = schemeRepository;
        _logger = Log.ForContext<LabelService>();
    }

    public int Add(LabelModel model)
    {
        _logger.Debug($"Attempt to create label {model.Title}");

        model.Title = char.ToUpperInvariant(model.Title[0]) + model.Title[1..].ToLowerInvariant();
        
        var labels = _labelRepository.Get();
        if (labels.Any(label => label.Title == model.Title))
        {
            return labels.FirstOrDefault(label => label.Title == model.Title).Id;
        }

        int res = _labelRepository.Add(model);

        _logger.Information($"New label: ${model.Title}");

        _logger.Debug($"Label {model.Title} successfully added");
        
        return res;
    }

    public void Delete(int id)
    {
        _logger.Debug($"Attempt to delete label ID{id}");

        var labels = _labelRepository.Get();
        if (labels.All(label => label.Id != id))
        {
            _logger.Error($"Label ID{id} does not exist");
            throw new LabelException("The label does not exist");
        }
        var schemes = _schemeRepository.GetAll();
        if (schemes.Any(scheme => scheme.LabelIds.Any(label => label.Id == id)))
        {
            _logger.Error($"Label ID{id} is already associated with a scheme");
            throw new LabelException("The label is already associated with a scheme");
        }

        _labelRepository.Delete(id);

        _logger.Information($"Deleted label ID${id}");

        _logger.Debug($"Label ID{id} successfully deleted");
    }

    public List<LabelModel> Get()
    {
        _logger.Debug($"Attempt to get labels");
        List<LabelModel> res = _labelRepository.Get();
        _logger.Debug($"Labels successfully got");
        return res;
    }
}
