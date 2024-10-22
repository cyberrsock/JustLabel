using System.Collections.Generic;
using JustLabel.Data.Models;

namespace IntegrationTests.Builders;

public class LabelDbModelBuilder
{
    private LabelDbModel _labelDbo = new();

    public LabelDbModelBuilder WithId(int id)
    {
        _labelDbo.Id = id;
        return this;
    }

    public LabelDbModelBuilder WithTitle(string title)
    {
        _labelDbo.Title = title;
        return this;
    }

    public LabelDbModel Build()
    {
        return _labelDbo;
    }
}