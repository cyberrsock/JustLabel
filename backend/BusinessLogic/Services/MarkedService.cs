using Serilog;
using System.Linq;
using System.Collections.Generic;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;

namespace JustLabel.Services;

public class MarkedService : IMarkedService
{
    private ISchemeRepository _schemeRepository;
    private IMarkedRepository _markedRepository;
    private IUserRepository _userRepository;
    private IImageRepository _imageRepository;
    private readonly ILogger _logger;

    public MarkedService(ISchemeRepository schemeRepository, IMarkedRepository markedRepository,
                            IUserRepository userRepository, IImageRepository imageRepository)
    {
        _schemeRepository = schemeRepository;
        _markedRepository = markedRepository;
        _userRepository = userRepository;
        _imageRepository = imageRepository;
        _logger = Log.ForContext<MarkedService>();
    }
    public void Create(MarkedModel model)
    {
        _logger.Debug($"Attempt to create marked for image: {model.ImageId}");

        if (_userRepository.GetUserById(model.CreatorId) is null)
        {
            _logger.Error($"Creator of markup does not exist");
            throw new MarkedException("CreatorId does not exist in the users list");
        }

        if (_schemeRepository.Get(model.SchemeId) is null)
        {
            _logger.Error($"Scheme of markup does not exist");
            throw new MarkedException("SchemeId does not exist in the schemes list");
        }

        if (_imageRepository.Get(model.ImageId) is null)
        {
            _logger.Error($"Image of markup does not exist");
            throw new MarkedException("ImageId does not exist in the images list");
        }

        _markedRepository.Create(model);

        _logger.Information($"New marked for image: {model.ImageId}");

        _logger.Debug($"Marked for image {model.ImageId} successfully added");
    }

    public void Delete(int id)
    {
        _logger.Debug($"Attempt to delete marked ID{id}");

        _markedRepository.Delete(id);

        _logger.Information($"Deleted makred ID${id}");

        _logger.Debug($"Marked ID{id} successfully deleted");
    }

    public MarkedModel Get(MarkedModel model)
    {
        _logger.Debug($"Attempt to get marked ID{model.Id}");
        var res = _markedRepository.Get(model);
        if (res == null) {
            throw new MarkedException("Marked not found");
        }
        _logger.Debug($"Marked ID{model.Id} successfully got");
        return res;
    }

    public List<MarkedModel> GetAll(int admin_id)
    {
        _logger.Debug($"Attempt to get ID{admin_id}");
        var res = _markedRepository.GetAll();
        _logger.Debug($"Marked ID{admin_id} successfully got");

        if (!_userRepository.GetUserById(admin_id).IsAdmin)
        {
            return res.Where(item => item.IsBlocked == false).ToList();
        }
    
        return res;
    }

    public List<AggregatedModel> GetForAggr(int admin_id, int dataset_id, int scheme_id)
    {
        if (!_userRepository.GetUserById(admin_id).IsAdmin)
        {
            _logger.Error($"User ID{admin_id} is not admin");
            throw new AdminUserException("The user with AdminId is not admin");
        }

        var res = _markedRepository.GetAggregatedData(dataset_id, scheme_id);
        return res;
    }

    public List<MarkedModel> Get(int id, int admin_id)
    {
        _logger.Debug($"Attempt to get marked ID{id}");
        var res = _markedRepository.Get_By_DatasetId(id);
        _logger.Debug($"Marked ID{id} successfully got");

        if (!_userRepository.GetUserById(admin_id).IsAdmin)
        {
            return res.Where(item => item.IsBlocked == false || item.CreatorId == admin_id).ToList();
        }

        return res;
    }

    public void Update_Rects(MarkedModel model)
    {
        _logger.Debug($"Attempt to update marked ID{model.Id}");
        _markedRepository.Update_Rects(model);
        _logger.Debug($"Marked ID{model.Id} successfully update");
    }

    public void Update_Block(MarkedModel model)
    {
        if (!_userRepository.GetUserById(model.CreatorId).IsAdmin)
        {
            _logger.Error($"User ID{model.CreatorId} is not admin");
            throw new AdminUserException("The user with AdminId is not admin");
        }

        _logger.Debug($"Attempt to update marked ID{model.Id}");
        _markedRepository.Update_Block(model);
        _logger.Debug($"Marked ID{model.Id} successfully update");
    }
}
