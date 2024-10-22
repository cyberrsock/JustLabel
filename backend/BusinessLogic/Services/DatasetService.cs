using Serilog;
using System.Collections.Generic;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;

namespace JustLabel.Services;

public class DatasetService: IDatasetService
{
    private IDatasetRepository _datasetRepository;
    private IImageRepository _imageRepository;
    private IUserRepository _userRepository;
    private IMarkedRepository _markedRepository;
    private readonly ILogger _logger;

    public DatasetService(IDatasetRepository datasetRepository, IUserRepository userRepository,
                            IImageRepository imageRepository, IMarkedRepository markedRepository)
    {
        _datasetRepository = datasetRepository;
        _userRepository = userRepository;
        _imageRepository = imageRepository;
        _markedRepository = markedRepository;
        _logger = Log.ForContext<DatasetService>();
    }

    public int Create(DatasetModel model, List<ImageModel> images)
    {
        _logger.Debug($"Attempt to create dataset {model.Title}");

        if (string.IsNullOrWhiteSpace(model.Title))
        {
            _logger.Error($"Dataset has empty title");
            throw new FailedDatasetCreationException("Title field cannot be empty");
        }

        if (_userRepository.GetUserById(model.CreatorId) is null)
        {
            _logger.Error($"Creator of dataset {model.Title} does not exist");
            throw new UserNotExistsException("CreatorId does not exist in the users list");
        }

        _logger.Debug($"Data of dataset {model.Title} is correct");

        int id = _datasetRepository.Add(model);
        
        foreach (var image in images)
        {
            image.DatasetId = id;
            _imageRepository.Add(image);
        }

        _logger.Information($"New dataset: ${model.Title} (${images.Count})");

        _logger.Debug($"Dataset {model.Title} successfully added");

        return id;
    }

    public (DatasetModel dataset, List<ImageModel> images) Get(int id)
    {
        _logger.Debug($"Attempt to get dataset ID{id}");

        var foundDataset = _datasetRepository.Get(id);
        if (foundDataset is null)
        {
            _logger.Error($"Dataset ID{id} does not exist");
            throw new DatasetNotExitedException("Dataset with this id does not exist");
        }

        var foundImages = _imageRepository.GetAll(id);
        if (foundImages is null)
        {
            _logger.Error($"Dataset ID{id} does not exist");
            throw new DatasetNotExitedException("Dataset with this id does not exist");
        }

        _logger.Debug($"Dataset ID{id} successfully got");

        return (foundDataset, foundImages);
    }

    public List<DatasetModel> Get()
    {
        _logger.Debug($"Attempt to get datasets");
        List<DatasetModel> res = _datasetRepository.GetAll();
        _logger.Debug($"Datasets successfully got");
        return res;
    }

    public int WhichImage(int id)
    {
        _logger.Debug($"Attempt to get image");
        var res = _imageRepository.Get(id);
        if (res == null) {
            _logger.Debug($"Image not exists");
            return -1;
        }
        _logger.Debug($"Image successfully got");
        return res.DatasetId;
    }

    public void Delete(int id)
    {
        _logger.Debug($"Attempt to delete dataset ID{id}");

        var foundDataset = _datasetRepository.Get(id);
        if (foundDataset is null)
        {
            _logger.Error($"Dataset ID{id} does not exist");
            throw new DatasetNotExitedException("Dataset with this id does not exist");
        }

        _logger.Debug($"Attempt to get marked ID{id}");
        var res = _markedRepository.Get_By_DatasetId(id);
        _logger.Debug($"Marked ID{id} successfully got");

        _logger.Debug($"Attempt to delete marks");
        foreach(var r in res) {
            _markedRepository.Delete(r.Id);
        }
        _logger.Debug($"Marks successfully got");

        _datasetRepository.Delete(id);

        _logger.Information($"Deleted dataset ID${id}");

        _logger.Debug($"Dataset ID{id} successfully deleted");
    }
}
