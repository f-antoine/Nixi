using Moq;

namespace NixiTestTools.Components.Dtos
{
    // TODO : Rename
    internal abstract class DtoWithTimesBase
    {
        internal ISetup SetupRegistered;

        internal int NbTimes;
        internal Times Times => Times.Exactly(NbTimes);
    }
}