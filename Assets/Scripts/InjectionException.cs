using System;

namespace Assets.Scripts
{
    [Serializable]
    public sealed class InjectionException : Exception
    {
        public InjectionException() { }

        public InjectionException(Type typeToFind)
            : base($"Aucun composant avec le type {typeToFind.Name} n'a été trouvé")
        {
        }

        public InjectionException(Type typeToFind, string nameToFind)
            : base($"Des composants avec le type {typeToFind.Name} ont été trouvé, mais aucun avec le nom {nameToFind}")
        {
        }

        public InjectionException(Type typeToFind, string nameToFind, int nbFound)
            : base($"Plusieurs composants ont été trouvés avec le type {typeToFind.Name} et le nom {nameToFind} ({nbFound} trouvés à la place d'un seul)")
        {
        }
    }
}