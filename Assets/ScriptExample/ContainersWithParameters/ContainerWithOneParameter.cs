using ScriptExample.Characters;

namespace ScriptExample.ContainersWithParameters
{
    public interface IContainerWithOneParameter
    {
        Sorcerer SorcererFromParameter { get; set; }
    }

    public class ContainerWithOneParameter : IContainerWithOneParameter
    {
        public Sorcerer SorcererFromParameter { get; set; }

        public ContainerWithOneParameter(Sorcerer sorcererToUseAsParameter)
        {
            SorcererFromParameter = sorcererToUseAsParameter;
        }
    }
}