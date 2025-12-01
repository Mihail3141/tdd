using System.Drawing;
using TagsCloudVisualization;
using FluentAssertions;

namespace TagsCloudVisualizationTest;

public class SpiralPointGeneratorTest
{
    private Point validCenter;
    private Size validSize;

    [SetUp]
    public void Setup()
    {
        validCenter = new Point(1920/2, 1080/2);
        validSize = new Size(1920, 1080);
    }


    [TestCase(-1, 100)]
    [TestCase(100, -1)]
    public void PointGenerator_ShouldThrowException_WhenInvalidCenter(int x, int y)
    {
        var invalidCenter = new Point(x, y);
        var act = () => new SpiralPointGenerator(invalidCenter, validSize);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Center coordinates must be non-negative");
    }

    [TestCase(-100, 100)]
    [TestCase(100, -100)]
    public void PointGenerator_ShouldThrowException_WhenInvalidImageSize(int imageWidth, int imageHeight)
    {
        var invalidSize = new Size(imageWidth, imageHeight);
        var act = () => new SpiralPointGenerator(validCenter,invalidSize);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Image size must be positive");
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void PointGenerator_ShouldThrowException_WhenInvalidRadius(int radius)
    {
        var act = () => new SpiralPointGenerator(validCenter, validSize, radius);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Radius must be positive");
    }

    [Test]
    public void GetPointsOnSpiral_ShouldEachNextPointFartherFromCenter_WhenValidParameters()
    {
        var pointGenerator = new SpiralPointGenerator(validCenter,validSize, 50, Math.PI / 12);
        var actualPoints = pointGenerator
            .GetPointsOnSpiral()
            .ToList();
        actualPoints.Should().HaveCountGreaterThan(1);
        var prevDistance = GetDistance(actualPoints[0], validCenter);
        foreach (var point in actualPoints.Skip(1))
        {
            var currentDistance = GetDistance(point, validCenter);
            currentDistance.Should().BeGreaterThanOrEqualTo(prevDistance);
            prevDistance = currentDistance;
        }
    }

    [Test]
    public void GetPointsOnSpiral_ShouldAngleGrowMonotonically_WhenValidParameters()
    {
        var pointGenerator = new SpiralPointGenerator(validCenter,validSize, 50, Math.PI / 12);
        var actualPoints = pointGenerator
            .GetPointsOnSpiral()
            .ToList();
        actualPoints.Should().HaveCountGreaterThan(1);
        
        var prevAngle = GetAngle(actualPoints[0], validCenter);

        foreach (var point in actualPoints.Skip(1))
        {
            var currentAngle = GetAngle(point, validCenter);
            var delta = GetNormalizedAngleDifference(currentAngle, prevAngle);
            delta.Should().BeGreaterThan(-1e-5);
            prevAngle = currentAngle;
        }
    }

    private static double GetNormalizedAngleDifference(double angle1, double angle2)
    {
        var result = angle1 - angle2;
        while (result <= -Math.PI)
            result += 2*Math.PI;
        while (result > Math.PI)
            result -= 2*Math.PI;
        return result;
    }

    private static double GetDistance(Point point1, Point point2)
    {
        var dx = point1.X - point2.X;
        var dy = point1.Y - point2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static double GetAngle(Point point1, Point point2)
    {
        var dx = point1.X - point2.X;
        var  dy = point1.Y - point2.Y;
        return Math.Atan2(dy, dx);
    }
}