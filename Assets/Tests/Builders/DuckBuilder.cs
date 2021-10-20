using Assets.ScriptExample.ComponentsWithInterface;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class DuckBuilder
    {
        private Duck duck;

        private DuckBuilder()
        {
            // New Scene for each test iteration, because if build many rootObject with same name
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            duck = new GameObject("MainDuck").AddComponent<Duck>();
        }

        internal static DuckBuilder Create()
        {
            return new DuckBuilder();
        }

        internal Duck BuildDuck()
        {
            return duck;
        }

        internal Duck BuildFullDuck()
        {
            WithWing();
            WithChildOfDuckCompanyAndBackPackOnIt();
            WithPocket();

            WithFirstLakeRoot();
            WithSkyAndSecondLake();

            return duck;
        }

        internal DuckBuilder WithWing()
        {
            duck.gameObject.AddComponent<DuckWings>();
            return this;
        }

        internal DuckBuilder WithChildOfDuckCompanyAndBackPackOnIt()
        {
            GameObject duckCompany = new GameObject("DuckCompany");
            duckCompany.AddComponent<DuckBackPack>();
            duck.transform.parent = duckCompany.transform;
            return this;
        }

        internal DuckBuilder WithPocket()
        {
            GameObject pocket = new GameObject("Pocket");
            pocket.AddComponent<DuckBackPack>();
            pocket.transform.parent = duck.transform;
            return this;
        }

        internal DuckBuilder WithFirstLakeRoot()
        {
            new GameObject("FirstLake").AddComponent<Lake>();
            return this;
        }

        internal DuckBuilder WithSkyAndSecondLake()
        {
            GameObject sky = new GameObject("Sky");
            GameObject secondLake = new GameObject("SecondLake");
            secondLake.AddComponent<Lake>();
            secondLake.transform.parent = sky.transform;
            return this;
        }
    }
}
