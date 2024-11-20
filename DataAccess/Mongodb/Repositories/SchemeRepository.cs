using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.DataMongoDb;
using JustLabel.DataMongoDb.Converters;

namespace JustLabel.Repositories.MongoDb;

public class SchemeRepositoryMongoDb : ISchemeRepository
{
    private AppDbContextMongoDb _context;
    private readonly ILogger _logger;

    public SchemeRepositoryMongoDb(AppDbContextMongoDb context)
    {
        _context = context;
        _logger = Log.ForContext<SchemeRepositoryMongoDb>();
    }

    public void Add(SchemeModel model)
    {
        _logger.Debug($"Attempt to add a scheme {model.Title}");
        model.CreateDatetime = DateTime.Now;
        bool g = _context.Schemes.Any();
        int newId = g ? _context.Schemes.Select(u => u.Id).Max() : 0;
        model.Id = newId + 1;
        var schemeDbModel = SchemeConverter.CoreToDbModel(model);
        _context.Schemes.Add(schemeDbModel);
        _context.SaveChanges();
        var labelSchemeDbModels = SchemeConverter.CoreToDbConnectModel(model);
        bool g2 = _context.LabelsSchemes.Any();
        int newId2 = g2 ? _context.LabelsSchemes.Select(u => u.Id).Max() : 0;
        foreach (var labelSchemeModel in labelSchemeDbModels)
        {
            newId2++;
            labelSchemeModel.Id = newId2;
            _context.LabelsSchemes.Add(labelSchemeModel);
        }
        _context.SaveChanges();
        _logger.Debug($"Scheme {model.Title} successfully added");
    }

    public void Delete(int id)
    {
        _logger.Debug($"Attempt to delete a scheme ID{id}");
        var scheme = _context.Schemes.FirstOrDefault(u => u.Id == id);
        if (scheme is not null)
        {
            _context.Schemes.Remove(scheme);
            _context.SaveChanges();
        }
        _logger.Debug($"Scheme ID{id} successfully deleted");
    }

    public SchemeModel? Get(int id)
    {
        _logger.Debug($"Attempt to get a scheme ID{id}");
        var LabelsSchemes = _context.LabelsSchemes.Where(u => u.SchemeId == id).ToList();
        var Scheme = _context.Schemes.FirstOrDefault(u => u.Id == id);
        SchemeModel res = SchemeConverter.DbToCoreModel(Scheme, LabelsSchemes);
        _logger.Debug($"Scheme ID{id} successfully got");
        return res;
    }

    public List<SchemeModel> GetAll()
    {
        _logger.Debug($"Attempt to get schemes");
        var LabelsSchemes = _context.Schemes.ToList();
        var schemeModels = new List<SchemeModel>();

        foreach (var scheme in LabelsSchemes)
        {
            var LabelsForScheme = _context.LabelsSchemes.Where(ls => ls.SchemeId == scheme.Id).ToList();
            var gg = SchemeConverter.DbToCoreModel(scheme, LabelsForScheme);
            schemeModels.Add(gg);
        }

        _logger.Debug($"Schemes successfully got");
        return schemeModels;
    }

    public void Update(SchemeModel model)
    {
        _logger.Debug($"Attemp to update scheme ID{model.Id}");
        var scheme = _context.Schemes.FirstOrDefault(u => u.Id == model.Id);
        if (scheme is not null)
        {
            scheme.Title = model.Title;
            scheme.Description = model.Description;
            scheme.CreatorId = model.CreatorId;
            _context.SaveChanges();
        }
        _logger.Debug($"Scheme ID{model.Id} successfully updated");
    }
}
