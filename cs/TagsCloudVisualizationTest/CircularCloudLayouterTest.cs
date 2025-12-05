using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.PointGenerator;
using TagsCloudVisualization.PointProvider;

namespace TagsCloudVisualizationTest;

public class CircularCloudLayouterTest
{
    private Point validCenter;
    private Size validRectangleSize;
    private IPointGenerator pointGenerator;
    private int maxPointsPerRectangle;

    [SetUp]
    public void Setup()
    {
        validCenter = new Point(1920 / 2, 1080 / 2);
        validRectangleSize = new Size(50, 30);
        pointGenerator = new SpiralPointGenerator(validCenter);
        maxPointsPerRectangle = 10000;
    }

    [TestCase(-500, 500)]
    [TestCase(500, -500)]
    public void CircularCloudLayouter_ShouldThrowException_WhenInvalidCenter(int centerX, int centerY)
    {
        var invalidCenter = new Point(centerX, centerY);

        var act = () => new CircularCloudLayouter(invalidCenter, maxPointsPerRectangle, pointGenerator);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Center should be greater than 0");
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void CircularCloudLayouter_ShouldThrowException_WhenInvalidCount(int count)
    {
        var act = () => new CircularCloudLayouter(validCenter, count, pointGenerator);

        act.Should().Throw<ArgumentException>()
            .WithMessage("maxPointsPerRectangle must be greater than 0");
    }

    [TestCase(-100, 100)]
    [TestCase(100, -100)]
    [TestCase(0, 100)]
    [TestCase(100, 0)]
    public void PutNextRectangle_ShouldThrowException_WhenInvalidRectangleSize(int rectangleWidth, int rectangleHeight)
    {
        var circularCloud = new CircularCloudLayouter(validCenter, maxPointsPerRectangle, pointGenerator);

        var act = () => circularCloud.PutNextRectangle(new Size(rectangleWidth, rectangleHeight));

        act.Should().Throw<ArgumentException>()
            .WithMessage("Rectangle size must be positive");
    }


    [Test]
    public void PutNextRectangle_ShouldThrowException_WhenNotFoundPointInPoints()
    {
        var circularCloud = new CircularCloudLayouter(validCenter, 1, pointGenerator);

        var act1 = () => circularCloud.PutNextRectangle(validRectangleSize);
        var act2 = () => circularCloud.PutNextRectangle(validRectangleSize);

        act1.Should().NotThrow();
        act2.Should().Throw<ArgumentException>()
            .WithMessage($"Failed to find place for the 1 rectangle");
    }


    [Test]
    public void PutNextRectangle_ShouldNotProduceIntersectingRectangles_WhenCalledMultipleTimes()
    {
        var circularCloud = new CircularCloudLayouter(validCenter, maxPointsPerRectangle, pointGenerator);
        var rectangles = new List<Rectangle>();
        var rectangleSize = new Size(20, 10);
        for (var i = 0; i < 50; i++)
        {
            var rect = circularCloud.PutNextRectangle(rectangleSize);
            rectangles.Add(rect);
        }

        Geometry.HasIntersectingRectangles(rectangles).Should().BeFalse();
    }
}