using System.Drawing;

namespace TagsCloudVisualization.PointProvider;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
}