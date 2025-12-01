using System.Drawing;

namespace TagsCloudVisualization;

public interface IPointGenerator
{
    public IEnumerable<Point> GetNextPoint();
}