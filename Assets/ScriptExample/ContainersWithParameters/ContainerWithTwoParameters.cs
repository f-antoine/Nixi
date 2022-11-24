using ScriptExample.Characters;

namespace ScriptExample.ContainersWithParameters
{
    public interface IContainerWithTwoParameters
    {
        Sorcerer SorcererFromParameter { get; set; }
        int IntFromSecondParameter { get; set; }
    }

    public class ContainerWithTwoParameters : IContainerWithTwoParameters
    {
        public Sorcerer SorcererFromParameter { get; set; }
        public int IntFromSecondParameter { get; set; }


        public ContainerWithTwoParameters(Sorcerer sorcererToUseAsParameter, int secondParameter)
        {
            SorcererFromParameter = sorcererToUseAsParameter;
            IntFromSecondParameter = secondParameter;
        }
    }
}