using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.PointProvider;

namespace TagsCloudVisualizationTest;

public class CircularCloudLayouterTest
{
    private Point validCenter;

    [SetUp]
    public void Setup()
    {
        validCenter = new Point(1920 / 2, 1080 / 2);
    }
    
    [TestCase(-500, 500)]
    [TestCase(500, -500)]
    public void CircularCloudLayouter_ShouldThrowException_WhenInvalidCenter(int centerX, int centerY)
    {
        var invalidCenter = new Point(centerX, centerY);
        var act = () => new CircularCloudLayouter(invalidCenter);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Center should be greater than 0");
    }
    
    [TestCase(-100, 100)]
    [TestCase(100, -100)]
    [TestCase(0, 100)]
    [TestCase(100, 0)]
    public void PutNextRectangle_ShouldThrowException_WhenInvalidRectangleSize(int rectangleWidth, int rectangleHeight)
    {
        var circularCloud = new CircularCloudLayouter(validCenter);
        var act = () => circularCloud.PutNextRectangle(new Size(rectangleWidth, rectangleHeight));

        act.Should().Throw<ArgumentException>()
            .WithMessage("Rectangle size must be positive");
    }
    

    [Test]
    public void PutNextRectangle_ShouldNotProduceIntersectingRectangles_WhenCalledMultipleTimes()
    {
        var circularCloud = new CircularCloudLayouter(validCenter);
        var rectangles = new List<Rectangle>();
        var rectangleSize = new Size(20, 10);
        for (var i = 0; i < 50; i++)
        {
            var rect = circularCloud.PutNextRectangle(rectangleSize);
            rectangles.Add(rect);
        }

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