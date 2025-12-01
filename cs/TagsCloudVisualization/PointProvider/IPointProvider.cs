using System.Drawing;

namespace TagsCloudVisualization.PointProvider;

public interface IPointProvider
{
    public IEnumerable<Rectangle> GetRectangles(int count, Size size);
}