using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace notification.scheduler.job.queries.unit.test;

public class AutoFixtureDataAttribute : AutoDataAttribute
{
    public AutoFixtureDataAttribute() : base(() => new Fixture().Customize(new AutoNSubstituteCustomization()))
    {
        
    }
}