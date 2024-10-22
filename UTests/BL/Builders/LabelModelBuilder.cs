using JustLabel.Models;

namespace UnitTests.Builders;

public class LabelModelBuilder
{
    private LabelModel _labelModel = new();

    public LabelModelBuilder WithId(int id)
    {
        _labelModel.Id = id;
        return this;
    }

    public LabelModelBuilder WithTitle(string title)
    {
        _labelModel.Title = title;
        return this;
    }

    public LabelModel Build()
    {
        return _labelModel;
    }
}
