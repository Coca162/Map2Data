using System.Text;

namespace Map2Data;

public class Map
{
    public Map((string, string) size, List<SVGData> counties)
    {
        Size = size;
        Counties = counties;
    }

    public ValueTuple<string, string> Size { get; set; }

    public List<SVGData> Counties { get; set; }


    public static Map ProcessSVG(string svg)
    {
        var split = svg.Split("d=\"").Where(x => x.StartsWith("m ", StringComparison.InvariantCultureIgnoreCase));
        var trimmed = split.Select(x => x.Split('"')[0]);

        var tuple = (svg.Split("width=\"")[1].Split('"')[0], svg.Split("height=\"")[1].Split('"')[0]);

        return new(tuple, trimmed.Select(x => new SVGData(x)).ToList());
    }

    public static Map ProcessCSV(string csv)
    {
        var data = csv.Split('\n');

        var cordinates = data[0].Split(',');
        var tuple = (cordinates[0], cordinates[1]);

        List<SVGData> final = new(data.Length);
        foreach (var item in data.Skip(1))
        {
            var split = item.Split(',');
            final.Add(new SVGData(split[0], split[1]));
        }
        return new(tuple, final);
    }

    public static string ToCSV(Map map)
    {
        StringBuilder sb = new();

        sb.Append(map.Size.Item1);
        sb.Append(','); 
        sb.Append(map.Size.Item2);
        sb.Append('\n');

        foreach (var item in map.Counties)
        {
            sb.Append(item.Name);
            sb.Append(',');
            sb.Append(item.Path);
            sb.Append('\n');
        }
        return sb.ToString();
    }
}

public class SVGData
{
    public SVGData(string path) => Path = path;

    public SVGData(string name, string path) : this(path) => Name = name;

    public string Path { get; }

    public string Name { get; set; } = "";

    public bool Selected { get; set; } = false;
}
