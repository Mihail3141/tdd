using System.Drawing;

namespace TagsCloudVisualization.Render;

public interface ICloudRenderer
{
    public Bitmap CreateRectangleCloud(IEnumerable<Rectangle> rectangles);
}