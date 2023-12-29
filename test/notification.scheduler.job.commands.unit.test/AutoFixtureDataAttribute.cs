using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace notification.scheduler.job.commands.unit.test;

public class AutoFixtureDataAttribute : AutoDataAttribute
{
    public AutoFixtureDataAttribute() : base(() => new Fixture().Customize(new AutoNSubstituteCustomization()))
    {
        
    }
}