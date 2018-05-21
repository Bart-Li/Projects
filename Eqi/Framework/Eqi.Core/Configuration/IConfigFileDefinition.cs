namespace Eqi.Core.Configuration
{
    public interface IConfigFileDefinition
    {
        string Name { get; set; }

        string FilePath { get; set; }

        FileFormat Format { get; set; }

        string ToString();
    }
}
