using System.Drawing;

namespace TagsCloudVisualization;

public class SpiralPointProvider(Point centerCloud)
{
    private readonly CircularCloudLayouter circularCloudLayouter = new(centerCloud);
    
    private readonly Random random = new();

    public IEnumerable<Rectangle> GetCircularCloudRectangles(int count, Size rectangleSize, double minScaleFactor = 0)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be positive");
        if (minScaleFactor == 0)
        {
            for (var i = 0; i < count; i++)
                yield return circularCloudLayouter.PutNextRectangle(rectangleSize);
            yield break;
        }
        
        if (minScaleFactor is < 0 or > 1)
            throw new ArgumentException("minScaleFactor must be between 0 and 1");
        for (var i = 0; i < count; i++)
        {
            var rnd = random.NextDouble() + minScaleFactor;
            var rndWidth = (int)Math.Round(rectangleSize.Width * rnd);
            var rndHeight = (int)Math.Round(rectangleSize.Height * rnd);
            yield return circularCloudLayouter.PutNextRectangle(new Size(rndWidth, rndHeight));
        }
    }
}