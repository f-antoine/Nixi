using Moq;

namespace Assets.Tests.ZombuildCoreTestsUtils.Templates.Components.Dtos
{
    internal abstract class DtoWithTimesBase
    {
        internal ISetup SetupRegistered;

        internal int NbTimes;
        internal Times Times => Times.Exactly(NbTimes);
    }
}