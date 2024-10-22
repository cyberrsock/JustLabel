using JustLabel.Data.Models;

namespace UnitTests.Builders;

public class MarkedAreaDbModelBuilder
{
    private MarkedAreaDbModel _markedAreaDbo = new();

    public MarkedAreaDbModelBuilder WithMarkedId(int markedId)
    {
        _markedAreaDbo.MarkedId = markedId;
        return this;
    }

    public MarkedAreaDbModelBuilder WithAreaId(int areaId)
    {
        _markedAreaDbo.AreaId = areaId;
        return this;
    }

    public MarkedAreaDbModel Build()
    {
        return _markedAreaDbo;
    }
}
