using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest;

public class SpiralPointProviderTest
{
    private Point validCenter;
    private Size validRectangleSize;

    [SetUp]
    public void Setup()
    {
        validCenter = new Point(1920 / 2, 1080 / 2);
        validRectangleSize = new Size(20, 10);
    }
    
    [TestCase(-1)]
    [TestCase(0)]
    public void GetCircularCloudRectangles_ShouldThrowException_WhenCountIsNotPositive(int count)
    {
        var spiralPointProvider = new SpiralPointProvider(validCenter);
        
        var act = () => spiralPointProvider
            .GetCircularCloudRectangles(count, validRectangleSize)
            .ToList();

        act.Should().Throw<ArgumentException>()
            .WithMessage("Count must be positive");
    }

    [Test]
    public void GetCircularCloudRectangles_ShouldNotProduceIntersectingRectangles_WhenCalledMultipleTimes()
    {
        var spiralPointProvider = new SpiralPointProvider(validCenter);

        var rectangles = spiralPointProvider
            .GetCircularCloudRectangles(100, validRectangleSize)
            .ToList();

        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j])
                    .Should().BeFalse($"rect #{i} should not intersect rect #{j}");
            }
        }
    }
    
    [TestCase(-1)]
    [TestCase(2)]
    public void GetCircularCloudRectangles_ShouldThrowException_WhenInvalidCoefficient(double minScaleFactor)
    {
        var spiralPointProvider = new SpiralPointProvider(validCenter);

        var act = () => spiralPointProvider
            .GetCircularCloudRectangles(100, validRectangleSize, minScaleFactor)
            .ToList();
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("minScaleFactor must be between 0 and 1");
    }
    
    [TestCase(0.1)]
    [TestCase(0.2)]
    [TestCase(0.5)]
    [TestCase(0.9)]
    public void GetCircularCloudRectangles_ShouldNotProduceIntersectingRectangles_WhenValidCoefficient(double minScaleFactor)
    {
        var spiralPointProvider = new SpiralPointProvider(validCenter);

        var rectangles = spiralPointProvider
            .GetCircularCloudRectangles(100, validRectangleSize, minScaleFactor)
            .ToList();

        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j])
                    .Should().BeFalse($"rect #{i} should not intersect rect #{j}");
            }
        }
    }
}