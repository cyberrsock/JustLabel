using JustLabel.Data.Models;

namespace IntegrationTests.Builders;

public class LabelSchemeDbModelBuilder
{
    private LabelSchemeDbModel _labelSchemeDbo = new();

    public LabelSchemeDbModelBuilder WithLabelId(int labelId)
    {
        _labelSchemeDbo.LabelId = labelId;
        return this;
    }

    public LabelSchemeDbModelBuilder WithSchemeId(int schemeId)
    {
        _labelSchemeDbo.SchemeId = schemeId;
        return this;
    }

    public LabelSchemeDbModel Build()
    {
        return _labelSchemeDbo;
    }
}
