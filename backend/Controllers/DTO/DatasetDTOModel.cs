using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace JustLabel.DTOModels;

public class DatasetArchiveDTOModel
{
    public string Title { get; set; }

    public string Description { get; set; }

    public IFormFile Archive { get; set; }
}

public class ArchiveInfo
{
    public int Id { get; set; }
    public byte[] Image { get; set; }
}

public class DatasetDTOModel
{
    public string Title { get; set; }

    public string Description { get; set; }

    public List<ArchiveInfo> Archive { get; set; }
}
