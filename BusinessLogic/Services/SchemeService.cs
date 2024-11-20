using Serilog;
using System.Linq;
using System.Collections.Generic;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;

namespace JustLabel.Services;

public class SchemeService : ISchemeService
{
    private ISchemeRepository _schemeRepository;
    private IUserRepository _userRepository;
    private IMarkedRepository _markedRepository;
    private readonly ILogger _logger;


    public SchemeService(ISchemeRepository schemeRepository, IUserRepository userRepository,
                            IMarkedRepository markedRepository)
    {
        _schemeRepository = schemeRepository;
        _userRepository = userRepository;
        _markedRepository = markedRepository;
        _logger = Log.ForContext<SchemeService>();
    }

    public void Add(SchemeModel model)
    {
        _logger.Debug($"Attempt to create scheme {model.Title}");

        if (string.IsNullOrWhiteSpace(model.Title))
        {
            _logger.Error($"Scheme has empty title");
            throw new SchemeException("Title field cannot be empty");
        }

        if (_userRepository.GetUserById(model.CreatorId) is null)
        {
            _logger.Error($"Creator of scheme {model.Title} does not exist");
            throw new SchemeException("CreatorId does not exist in the users list");
        }

        if (model.LabelIds.Count == 0)
        {
            _logger.Error($"There are no labels in the scheme {model.Title}");
            throw new SchemeException("There are no labels in the scheme");
        }

        _schemeRepository.Add(model);

        _logger.Information($"New scheme {model.Title}");

        _logger.Debug($"Scheme {model.Title} successfully added");
    }

    public void Delete(int id)
    {
        _logger.Debug($"Attempt to delete scheme ID{id}");

        if (_schemeRepository.Get(id) is null)
        {
            _logger.Error($"Scheme ID{id} with this id does not exist");
            throw new SchemeException("Scheme with this id does not exist");
        }

        var marked = _markedRepository.Get_By_SchemeId(id);
        foreach (var m in marked)
        {
            _markedRepository.Delete(m.Id);
        }

        _schemeRepository.Delete(id);

        _logger.Information($"Deleted scheme ID{id}");

        _logger.Debug($"Scheme ID{id} successfully deleted");
    }

    public SchemeModel Get(int id)
    {
        _logger.Debug($"Attempt to get scheme ID{id}");
        SchemeModel res = _schemeRepository.Get(id);
        if (res == null)
        {
            _logger.Error($"Scheme ID{id} with this id does not exist");
            throw new SchemeException("Scheme with this id does not exist");
        }
        _logger.Debug($"Scheme ID{id} successfully got");
        return res;
    }

    public List<SchemeModel> Get()
    {
        _logger.Debug($"Attempt to get schemes");
        List<SchemeModel> res = _schemeRepository.GetAll();
        _logger.Debug($"Schemes successfully got");
        return res;
    }

    public void Update(SchemeModel model)
    {
        _logger.Debug($"Attempt to update scheme {model.Title}");

        if (_schemeRepository.Get(model.Id) is null)
        {
            _logger.Error($"Scheme ID{model.Id} does not exist");
            throw new SchemeException("Scheme with this id does not exist");
        }

        if (string.IsNullOrWhiteSpace(model.Title))
        {
            _logger.Error($"Scheme ID{model.Id} has empty title");
            throw new SchemeException("Title field cannot be empty");
        }

        if (model.LabelIds.Count == 0)
        {
            _logger.Error($"There are no labels in the scheme {model.Id}");
            throw new SchemeException("There are no labels in the scheme");
        }

        _schemeRepository.Update(model);

        _logger.Information($"Updated scheme {model.Title}");

        _logger.Debug($"Scheme {model.Title} successfully update");
    }
}
