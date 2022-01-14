# Table of contents
- [Table of contents](#table-of-contents)
- [Mark a class as injectable with MonoBehaviourInjectable type](#mark-a-class-as-injectable-with-monobehaviourinjectable-type)
- [NixInjector : Injector used in Play Mode scene](#nixinjector--injector-used-in-play-mode-scene)
  - [Description](#description)
  - [NixiContainer](#nixicontainer)
    - [Add transient mapping](#add-transient-mapping)
    - [Add singleton mapping](#add-singleton-mapping)
    - [Add singleton mapping with implementation](#add-singleton-mapping-with-implementation)
    - [Check if a mapping is registered](#check-if-a-mapping-is-registered)
    - [Resolve mapping](#resolve-mapping)
    - [Remove mapping](#remove-mapping)
    - [Register container mappings before scene load](#register-container-mappings-before-scene-load)
    - [Use NixInjectFromContainer to inject a field from container registrations](#use-nixinjectfromcontainer-to-inject-a-field-from-container-registrations)
  - [Component Injection](#component-injection)
    - [Implications of injection on Awake](#implications-of-injection-on-awake)
  - [Single component injection](#single-component-injection)
    - [NixInjectComponent](#nixinjectcomponent)
    - [NixInjectComponentFromChildren](#nixinjectcomponentfromchildren)
    - [NixInjectComponentFromParent](#nixinjectcomponentfromparent)
    - [NixInjectRootComponent](#nixinjectrootcomponent)
    - [NixInjectRootComponent with only rootGameObjectName passed as parameter](#nixinjectrootcomponent-with-only-rootgameobjectname-passed-as-parameter)
    - [NixInjectRootComponent with rootGameObjectName and subGameObjectName passed as parameters](#nixinjectrootcomponent-with-rootgameobjectname-and-subgameobjectname-passed-as-parameters)
    - [Using interface instead of components](#using-interface-instead-of-components)
  - [Multiple components injection](#multiple-components-injection)
    - [NixInjectComponents](#nixinjectcomponents)
    - [NixInjectComponentsFromChildren](#nixinjectcomponentsfromchildren)
    - [NixInjectComponentsFromParent](#nixinjectcomponentsfromparent)
    - [Using interface instead of components (Multiple components injection)](#using-interface-instead-of-components-multiple-components-injection)
  - [Special cases](#special-cases)
    - [SerializeField](#serializefield)
    - [Multiple Nixi attributes](#multiple-nixi-attributes)
    - [GameObject VS Component](#gameobject-vs-component)
    - [The difference with Unity dependency injection in multiple components injection](#the-difference-with-unity-dependency-injection-in-multiple-components-injection)
    - [Multiple scenes](#multiple-scenes)
- [TestInjector : Injector used for Tests](#testinjector--injector-used-for-tests)
  - [Description](#description-1)
  - [Recursion in TestInjector](#recursion-in-testinjector)
  - [Using TestInjector](#using-testinjector)
    - [Setting up your Unity project for testing](#setting-up-your-unity-project-for-testing)
    - [In newly created Unity project that has no configuration for testing (full tutorial)](#in-newly-created-unity-project-that-has-no-configuration-for-testing-full-tutorial)
  - [Fields accesses](#fields-accesses)
    - [Mock on NixInjectFromContainer and SerializeField decorators](#mock-on-nixinjectfromcontainer-and-serializefield-decorators)
    - [Single component injection decorators](#single-component-injection-decorators)
      - [Fields with type derived from component](#fields-with-type-derived-from-component)
      - [Fields with type derived from interface](#fields-with-type-derived-from-interface)
    - [Multiple components injection decorators](#multiple-components-injection-decorators)
      - [Enumerable fields with "enumerable type" derived from component](#enumerable-fields-with-enumerable-type-derived-from-component)
      - [Enumerable fields with "enumerable type" derived from interface](#enumerable-fields-with-enumerable-type-derived-from-interface)
  - [InjectableTestTemplate](#injectabletesttemplate)
    - [How to use InjectableTestTemplate](#how-to-use-injectabletesttemplate)
  - [Tests with single component](#tests-with-single-component)
    - [Get a single component with type](#get-a-single-component-with-type)
    - [Get a single component with type and name](#get-a-single-component-with-type-and-name)
    - [More informations about GetComponent](#more-informations-about-getcomponent)
      - [NixInjectComponentFromParent and NixInjectComponentFromChildren decorators](#nixinjectcomponentfromparent-and-nixinjectcomponentfromchildren-decorators)
      - [NixInjectComponent decorator](#nixinjectcomponent-decorator)
      - [NixInjectRootComponent with only rootGameObjectName parameter decorator](#nixinjectrootcomponent-with-only-rootgameobjectname-parameter-decorator)
      - [NixInjectRootComponent with rootGameObjectName and subGameObjectName parameters decorator](#nixinjectrootcomponent-with-rootgameobjectname-and-subgameobjectname-parameters-decorator)
      - [NixInjectRootComponent, special case where you want your MainTested instance to be recognized as a root GameObject](#nixinjectrootcomponent-special-case-where-you-want-your-maintested-instance-to-be-recognized-as-a-root-gameobject)
  - [Tests with multiple components](#tests-with-multiple-components)
    - [Fields decorated with a Nixi attribute for multiple components fields](#fields-decorated-with-a-nixi-attribute-for-multiple-components-fields)
    - [InitEnumerableComponents](#initenumerablecomponents)
    - [InitEnumerableComponentsWithTypes](#initenumerablecomponentswithtypes)
    - [InitSingleEnumerableComponents](#initsingleenumerablecomponents)
    - [GetEnumerableComponents](#getenumerablecomponents)
  - [Tests with mockable fields](#tests-with-mockable-fields)
    - [Inject and read fields](#inject-and-read-fields)
    - [Inject field for NixInjectFromContainer or SerializeField](#inject-field-for-nixinjectfromcontainer-or-serializefield)
    - [Mock single component field decorating an interface (e.g : NixInjectComponent)](#mock-single-component-field-decorating-an-interface-eg--nixinjectcomponent)
    - [Mock multiple components field having an interface as enumerable type (e.g : NixInjectComponents)](#mock-multiple-components-field-having-an-interface-as-enumerable-type-eg--nixinjectcomponents)
  - [Details of other features of InjectableTestTemplate](#details-of-other-features-of-injectabletesttemplate)
    - [InstanceName property](#instancename-property)
    - [ResetTemplate method](#resettemplate-method)
    - [SetTemplateWithConstructor property](#settemplatewithconstructor-property)
    - [Using AbstractComponentMappingContainer](#using-abstractcomponentmappingcontainer)
    - [Template with NUnit framework](#template-with-nunit-framework)
  - [Special cases](#special-cases-1)
    - [Multiple Transform components](#multiple-transform-components)
- [Support](#support)

# Mark a class as injectable with MonoBehaviourInjectable type
**To use Nixi, you have to inherit your classes from MonoBehaviourInjectable instead of MonoBehaviour.**

MonoBehaviourInjectable class was designed to call *CheckAndInjectAll* method during *Awake* call. The purpose of this method is to inject all fields decorated with Nixi attributes.

> The test template (InjectableTestTemplate) uses a generic type which has for constraint to be a class derived from MonoBehaviourInjectable. 
> This is because this template can reuse all decorators to simulate Unity and simplify test writing.

**An injector (NixInjector or TestInjector) can call CheckAndInjectAll() only once.**

If you want to use *Awake* in your code and keep the same behaviour, you can override it and call *base.Awake()*.
```cs
public class Player : MonoBehaviourInjectable
{
    // If you want to use Awake you can override it
    // and call base.Awake() to apply injection
    protected override void Awake()
    {
        // My logic before injection

        base.Awake();

        // My logic after injection
    }
}
```
If you want to apply injection at another time, you have to :
- Override *Awake* method to not call *CheckAndInjectAll*.
- Then you can and use a NixInjector manually like below.
```cs
public class Player : MonoBehaviourInjectable
{
    // Canceling build injection on Awake
    protected override void Awake()
    {
    }

    // Build injection at Start
    private void Start()
    {
        // injector.IsInjected = false
        NixInjector injector = new NixInjector(this);

        // injector.IsInjected = true, cannot be called twice
        injector.CheckAndInjectAll();
    }
}
```

# NixInjector : Injector used in Play Mode scene

## Description

This class performs the injections when the scene is launched in play mode.

It is instantiated and injected during Awake method call in MonoBehaviourInjectable.

## NixiContainer

This is a simple IOC container in which each mapping must have an interface as key type.

NixiContainer being a static class means that it is not limited to a scene but to the entire "code execution".

> **_Exception(s)_**<br/>
> If the key type is not an interface, an exception will be thrown.

It has the following features :
  - [Add transient mapping](#add-transient-mapping)
  - [Add singleton mapping](#add-singleton-mapping)
  - [Add singleton mapping with implementation](#add-singleton-mapping-with-implementation)
  - [Check if a mapping is registered](#check-if-a-mapping-is-registered)
  - [Resolve mapping](#resolve-mapping)
  - [Remove mapping](#remove-mapping)

You can initialize NixiContainer and use it by following :
  - [Register container mappings before scene load](#register-container-mappings-before-scene-load)
  - [Use NixInjectFromContainer to inject a field from container registrations](#use-nixinjectfromcontainer-to-inject-a-field-from-container-registrations)
  
### Add transient mapping

You can add a mapping with "transient scope", this scope means that **a new instance is created each time you resolve its mapping** like in the example below.

You can't map an "implementation class" inherited from Component because I didn't want NixiContainer to handle GameObject building (maybe this should be changed).

Instead, I implemented a solution to be able to map this type of implementation by passing an implementation from the scene.

You can read how to use it in [Add singleton mapping with implementation part](#add-singleton-mapping-with-implementation).

> By default there is no need to pass the "constructorParameters" argument, if so the container will use the default constructor to construct the implementation. But you can use different constructor by filling as many parameter as you want in "constructorParameters".

```cs
public void AddMappings()
{
    // Register link between ISimpleInterface and SimpleImplementation as transient
    NixiContainer.MapTransient<ISimpleInterface, SimpleImplementation>();

    // Resolve create an instance
    ISimpleInterface instance = NixiContainer.ResolveMap<ISimpleInterface>();

    // Resolve create a different instance
    ISimpleInterface differentInstance = NixiContainer.ResolveMap<ISimpleInterface>();
}
```

### Add singleton mapping

You can add a mapping with "singleton scope", this scope means that **the first mapping resolution call creates the instance and that every subsequent call always returns that same instance** like in the example below.

You can't map an "implementation class" inherited from Component because I didn't want NixiContainer to handle GameObject building (maybe this should be changed).

Instead, I implemented a solution to be able to map this type of implementation by passing an implementation from the scene. You can read how to use it in [Add singleton mapping with implementation part](#add-singleton-mapping-with-implementation).

> By default there is no need to pass the "constructorParameters" argument, if so the container will use the default constructor to construct the implementation.<br/>
But you can use different constructor by filling as many parameter as you want in "constructorParameters".
```cs
public void AddMappings()
{
    // Register link between ISimpleInterface and SimpleImplementation as singleton
    NixiContainer.MapSingleton<ISimpleInterface, SimpleImplementation>();

    // First resolution create a new instance
    ISimpleInterface instance = NixiContainer.ResolveMap<ISimpleInterface>();

    // Subsequent resolutions always return the same instance as the first resolution call
    ISimpleInterface sameInstance = NixiContainer.ResolveMap<ISimpleInterface>();
}
```

### Add singleton mapping with implementation

Like in [Add singleton mapping](#add-singleton-mapping) part, you can add a mapping with "singleton scope".<br/>
But this time you have to pass an implementation directly, it means that **every mapping resolution call will return the implementation you passed before and NixiContainer will not handle its construction**.

You can see an example below.

> With this method, you can map every type of implementation class, including Component. <br/>
> This was also created to allow passing implementations from the scene.

```cs
[SerializeField]
private MonoBehaviourImplementation PrefabFromTheScene;

public void AddMappings()
{
    // Register link between ISimpleInterface and MonoBehaviourImplementation as singleton
    // The prefab to get from resolution is passed into this mapping
    NixiContainer.MapSingletonWithImplementation<ISimpleInterface, MonoBehaviourImplementation>(PrefabFromTheScene);

    // All resolutions return PrefabFromTheScene instance
    ISimpleInterface instance = NixiContainer.ResolveMap<ISimpleInterface>();
}
```

### Check if a mapping is registered

You can check if a mapping is already registered with the command shown in the example below.

```cs
public void DoLogic()
{
    // false
    bool isRegistered = NixiContainer.CheckIfMappingRegistered<ISimpleInterface>();

    NixiContainer.MapSingleton<ISimpleInterface, SimpleImplementation>();

    // true
    isRegistered = NixiContainer.CheckIfMappingRegistered<ISimpleInterface>();
}
```

### Resolve mapping

After adding one or more mappings in NixiContainer, you can retrieve an instance from a key type (interface) as shown in the example below.

> This example does not cover all scenarios in terms of scope, if you want to know more about registration scopes you can read these parts :
  >- [Add transient mapping](#add-transient-mapping)
  >- [Add singleton mapping](#add-singleton-mapping)
  >- [Add singleton mapping with implementation](#add-singleton-mapping-with-implementation)

```cs
public void DoLogic()
{
    // Register link between ISimpleInterface and SimpleImplementation as transient
    NixiContainer.MapTransient<ISimpleInterface, SimpleImplementation>();

    // Resolve create an instance
    ISimpleInterface instance = NixiContainer.ResolveMap<ISimpleInterface>();
}
```

### Remove mapping

You can remove a mapping with the command shown in the example below.

```cs
public void DoLogic()
{
    // Add mapping
    NixiContainer.MapSingleton<ISimpleInterface, SimpleImplementation>();

    // Remove mapping
    NixiContainer.RemoveMap<ISimpleInterface>();
}
```
### Register container mappings before scene load

To load all NixiContainer configurations before all other scripts, there are several possible approaches.

I use one of the following :
    
  - You can create a static class with *RuntimeInitializeOnLoadMethod* decorator to choose when it should be call (no need to have a script in the scene) like in the example below.

    ```cs
    public static class ContainerConfiguration
    {
        /// <summary>
        /// Create all mappings between interfaces and implementations used in NixiContainer after that assemblies are loaded
        /// You can change the RuntimeInitializeLoadType to best suit your situation
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void LinkFieldInjections()
        {
            NixiContainer.MapSingleton<ISimpleInterface, SimpleImplementation>();
        }
    }
    ```
  - Or you can create a MonoBehaviour script like this one :
    ```cs
    public class ContainerInitiliazer : MonoBehaviour
    {
        /// <summary>
        /// Create all mappings between interfaces and implementations used in NixiContainer after that assemblies are loaded
        /// </summary>
        void Awake()
        {
            NixiContainer.MapSingleton<ISimpleInterface, SimpleImplementation>();
        }
    }
    ```
    Then you can attached it to a GameObject in the scene and change the Script Execution Order located in :

            -> Edit in the menu bar
                -> Project Settings
                    -> Script Execution Order (tab)
        
    Lastly, you can add your script in the list and drag it to have it before "Default time", this will ensure it will be called before your scripts when the scene is loaded.

### Use NixInjectFromContainer to inject a field from container registrations

After registering the mappings, you can inject a field whose type matches a record from the NixInjectFromContainer decorator like in the example below.

```cs
// This class will injects the fields decorated with Nixi attributes
// on Awake() call because it is inherited from MonoBehaviourInjectable
public class Player : MonoBehaviourInjectable
{
    [NixInjectFromContainer]
    private ISimpleInterface instance;
}
```

## Component Injection

[NixiContainer](#nixicontainer) can handle the construction of classes that are not Unity components and register some component with object passed from the scene. But in Play Mode scene Nixi never builds Unity components and we won't store everything in the NixiContainer, especially since we have Unity dependency injection available (with methods like *GetComponent<Slider>()*).

For all Unity components or classes derived from it, or even interfaces implemented on Unity components, there are specific Nixi attributes.

**Nixi attributes for Unity components use Unity dependency injection methods when *Awake* method is called in classes derived from MonoBehaviourInjectable.**

This applies on classes derived from component type and on interfaces implemented on components present in the scene.
 
> Inheritance is taken into account, which means that if you try to fill a field with "Renderer" type, it may find a SpriteRenderer like a MeshRenderer. If you want more precision you can, by example, directly specify the last descendant type.

### Implications of injection on Awake

Fields decorated with Nixi attributes are injected during *Awake()* method call of a MonoBehaviourInjectable, this means :
- That it will be injected only when the MonoBehaviourInjectable is at the beginning of its life cycle in the scene and anything not available at this moment cannot be injected with Nixi later.
- Anything created after cannot update what is already initialized/injected into the scene.
  > By example, if a spawner creates a monster, the spawner will have to handle this new instance (this new monster will also have an initialization phase and can have its fields injected if it is MonoBehaviourInjectable but the spawner will not be updated even if by example he had injected one of its fields with all the monsters present in the scene during its initialization).

## Single component injection

A single component field is a field which has a type derived from Unity component.

Contrary to a [Multiple components injection](#multiple-components-injection), it handle only one instance and not an Enumerable of component instances.

A single component injection is an injection performed in a single component field decorated with one of the Nixi attributes provided for this when calling the *Awake* method of a MonoBehaviourInjectable.

> It can be on a field with an interface type which is implemented on an Unity component present in the scene.
> 
> You can find more information about interface type combined with single component injection [into this part](#using-interface-instead-of-components).

Here is the list of Nixi attributes that handle **single component injection** :

- [NixInjectComponent](#nixinjectcomponent)
- [NixInjectComponentFromChildren](#nixinjectcomponentfromchildren)
- [NixInjectComponentFromParent](#nixinjectcomponentfromparent)
- [NixInjectRootComponent](#nixinjectrootcomponent)

### NixInjectComponent

When a field is decorated with NixInjectComponent, as in the example below, NixInjector will get the single component attached to its own GameObject that match the field type.

> If you want to get multiple components of the same type present on its own GameObject, [you can use NixInjectComponents](#NixInjectComponents).


> **_Exception(s)_**<br/>
> If component not found or multiple are returned, an exception will be thrown.

```cs
public class Character : MonoBehaviourInjectable
{
    // Will call GetComponents<BoxCollider2D> from current GameObject
    // and find the single component with BoxCollider2D type attached to this GameObject
    [NixInjectComponent]
    private BoxCollider2D colliderToInject;
}
```

### NixInjectComponentFromChildren

When a field is decorated with NixInjectComponentFromChildren, as in the example below, NixInjector will get the single component attached to one of its child GameObjects that match the field type (**excluding itself**, which differs from Unity).

If multiple GameObjects with a component of the searched type can be found, you can specify the *gameObjectName* parameter, this will filter GameObjects found to keep the one with that name to find the unique match.

> If you want to get multiple components of same type in children, [you can use NixInjectComponentsFromChildren](#NixInjectComponentsFromChildren).

> **_Exception(s)_**<br/>
> If component not found or multiple are returned, an exception will be thrown.

```cs
public class Character : MonoBehaviourInjectable
{
    // Will call GetComponentsInChildren<BoxCollider2D> from the current GameObject 
    // and find the single BoxCollider2D type component attached to any of its child GameObjects (excluding itself)
    [NixInjectComponentFromChildren]
    private BoxCollider2D colliderToInject;
}
```

### NixInjectComponentFromParent

When a field is decorated with NixInjectComponentFromParent, as in the example below, NixInjector will get the single component attached to one of its parent GameObjects that match the field type (**excluding itself**, which differs from Unity).

If multiple GameObjects with a component of the searched type can be found, you can specify the gameObjectName parameter, this will filter GameObjects found to keep the one with that name to find the unique match.

> If you want to get multiple components of same type in parents, [you can use NixInjectComponentsFromParent](#NixInjectComponentsFromParent).

> **_Exception(s)_**<br/>
> If component not found or multiple are returned, an exception will be thrown.

```cs
public class Character : MonoBehaviourInjectable
{
    // Will call GetComponentsInParent<BoxCollider2D> from the current GameObject
    // and find the single BoxCollider2D type component attached to any of its parent GameObjects (excluding itself)
    [NixInjectComponentFromParent]
    private BoxCollider2D colliderToInject;
}
```

### NixInjectRootComponent

In Unity, a **root GameObject** is a GameObject that is at the first level of the scene (top most).

It has no parents and is often harder to access from Nixi attributes, especially if you're looking for a GameObject from a GameObject that can't find it in its parents (because it's a descendant of another root GameObject or a root GameObject directly).

That's why **NixInjectRootComponent** was created.

> In this documentation, a **root component** is a component attached to a root GameObject or attached to one of its child GameObject.

NixInjector will get the single root component which match the field type and parameter(s) :
  - Required first parameter [rootGameObjectName](#nixinjectrootcomponent-with-only-rootgameobjectname-passed-as-parameter).
  - Optional second parameter [subGameObjectName](#nixinjectrootcomponent-with-rootgameobjectname-and-subgameobjectname-passed-as-parameters).

### NixInjectRootComponent with only rootGameObjectName passed as parameter

When a field is decorated with NixInjectRootComponent with only rootGameObjectName parameter filled, as in the example below, NixInjector will : 
  - Get the unique root GameObject which has a name equal to rootGameObjectName.
  - And from that root GameObject it will find the single component attached to this root GameObject that match the field type.

> There is no Nixi attribute provided to fetch multiple components from root GameObjects, this is to avoid more expensive method calls (from my perspective if I wanted to fetch all children of a type descended from a root GameObject, I will give access and responsibility to these child components to a component attached to the root GameObject)

> **_Exception(s)_**<br/>
> - If root GameObject not found or multiple are returned, an exception will be thrown.
> - If component not found or multiple are returned, an exception will be thrown.

```cs
public class Character : MonoBehaviourInjectable
{
    // Will call UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()
    // then find the single root GameObject named "rootGameObjectName"
    // and find the single component with BoxCollider2D type attached to this root GameObject
    [NixInjectRootComponent("rootGameObjectName")]
    private BoxCollider2D colliderToInject;
}
```

### NixInjectRootComponent with rootGameObjectName and subGameObjectName passed as parameters

When a field is decorated with NixInjectRootComponent with rootGameObjectName and subGameObjectName parameters filled, as in the example below, NixInjector will :
  - Get the unique root GameObject which has a name equal to rootGameObjectName.
  - And from that root GameObject it will find the single child GameObject which has name equals to subGameObjectName.
  - And from that subGameObject it will find the single component attached that match the field type.

> There is no Nixi attribute provided to fetch multiple components from root GameObjects, this is to avoid more expensive method calls (from my perspective if I wanted to fetch all children of a type descended from a root GameObject, I will give access and responsibility to these child components to a component attached to the root GameObject)

> **_Exception(s)_**<br/>
> - If root GameObject not found or multiple are returned, an exception will be thrown.
> - If subGameObject not found or multiple are returned, an exception will be thrown.
> - If component not found or multiple are returned, an exception will be thrown.

```cs
public class Character : MonoBehaviourInjectable
{
    // Will call UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()
    // then find the single root GameObject named "rootGameObjectName"
    // then find the single child GameObject named "subGameObjectName"
    // and find the single component with BoxCollider2D type attached to this root GameObject
    [NixInjectRootComponent("rootGameObjectName", "subGameObjectName")]
    private BoxCollider2D colliderToInject;
}
```

### Using interface instead of components

For each component injection, it is possible to use an interface instead of a component.

To do this, you have to
  - Implement an interface on a MonoBehaviour script.
  - Attach this script to a GameObject presents in the scene.
  - Decorate the field (which has the type of the interface) in which you would like to inject the target component from a Nixi attribute.

> Unity dependendy injection can retrieve components as well as components implementing interfaces present in the scene using the same methods, which is why Nixi can also do it.

You can see an example below.

```cs
// If a GameObject has an Hero component and a LifeBar component attached to it,
// the LifeBar component will be injected into the lifeBar variable in the Hero component.
// This variable will therefore depend on an abstraction and not on an implementation.

public interface ILifeBar
{
    int HealthPoints { get; }
}

public class LifeBar : MonoBehaviour, ILifeBar
{
    public int HealthPoints => 123;
}

public class Hero : MonoBehaviourInjectable
{
    [NixInjectComponent]
    private ILifeBar lifeBar;
}
```

## Multiple components injection

A multiple components field is a field that is a List, an IEnumerable or an array where the enumerable type is a type derived from Unity component.

Contrary to a [Single component injection](#single-component-injection), it can handle one or several instance(s).

A multiple components injection is an injection performed in a multiple components field decorated with one of the Nixi attributes provided for this when calling the *Awake* method of a MonoBehaviourInjectable.

> It can be on a field that a List, an IEnumerable or an array where the enumerable type is an interface type which is implemented on one or several Unity component(s) present in the scene.
> 
> You can find more information about interface type combined with multiple component(s) injection [into this part](#using-interface-instead-of-components-multiple-components-injection).

Here is the list of Nixi attributes that handle **multiple components injection** :

- [NixInjectComponents](#nixinjectcomponents)
- [NixInjectComponentsFromChildren](#nixinjectcomponentsfromchildren)
- [NixInjectComponentsFromParent](#nixinjectcomponentsfromparent)

### NixInjectComponents

When a field is decorated with NixInjectComponents, as in the example below, NixInjector will get all the components attached to its own GameObject that match the "enumerable type".

> If you want to get the only component of a type present on its own GameObject, [you can use NixInjectComponent](#NixInjectComponent).

> **_Exception(s)_**<br/>
> It can only decorate IEnumerables, Lists and arrays, otherwise an exception will be thrown.


```cs
// In this example, all fields have the same values,
// because they are all at the same level (GetComponents)
public class Character : MonoBehaviourInjectable
{
    // Will call GetComponents on the current GameObject
    // and find all components of type Weapon attached to this GameObject
    [NixInjectComponents]
    private List<Weapon> weaponList;

    // Will call GetComponents on the current GameObject
    // and find all components of type Weapon attached to this GameObject
    [NixInjectComponents]
    private IEnumerable<Weapon> weaponEnumerable;

    // Will call GetComponents on the current GameObject
    // and find all components of type Weapon attached to this GameObject
    [NixInjectComponents]
    private Weapon[] weaponArray;
}
```

### NixInjectComponentsFromChildren

When a field is decorated with NixInjectComponentsFromChildren, as in the example below, NixInjector will get all the components attached to its child GameObjects (**excluding itself** , which differs from Unity) that match the "enumerable type".

> If you want to get the only component of a type present in its own child GameObjects, [you can use NixInjectComponentFromChildren](#NixInjectComponentFromChildren).

> **_Exception(s)_**<br/>
> It can only decorate IEnumerables, Lists and arrays, otherwise an exception will be thrown.

```cs
// In this example, all fields have the same values,
// because they are all at the same level (GetComponentsInChildren)
public class Character : MonoBehaviourInjectable
{
    // Will call GetComponentsInChildren on the current GameObject
    // and find all components of type Weapon attached to its child GameObjects (excluding itself)
    [NixInjectComponentsFromChildren]
    private List<Weapon> weaponList;

    // Will call GetComponentsInChildren on the current GameObject
    // and find all components of type Weapon attached to its child GameObjects (excluding itself)
    [NixInjectComponentsFromChildren]
    private IEnumerable<Weapon> weaponEnumerable;

    // Will call GetComponentsInChildren on the current GameObject
    // and find all components of type Weapon attached to its child GameObjects (excluding itself)
    [NixInjectComponentsFromChildren]
    private Weapon[] weaponArray;
}
```
### NixInjectComponentsFromParent

When a field is decorated with NixInjectComponentsFromParent, as in the example below, NixInjector will get all the components attached to its parent GameObjects (**excluding itself** , which differs from Unity) that match the "enumerable type".

> If you want to get the only component of a type present in its own parent GameObjects, [you can use NixInjectComponentFromParent](#NixInjectComponentFromParent).

> **_Exception(s)_**<br/>
> It can only decorate IEnumerables, Lists and arrays, otherwise an exception will be thrown.

```cs
// In this example, all fields have the same values,
// because they are all at the same level (NixInjectComponentsFromParent)
public class Character : MonoBehaviourInjectable
{
    // Will call NixInjectComponentsFromParent on the current GameObject
    // and find all components of type Weapon attached to its parent GameObjects (excluding itself)
    [NixInjectComponentsFromParent]
    private List<Weapon> weaponList;

    // Will call NixInjectComponentsFromParent on the current GameObject
    // and find all components of type Weapon attached to its parent GameObjects (excluding itself)
    [NixInjectComponentsFromParent]
    private IEnumerable<Weapon> weaponEnumerable;

    // Will call NixInjectComponentsFromParent on the current GameObject
    // and find all components of type Weapon attached to its parent GameObjects (excluding itself)
    [NixInjectComponentsFromParent]
    private Weapon[] weaponArray;
}
```

### Using interface instead of components (Multiple components injection)

The same rules apply on interfaces usage for multiple components injection as [interface usage on single component injection](#using-interface-instead-of-components).

Instead of a single interface, you will deal with an IEnumerable, a List or an array with enumerable type equals to the interface type you are targeting.

## Special cases

### SerializeField

You cannot use a Nixi attribute with a SerializeField attribute on the same field, because SerializeField means you are passing the value directly from the scene.

> **If this is the case the injector will throw an exception.**

### Multiple Nixi attributes

You cannot use multiple Nixi attributes on the same field because that wouldn't really make sense.

> **If this is the case the injector will throw an exception.**

### GameObject VS Component

A GameObject is not a component, that means, you can't inject it directly into a MonoBehaviourInjectable.

If you want to access it, you can use the Transform type for these two reasons :
  - It is an Unity component.
  - It contains the GameObject you want to inject.

### The difference with Unity dependency injection in multiple components injection

Like in Unity, when we use NixInjectComponents, only the components on the GameObject that calls the method are returned.

But for NixInjectComponentsFromChildren and NixInjectComponentsFromParent, I chose to do it differently from Unity : the GameObject that calls the method in the components is removed from the results.

> I wanted to differentiate all thoses cases to avoid confusions and allow to be more precise.
    
> Moreover, in [TestInjector part](#testinjector--injector-used-for-tests), it makes it easier to link transform and transform.parent for the GameObjects with this approach.

### Multiple scenes

Nixi was written with a focus on single scene context, I don't know what it might imply to use it in a multiple scene context.

# TestInjector : Injector used for Tests

## Description

This library also aims to facilitate testing on MonoBehaviourInjectables, which is why a specific injector called **TestInjector** has been created for testing purposes only. It was written, thinking of "edit mode tests" which cover most cases that don't require runtime (frame by frame tests with Awake, Start, Update, etc.).

> It is possible to use TestInjector on tests that need to be played at runtime (play mode tests), the usage is no different, you can adapt it at your convenience.

The Nixi attributes are intended to be used during play mode scene and can also be reused in tests to follow the evolution of these fields and/or mock where it is useful, including non-public fields. These reuses and accesses during testing are intended to simulate and verify Unity context (GetComponent, GetComponentInChildren, etc.), as well as to simulate interfaces that have been decorated for injection from NixiContainer (or decorated with SerializeField).

**This is only allowed for these cases, no others.**

**TestInjector** provides a set of tools that can handle most situations I'm used to seeing to allow focus on testing. These tools help preserve encapsulation, avoid writing workarounds, and rely on Unity dependency injection as well as the more classic dependency injection handled by [NixiContainer](#nixicontainer).

## Recursion in TestInjector

Each time TestInjector creates a component (from the detection of a single component injection or a multiple components injection on which data has been initialized), if this component is a MonoBehaviourInjectable, TestInjector will also inject the fields of this component and this, in a recursive way.

You can apply all of the TestInjector's operations to any of recursively generated components, regardless of the number of recursions that have taken place.

To do this, you have to pass the targetInjectable parameter in the methods you want to use.

> If this is parameter is null, MainTested will be the targetInjectable (this is the default behavior).
>
> This parameter can come from any level of recursivity.

## Using TestInjector

### Setting up your Unity project for testing

If your environment is not configured to use Unity tests you can :

  - Read [this part of Unity documentation](https://docs.unity3d.com/Packages/com.unity.test-framework@2.0/manual/index.html), and :
      - Add **Nixi.dll** and **NixiTestTools.dll** reference to your "Tests" Assembly Definition in "Assembly References" part (below nunit.framework.dll).
      - Probably have to check the box "Override References" in your "Scripts" Assembly Definition to add **Nixi.dll** in "Assembly References" part continue this part.
  - Or you can read [the full tutorial part](#in-newly-created-unity-project-that-has-no-configuration-for-testing-full-tutorial) described in this documentation to do it from scratch.

### In newly created Unity project that has no configuration for testing (full tutorial)

1) If Nixi is not installed in your project, please follow [Getting Started into README page](https://github.com/f-antoine/Nixi#getting-started).
2) Setup your "Scripts" assembly :
   - Create a folder named "Scripts" into your "Assets" folder for your code.
   - Move in this "Scripts" folder and create a simple script to test your environment like this :
        ```cs
        using Nixi.Injections;
        public class ClassToTest : MonoBehaviourInjectable { }
        ```
   - Still in "Scripts" folder :

            -> Right click
                -> Click on Create / Assembly Definition
                    -> Name it "Scripts"
   - In this newly created Assembly Definition, if you have some issues about Nixi referencing, you can specify these references as follows :
     - Check the box "Override References"
     - In "Assembly References" part, add a reference to **Nixi.dll**

3) Setup your "Tests" assembly :
   - Move into your "Assets" folder, then :
   
            -> Click on Window in menu bar
                -> Move into General
                    -> Click on Test runner
   - Click on "Create EditMode Test Assembly Folder", validate the newly created "Tests" folder and move into this "Tests" folder.
   - Click on "Tests" assembly definition (with the puzzle piece icon)
     - In "Assembly Definition References" part, add the "Scripts" assembly definition.
     - In "Assembly References" part, add **Nixi.dll** and **Nixi.TestTools** (next to nunit.framework.dll).
   - Still in "Tests" folder and from "Test Runner" window, click on : "Create Test Script in current folder".
   - Name it the way you want and open it, in this script you can write your first test like this :
        ```cs
        using NixiTestTools;
        using NUnit.Framework;

        public class ClassTests : InjectableTestTemplate<ClassToTest>
        {
            [Test]
            public void TestsWorking()
            {
                Assert.NotNull(MainTested);
                Assert.NotNull(MainInjector);
            }
        }
        ```

4) To check if everything is correctly setup :
   - Return into the Test Runner window, the test should be displayed.
   - Play this test and if there is no error and the result is green, it means your environment is setup.

> If you have any issue using assembly definition in Unity or if you want more informations, you can check into this link : https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html

## Fields accesses

TestInjector allows access to fields decorated with Nixi attributes or with SerializeField.

These accesses are allowed on public, protected and private fields at all levels of inheritance.

> You can skip the parts about design choices about field accesses and go straight to the [more concrete parts with code examples](#injectabletesttemplate).

The reasons these field accesses are allowed are described along the parts :

- [Mock on NixInjectFromContainer and SerializeField decorators](#mock-on-nixinjectfromcontainer-and-serializefield-decorators)
- [Single component injection decorators](#single-component-injection-decorators)
- [Multiple components injection decorators](#multiple-components-injection-decorators)
 

### Mock on NixInjectFromContainer and SerializeField decorators

If you are not familiar with NixInjectFromContainer, [please read this part](#use-nixinjectfromcontainer-to-inject-a-field-from-container-registrations).

The fields decorated with NixInjectFromContainer or SerializeField are not filled from TestInjector, you will have to mock it and their values will be readable.

> The reasons for being able to access decorated fields with **NixInjectFromContainer** are :
> - It's not the same environment, so by default, the NixiContainer configuration is not loaded.
> - You can isolate each test situation, mock fields (or use an implementation) and focus on what is important.

> The reasons for being able to access fields decorated with **SerializeField** are :
> - TestInjector responds to test categories that do not need to be linked to a scene (for play mode tests, the situation could be different).
> - If these fields are not public, they cannot be filled in or read. Especially since the code of the classes must remain independent of the tests and the encapsulation must not be bypassed in order to be able to test.

### Single component injection decorators

If you are not familiar with single component decorators, [please read this part](#single-component-injection).

#### Fields with type derived from component

If a field with a type derived from component is decorated with a Nixi attribute used for single component injection, TestInjector will create a new instance of this type during *CheckAndInjectAll* method call and will fill the field with this newly created instance.

> The code used during instantiation is : new GameObject().AddComponent\<FieldType>().

This instance will be accessible from TestInjector (as described [in this part](#tests-with-single-component)).

> If this new instance has a type derived from MonoBehaviourInjectable it will inject its fields as well, see [recursion part for informations](#recursion-in-testinjector).

#### Fields with type derived from interface

If a Nixi attribute is used for single component injection on a field with an interface type, this will make this field mockable like in [Mock on NixInjectFromContainer and SerializeField decorators part](#mock-on-nixinjectfromcontainer-and-serializefield-decorators).

These fields are not filled from TestInjector, you will have to mock them and their values will be readable.

> There isn't really a way or even a reason to instantiate a GameObject that would implement an interface automatically from TestInjector based on the interface only. The purpose is to not depend on the implementation, it is easier to mock an interface than a GameObject and if you want you can pass an implementation.

### Multiple components injection decorators

If you are not familiar with multiple components decorators, [please read this part](#multiple-components-injection).

#### Enumerable fields with "enumerable type" derived from component

If a field is a List, an IEnumerable or an array with its "enumerable type" derived from component is decorated with a Nixi attribute used for multiple components injection, TestInjector will fill this field with an empty enumerable during *CheckAndInjectAll* method call. 

Some methods allow you to initialize theses fields with a certain number of instances and these fields can be read, you can see how [in this part](#tests-with-multiple-components).

> If the new instances have a type derived from MonoBehaviourInjectable, they will also have their fields injected, see [recursion part for informations](#recursion-in-testinjector).

#### Enumerable fields with "enumerable type" derived from interface

If a Nixi attribute is used for multiple components injection on a field  that is a List, an IEnumerable or an array with an "enumerable type" that is an interface, this will make this field mockable like [Mock on NixInjectFromContainer and SerializeField decorators part](#mock-on-nixinjectfromcontainer-and-serializefield-decorators).

These fields are not filled from TestInjector, you will have to mock them and their values will be readable.

> There isn't really a way or even a reason to instantiate a GameObject that would implement an interface automatically from TestInjector based on the interface only. The purpose is to not depend on the implementation, it is easier to mock an interface than a GameObject and if you want you can always pass an implementation.

## InjectableTestTemplate

A test template is provided to test classes inherited from MonoBehaviourInjectable using TestInjector, but you can do it from scratch.

This template is called **InjectableTestTemplate** and the most importants things are :
- It has a property named **MainTested** which is the instance you are testing.
- It has a property named **MainInjector**, which is the injector (TestInjector) linked to MainTested. <br/>It is useful to access to components, inject mocks, etc.

You can read how to use this template in your tests in the following parts :
  - [How to use InjectableTestTemplate](#how-to-use-injectabletesttemplate)
  - [Tests with single component](#tests-with-single-component)
  - [Tests with multiple components](#tests-with-multiple-components)
  - [Tests with mockable fields](#tests-with-mockable-fields)
  - [Details of other features of InjectableTestTemplate](#details-of-other-features-of-injectabletesttemplate)

> This not necessary to read its source code, but you can find it just below.

```cs
/// <summary>
/// Test template for MonoBehaviourInjectable, it creates an instance of the MonoBehaviourInjectable and use TestInjector 
/// to specially handle dependency injection for testing
/// </summary>
/// <typeparam name="T">MonoBehaviourInjectable</typeparam>
public abstract class InjectableTestTemplate<T>
    where T : MonoBehaviourInjectable
{
    /// <summary>
    /// Instance of the tested MonoBehaviourInjectable
    /// </summary>
    protected T MainTested { get; private set; }

    /// <summary>
    /// Injector implemented for tests, it handle all injections of fields decorated with Nixi inject attributes 
    /// of a class derived from MonoBehaviourInjectable during test execution
    /// <para/>His purpose is to simplify access to fields inexposed and make these fields testables/mockables
    /// </summary>
    protected TestInjector MainInjector { get; private set; }

    /// <summary>
    /// Name of the instance, mainly modified to consider MainTested as a root component
    /// <para/>See documentation part : "NixInjectRootComponent special case when you want your MainTested to be 
    /// recognized as a root gameObject"
    /// </summary>
    protected virtual string InstanceName => "";

    /// <summary>
    /// If true (default value), constructor call will set once MainTested and MainInjector (with method ResetTemplate())
    /// <para/>It was made to avoid NUnit dependency with SetUp decorator
    /// <para/>If you want to ResetTemplate() at each test with NUnit by example,
    /// you have to create a method decorated with [SetUp] and call ResetTemplate() in it
    /// </summary>
    protected virtual bool SetTemplateWithConstructor => true;

    /// <summary>
    /// Each mapping added into this container force a type to be used by its derived form during tests using TestInjector, 
    /// useful when working on abstract component injected with Nixi
    /// </summary>
    protected AbstractComponentMappingContainer ComponentMappingContainer = null;

    /// <summary>
    /// Map a component type with a derived type into ComponentMappingContainer
    /// </summary>
    /// <typeparam name="TAbstract">Component key type</typeparam>
    /// <typeparam name="TImplementation">Implementation type</typeparam>
    protected void AddAbstractComponentMapping<TAbstract, TImplementation>()
        where TAbstract : Component
        where TImplementation : class, TAbstract
    {
        ComponentMappingContainer = ComponentMappingContainer ?? new AbstractComponentMappingContainer();
        ComponentMappingContainer.Map<TAbstract, TImplementation>();
    }

    /// <summary>
    /// Create an instance of the MonoBehaviourInjectable to test as well as its TestInjector which allows to expose the fields to 
    /// test / mock with the Nixi approach
    /// <para/>If you want to ResetTemplate() at each test with NUnit by example,
    /// you have to create a method decorated with [SetUp] and call ResetTemplate() in it
    /// </summary>
    protected virtual void ResetTemplate()
    {
        MainTested = new GameObject().AddComponent<T>();

        MainInjector = new TestInjector(MainTested, InstanceName, ComponentMappingContainer);

        MainInjector.CheckAndInjectAll();
    }

    /// <summary>
    /// This constructor was made to avoid NUnit dependency, it will set MainTested instance and its TestInjector
    /// named MainInjector using ResetTemplate() method only once.
    /// <para/>If you want to ResetTemplate() at each test with NUnit by example,
    /// you have to create a method decorated with [SetUp] and call ResetTemplate() in it
    /// You can suppress the first call on ResetTemplate (in this constructor) by setting value on SetTemplateWithConstructor to false
    /// </summary>
    public InjectableTestTemplate()
    {
        if (SetTemplateWithConstructor)
            ResetTemplate();
    }
}
```

### How to use InjectableTestTemplate

To use InjectableTestTemplate for your tests, create a test class that inherits from InjectableTestTemplate with a generic parameter that represents the class you want to test like in the example below.
> The generic parameter (the class you are testing) must have been inherited from MonoBehaviourInjectable.

```cs
public class Sorcerer : MonoBehaviourInjectable
{
    [NixInjectComponent]
    private BoxCollider2D colliderToInject;
}

// Generic type (Sorcerer) must be a class inherited from MonoBehaviourInjectable
public class SorcererTests : InjectableTestTemplate<Sorcerer>
{
    [Test]
    public void TestToExecute()
    {
        // Sorcerer instance was successfully loaded
        Assert.NotNull(MainTested);

        // TestInjector instance was successfully loaded
        Assert.NotNull(MainInjector);
    }
}
```
## Tests with single component

This part describes how to test the fields decorated with Nixi attributes concerning the single component cases.

You can read how to test your code for single component cases in the following parts :
  - [Get a single component with type](#get-a-single-component-with-type)
  - [Get a single component of type with type and name](#get-a-single-component-with-type-and-name)
  - [More informations about GetComponent](#more-informations-about-getcomponent)

### Get a single component with type

To get a single component, you can call GetComponent from the MainInjector and specify which type you want.

```cs
[Test]
public void GetComponent_OnlyWithFieldType()
{
    // Get the unique BoxCollider2D component in Sorcerer class
    // It was filled from TestInjector when the InjectableTestTemplate constructor was called
    BoxCollider2D colliderGenerated = MainInjector.GetComponent<BoxCollider2D>();

    // Check BoxCollider2D was built with TestInjector
    Assert.NotNull(colliderGenerated);
}
```
> **_Exception(s)_**<br/>
> - **If not found an exception will be thrown**
> - **If more than one field with the same type are found in the class, an exception will be thrown**.
> <br/>It means that you have to pass the name of the field in GetComponent like [in this part](#get-a-single-component-with-type-and-name).
### Get a single component with type and name
If several components of the same type are present in the class, **the name of the field** must be passed as a parameter.

```cs
public class Sorcerer : MonoBehaviourInjectable
{
    [NixInjectComponent]
    private BoxCollider2D firstCollider;

    [NixInjectComponentFromChildren("anyChildGameObjectName")]
    private BoxCollider2D secondCollider;
}

[Test]
public void GetComponent_WithFieldTypeAndFieldName()
{
    BoxCollider2D firstCollider = MainInjector.GetComponent<BoxCollider2D>("firstCollider");
    BoxCollider2D secondCollider = MainInjector.GetComponent<BoxCollider2D>("secondCollider");

    Assert.NotNull(firstCollider);
    Assert.NotNull(secondCollider);
    Assert.AreNotEqual(firstCollider.GetInstanceID(), secondCollider.GetInstanceID());
}
```
> **_Exception(s)_**<br/>
> **If not found an exception will be thrown**

### More informations about GetComponent

For the tests, I wanted to reproduce as much as possible the parent / child relationship scheme between the components associated with the Nixi attributes, as well as carry over the names that we are supposed to find.

You can see how it was written in the following parts :
  - [NixInjectComponentFromParent and NixInjectComponentFromChildren decorators](#nixinjectcomponentfromparent-and-nixinjectcomponentfromchildren-decorators)
  - [NixInjectComponent decorator](#nixinjectcomponent-decorator)
  - [NixInjectRootComponent with only rootGameObjectName parameter decorator](#nixinjectrootcomponent-with-only-rootgameobjectname-parameter-decorator)
  - [NixInjectRootComponent with rootGameObjectName and subGameObjectName parameters decorator](#nixinjectrootcomponent-with-rootgameobjectname-and-subgameobjectname-parameters-decorator)
  - [NixInjectRootComponent, special case where you want your MainTested instance to be recognized as a root GameObject](#nixinjectrootcomponent-special-case-where-you-want-your-maintested-instance-to-be-recognized-as-a-root-gameobject)

#### NixInjectComponentFromParent and NixInjectComponentFromChildren decorators

For these decorators, we don't know how many generations there are between a child and the current GameObject (or a parent and the current GameObject), so I decided to :
  - Not to create a link between them (transform.parent).
  - And change the name of the new GameObject to match "gameObjectNameToFind" parameter passed in the decorator.

#### NixInjectComponent decorator

For this decorator, we know this is attached to the same GameObject as the MonoBehaviourInjectable (MainTested) instance, so it has the same GameObject, therefore the same transform and the same name.

#### NixInjectRootComponent with only rootGameObjectName parameter decorator

For this decorator, it means only one root GameObject must have rootGameObjectName name.

That implies that :
- If you have many different types of components decorated with this decorator (with only rootGameObjectName parameter), they all will have the same GameObject but all these fields will have a different component injected.
- If you have many identical types of components decorated with this decorator (with only rootGameObjectName parameter), they all will have the same GameObject and all these fields will have the same component injected.
> If the instance with rootGameObjectName name does not exist, it is created for the tests.<br/>
> If it already exists, the same instance will be used for every field decorated with the same NixInjectRootComponent in MainTested.


> It also works when an injected field has a type derived from MonoBehaviourInjectable and one of its fields has the same NixInjectRootComponent (with same parameters and same type of field). You can see [recursion part for more informations](#recursion-in-testinjector).<br/>
> The code below show an application of this point.

```cs
public class DummyController : MonoBehaviourInjectable
{
    // code
}

public class Example : MonoBehaviourInjectable
{
    // 1) Create an instance of DummyController
    [NixInjectRootComponent("DummyControllerName")]
    private DummyController dummyController;

    // 2) Create an instance of RecursionExample, fill this field with.
    // Then by recursion, the fields in the instance of RecursionExample are also injected 
    // because its type derives from MonoBehaviourInjectable
    // See Recursion part for more informations
    [NixInjectComponent]
    private RecursionExample recursionExample;
}

public class RecursionExample : MonoBehaviourInjectable
{
    // 3) Then it is filled with the same instance as Example.dummyController
    // because Example is the class tested, RecursionExample is built after
    // and have to refer to the rootComponent previously generated
    [NixInjectRootComponent("DummyControllerName")]
    private DummyController dummyController;
}
```

#### NixInjectRootComponent with rootGameObjectName and subGameObjectName parameters decorator

This is the same as for [NixInjectRootComponent with only rootGameObjectName parameter decorator part](#nixinjectrootcomponent-with-only-rootgameobjectname-parameter-decorator), the root GameObject will be instantiated if no instance already exists with the name rootGameObjectName (if it exists, the next operation will be done directly on the existing instance).

Then a GameObject will be instantiated with the name that matches the value contained in subGameObjectName (if it doesn't already exist) with the root GameObject as parent.

It implies :
- The same rules (from [NixInjectRootComponent with only rootGameObjectName parameter decorator part](#nixinjectrootcomponent-with-only-rootgameobjectname-parameter-decorator)) apply on this child GameObject, this is just one level below.
- Same goes for rules of instance crossing between all fields decorated with NixInjectRootComponent in MainTested and recursively from its fields injectables (you can see [recursion part for more informations](#recursion-in-testinjector)).
- Both constructor of NixInjectRootComponent can be combined on the same root GameObject (with name rootGameObjectName).

Code example :
```cs
public class Example : MonoBehaviourInjectable
{
    // 1) Create an instance of AllControllers
    // 2) Create an instance of DummyController which has AllControllers as parent
    [NixInjectRootComponent("AllControllers", "DummyController")]
    private DummyController dummyController;

    // 3) Create an instance of RecursionExample, fill this field with.
    // Then by recursion, the fields of the instance of RecursionExample are also injected 
    // because its type derives from MonoBehaviourInjectable
    // See Recursion part for more informations
    [NixInjectComponent]
    private RecursionExample recursionExample;
}

public class RecursionExample : MonoBehaviourInjectable
{
    // 4) Fill with the same intance as Example.dummyController (which has AllControllers 
    // as parent) because Example is the class tested, RecursionExample is built after
    // and have to refer to the rootComponent previously generated
    [NixInjectRootComponent("AllControllers", "DummyController")]
    private DummyController dummyController;
}
```

#### NixInjectRootComponent, special case where you want your MainTested instance to be recognized as a root GameObject

If you want a field decorated with NixInjectRootComponent to target MainTested as a root component, there is a property in InjectableTestTemplate to specify the name of MainTested instance.

If you do this and a field targets a root component with the name of MainTested, this field will be filled with the instance of MainTested.

```cs
// Class for MainTested
public class MonsterController : MonoBehaviourInjectable
{
    [NixInjectRootComponent("SorcererController")]
    private SorcererController SorcererController;
}

public class SorcererController : MonoBehaviourInjectable
{
    [NixInjectRootComponent("MonsterController")]
    private MonsterController MonsterController;
}

// MainTested = MonsterController instance tested
public class RootTests : InjectableTestTemplate<MonsterController>
{
    // Set an InstanceName allows other components to refer to MainTested 
    // as a root component
    protected override string InstanceName => "MonsterController";

    [Test]
    public void Instances_MustInject_EachOther()
    {
        // Get other controller
        SorcererController sorcererController = MainInjector.GetComponent<SorcererController>();

        // Check correctly named
        Assert.AreEqual("MonsterController", MainTested.name);
        Assert.AreEqual("SorcererController", sorcererController.name);            

        // Check that both refer to each other
        Assert.NotNull(MainTested.SorcererController);
        Assert.AreEqual(MainTested.SorcererController.GetInstanceID(), sorcererController.GetInstanceID());

        Assert.NotNull(sorcererController.MonsterController);
        Assert.AreEqual(sorcererController.MonsterController.GetInstanceID(), MainTested.GetInstanceID()); 
    }
}
```
## Tests with multiple components

This part describes how to test the fields decorated with Nixi attributes concerning the multiple components cases.

You can read how to test your code for multiple components cases in the following parts :
  - [Fields decorated with a Nixi attribute for multiple components fields](#fields-decorated-with-a-nixi-attribute-for-multiple-components-fields)
  - [InitEnumerableComponents](#initenumerablecomponents)
  - [InitEnumerableComponentsWithTypes](#initenumerablecomponentswithtypes)
  - [InitSingleEnumerableComponents](#initsingleenumerablecomponents)
  - [GetEnumerableComponents](#getenumerablecomponents)

### Fields decorated with a Nixi attribute for multiple components fields

For each field decorated with a Nixi attribute for multiple components injection, TestInjector will set these fields as empty and you will have to initialize them.

> This is because there is no way to know how many you want to test based on type only.

It implies :
 - Initializing a component field twice will throw an exception.
 - If you initialize a field with one or more components and if their type is derived from MonoBehaviourInjectable, TestInjector will recursively inject the fields of each instance (you can see [recursion part for more informations](#recursion-in-testinjector)).
 - Is considered initialized, a multiple components field which is not empty.
 - A multi-component field can be at 3 different levels, each level has a separated injection :
   - Current level (itself at current GameObject level)
   - Parent level (NixInjectComponentsFromParent, parent GameObjects excluding itself)
   - Child level (NixInjectComponentsFromChildren, child GameObjects excluding itself)
 - If you initialize several multi-component fields having the same enumerable type and the same level (current, child, parent): they will be filled with the same components.
> As a [reminder](#multiple-components-injection) : a multiple components field is a List/IEnumerable/Array with an enumerable type derived from Component.

### InitEnumerableComponents

You can initialize a field with several components at the same time specifying the number of instances you want to create.

All the components built are returned at the output of the method and if the enumerable type is derived from MonoBehaviourInjectable, TestInjector will recursively inject the fields of each instance (you can see [recursion part for more informations](#recursion-in-testinjector)).

> If initialized at the current level (GameObjectLevel.Current by default), all instances will be attached to the MainTested GameObject (or on the instance recursively injected if the case is encountered).

You can see an example below.

```cs
// Class tested
public class HeroWithMultiLevelWeapon : MonoBehaviourInjectable
{
    [NixInjectComponentsFromChildren] // Children level
    public List<Weapon> ChildrenWeapons;

    [NixInjectComponents] // Current level
    public List<Weapon> Weapons;

    [NixInjectComponentsFromParent] // Parent level
    public List<Weapon> ParentWeapons;
}

public sealed class HeroWithMultiLevelWeaponTests : InjectableTestTemplate<HeroWithMultiLevelWeapon>
{
    [Test]
    public void InitEnumerable_ShouldFillAtDifferentLevels()
    {
        // Init 4 weapons at children level
        IEnumerable<Weapon> childrenWeapons = MainInjector.InitEnumerableComponents<Weapon>(GameObjectLevel.Children, 4);

        // Init 2 weapons at current level (attached to MainTested GameObject)
        IEnumerable<Weapon> weapons = MainInjector.InitEnumerableComponents<Weapon>(2);

        // Init 3 weapons at parent level
        IEnumerable<Weapon> parentWeapons = MainInjector.InitEnumerableComponents<Weapon>(GameObjectLevel.Parent, 3);

        Assert.AreEqual(4, MainTested.ChildrenWeapons.Count);
        Assert.AreEqual(2, MainTested.Weapons.Count);
        Assert.AreEqual(3, MainTested.ParentWeapons.Count);

        // Cannot init twice at any level
        Assert.Throws<TestInjectorException>(() => MainInjector.InitEnumerableComponents<Weapon>(2));
    }
}
```

### InitEnumerableComponentsWithTypes

You can initialize a field with multiple components at the same time by passing one or more types that are of the same type or derived from the enumerable type you want to create.

All the components built are returned at the output of the method and if the enumerable type is derived from MonoBehaviourInjectable, TestInjector will recursively inject the fields of each instance (you can see [recursion part for more informations](#recursion-in-testinjector)).

> If initialized at the current level (GameObjectLevel.Current by default), all instances will be attached to the MainTested GameObject (or on the instance recursively injected if the case is encountered).

You can see an example below.

> For the line with ResetTemplate() method call, you can read what it is for [into this part](#resettemplate-method).

```cs
public abstract class Animal : MonoBehaviour {}
public class Cat : Animal {}
public class Dog : Animal {}

public class Farm : MonoBehaviourInjectable
{
    [NixInjectComponentsFromParent]
    public List<Cat> CatsParent;

    [NixInjectComponentsFromParent]
    public List<Animal> AnimalsParent;

    [NixInjectComponents]
    public List<Cat> Cats;

    [NixInjectComponents]
    public List<Animal> Animals;
}

public class FarmTests : InjectableTestTemplate<Farm>
{
    // Force MainTested to be rebuilt at each test,
    // see part concerning ResetTemplate to have more explanations
    [SetUp]
    public void InitTests()
    {
        ResetTemplate();
    }

    [Test]
    public void InitEnumerableComponentsWithTypes_ShouldLoadType_AndParentType()
    {
        // Init 2 cat at parent level
        IEnumerable<Cat> cats = MainInjector.InitEnumerableComponentsWithTypes<Cat>(GameObjectLevel.Parent, typeof(Cat), typeof(Cat));

        // At current level, nothing is instantiated
        Assert.IsEmpty(MainTested.Cats);
        Assert.IsEmpty(MainTested.Animals);

        // Parent level (GameObjectLevel.Parent parameter targeting NixInjectComponentsFromParent decorator in InitEnumerableComponentsWithTypes)
        Assert.AreEqual(2, MainTested.CatsParent.Count);

        // It work over inheritance
        Assert.AreEqual(2, MainTested.AnimalsParent.Count);
    }

    [Test]
    public void InitEnumerableComponentsWithTypes_ShouldOnlyLoadType_AndNotParentType()
    {
        // Init a cat and a dog at current level (attached to MainTested GameObject)
        // This line is equivalent to :
        // IEnumerable<Animal> animals = MainInjector.InitEnumerableComponentsWithTypes<Animal>(GameObjectLevel.Current, typeof(Cat), typeof(Dog));
        IEnumerable<Animal> animals = MainInjector.InitEnumerableComponentsWithTypes<Animal>(typeof(Cat), typeof(Dog));
        
        // At parent level, nothing is instantiated
        Assert.IsEmpty(MainTested.CatsParent);
        Assert.IsEmpty(MainTested.AnimalsParent);

        // Empty because Animal type is not Cat type
        // Generic type of InitEnumerableComponentsWithTypes<Animal> define 
        // which enumerable type is targeted on fields decorated with NixInjectComponents
        Assert.IsEmpty(MainTested.Cats);

        // Cat and Dog are Animal and InitEnumerableComponentsWithTypes<Animal> target
        // fields with enumerable type "Animal"
        Assert.AreEqual(2, MainTested.Animals.Count);
    }
}
```

### InitSingleEnumerableComponents

It is possible to initialize a field with only one component into a multiple components field.
The method returns this instance and not an IEnumerable.

If this component is derived from MonoBehaviourInjectable, TestInjector will recursively inject all its fields (you can see [recursion part for more informations](#recursion-in-testinjector)).

> If initialized at the current level (GameObjectLevel.Current by default), all instances will be attached to the MainTested GameObject (or on the instance recursively injected if the case is encountered).

You can see an example below.

```cs
[Test]
public void SingleInit_InMultiComponent()
{
    // Init single cat at current level (attached to MainTested GameObject)
    // This line is equivalent to :
    // Cat cat = MainInjector.InitSingleEnumerableComponent<Cat>(GameObjectLevel.Current);
    Cat cat = MainInjector.InitSingleEnumerableComponent<Cat>();

    // At parent level, nothing is instantiated
    Assert.IsEmpty(MainTested.CatsParent);
    Assert.IsEmpty(MainTested.AnimalsParent);

    // Only one instantiated
    Assert.AreEqual(1, MainTested.Cats.Count);
    Assert.AreEqual(1, MainTested.Animals.Count);

    Assert.AreEqual(cat.GetInstanceID(), MainTested.Cats[0].GetInstanceID());
    Assert.AreEqual(cat.GetInstanceID(), MainTested.Animals[0].GetInstanceID());
}
```

### GetEnumerableComponents

Although you can initialize a field of multiple components with multiple values and retrieve them from the initialization method, you may want to re-access a field's components to see the new values if, by example, a method instantiates or destroys an element of this same list.

The initialization is mainly used to have an original state to test, but you can see the values inside at any time like in the example below.

> The approach is like [GetComponent with type](#get-a-single-component-with-type) and [GetComponent with type and name](#get-a-single-component-with-type-and-name) from TestInjector, you don't have to precise the level (current, parent, child) but only the type if there is only one multiple components field which has its enumerable type equal to the type you are looking for.

> If there is multiple result, you have to pass the name of the field as parameter to find the one you are targeting.

```cs
public class Monster : MonoBehaviour {}

public class Spawner : MonoBehaviourInjectable
{
    [NixInjectComponents]
    public List<Monster> Monsters;

    public Monster Spawn()
    {
        Monster newMonster = new GameObject().AddComponent<Monster>();

        Monsters.Add(newMonster);

        return newMonster;
    }
}

[Test]
public void Initialisation_ThenSpawn_ShouldReturnAllValues()
{
    // Init 2 monsters
    List<Monster> monstersInjected = MainInjector.InitEnumerableComponents<Monster>(2).ToList();

    // 3 monsters now
    Monster monsterAddedFromClass = MainTested.Spawn();

    // Get all monsters (first 2 initialized + the last which spawned from the previous line)
    List<Monster> allMonsters = MainInjector.GetEnumerableComponents<Monster>().ToList();
    
    Assert.AreEqual(3, allMonsters.Count());
    Assert.AreEqual(3, MainTested.Monsters.Count);

    Assert.AreEqual(monstersInjected[0].GetInstanceID(), allMonsters[0].GetInstanceID());
    Assert.AreEqual(monstersInjected[1].GetInstanceID(), allMonsters[1].GetInstanceID());
    Assert.AreEqual(monsterAddedFromClass.GetInstanceID(), allMonsters[2].GetInstanceID());
}
```

## Tests with mockable fields

This part describes how to test the mockables fields decorated with Nixi attributes (concerning fields with interface type).

You can read how to test your code for mockables fields cases in the following parts :
  - [Inject and read fields](#inject-and-read-fields)
  - [Inject field for NixInjectFromContainer or SerializeField](#inject-field-for-nixinjectfromcontainer-or-serializefield)
  - [Mock single component field decorating an interface (e.g : NixInjectComponent)](#mock-single-component-field-decorating-an-interface-eg--nixinjectcomponent)
  - [Mock multiple components field having an interface as enumerable type (e.g : NixInjectComponents)](#mock-multiple-components-field-having-an-interface-as-enumerable-type-eg--nixinjectcomponents)

### Inject and read fields

There are several cases where you can **inject** or **read** values directly into fields using TestInjector.

This concerns :

- Fields decorated [with NixInjectFromContainer](#mock-on-nixinjectfromcontainer-and-serializefield-decorators).
- Fields decorated [with SerializeField](#mock-on-nixinjectfromcontainer-and-serializefield-decorators).
- Fields that have an interface type and are decorated with a Nixi attribute to [inject a single component](#fields-with-type-derived-from-interface).
- Fields that have IEnumerable, List or array type with an interface as enumerable type, decorated with a Nixi attribute to [inject multiple component](#enumerable-fields-with-enumerable-type-derived-from-interface).

> You can call InjectField and ReadField with type only, if there is only one field which has the targeted type.
>
> If many are found, you have to specify the field name.

### Inject field for NixInjectFromContainer or SerializeField

You can create a mock in the test and **inject** all the fields : 
  - With an interface type.
  - Decorated with NixInjectFromContainer or SerializeField.

You can also **read** the values in these fields.

You can see an example below.

```cs
public interface IStrategy
{
    string Title { get; }
}

public class StrategyImplementation : IStrategy
{
    public string Title { get; private set; }

    public StrategyImplementation(string title)
    {
        Title = title;
    }
}

public class Warrior : MonoBehaviourInjectable
{
    [NixInjectComponent]
    private IStrategy strategy;

    [SerializeField]
    private Image firstImage;

    [SerializeField]
    private Image secondImage;
}

public class WarriorTests : InjectableTestTemplate<Warrior>
{
    [Test]
    public void InjectAndReadInterface()
    {
        // Read field before inject should return null
        Assert.Null(MainInjector.ReadField<IStrategy>());

        // Here I use an implementation
        // but this could be more pratical to use library like Mock/NSubstitute/etc.
        IStrategy strategyInjected = new StrategyImplementation("Hit and run");
        
        // Inject
        MainInjector.InjectField(strategyInjected);

        // Read should return the StrategyImplementation
        IStrategy strategyFromRead = MainInjector.ReadField<IStrategy>();

        // Should not be null anymore and implementation should be the same
        Assert.IsNotNull(strategyFromRead);
        Assert.AreEqual("Hit and run", strategyFromRead.Title);
    }

    [Test]
    public void InjectAndReadSerializedField_WithTwoSameFieldType()
    {
        // Read field before inject should return null for both image fields
        Assert.Null(MainInjector.ReadField<Image>("firstImage"));
        Assert.Null(MainInjector.ReadField<Image>("secondImage"));

        Image imageInjected = new GameObject().AddComponent<Image>();

        // Inject into Image field named "FirstImage",
        // we have to specify the name because there are two field with Image type
        // (contrary to strategy field)
        MainInjector.InjectField(imageInjected, "firstImage");

        // Read should return the same Image
        Image imageFromRead = MainInjector.ReadField<Image>("firstImage");

        // Second image still null
        Assert.IsNull(MainInjector.ReadField<Image>("secondImage"));

        // First image is not null anymore and imageFromRead should be the same as imageInjected
        Assert.IsNotNull(imageFromRead);
        Assert.AreEqual(imageInjected.GetInstanceID(), imageFromRead.GetInstanceID());
    }
}
```

### Mock single component field decorating an interface (e.g : NixInjectComponent)

You can create a mock in the test and **inject** all the fields : 
  - With an interface type.
  - Decorated with a Nixi attribute for single component injection.

You can also **read** the values in these fields.

You can see an example below.

```cs
public interface IOption
{
    public int OptionValue { get; }
}

public class Option : IOption
{
    public int OptionValue { get; private set; } = 0;

    public Option(int optionValue)
    {
        OptionValue = optionValue;
    }
}

public class GameOptionsMenu : MonoBehaviourInjectable
{
    [NixInjectComponent]
    public IOption Option;

    [NixInjectComponents]
    public List<IOption> Options;
}

public class GameOptionsMenuTests : InjectableTestTemplate<GameOptionsMenu>
{
    [Test]
    public void InjectAndReadInterface()
    {
        // Read field before inject should return null
        Assert.Null(MainInjector.ReadField<IOption>());

        // Here I use an implementation
        // but this could be more pratical to use library like Mock/NSubstitute/etc.
        IOption optionInjected = new Option(4);

        // Inject
        MainInjector.InjectField(optionInjected);

        // Read should return Option implementation
        IOption optionFromRead = MainInjector.ReadField<IOption>();

        // Should not be null anymore and implementation should be the same
        Assert.IsNotNull(optionFromRead);
        Assert.AreEqual(4, optionFromRead.OptionValue);
    }
}
```

### Mock multiple components field having an interface as enumerable type (e.g : NixInjectComponents)

You can create a mock in the test and **inject** all the IEnumerable, List or array fields : 
  - Having an interface as enumerable type.
  - Decorated with a Nixi attribute for multiple components injection.

You can also **read** the values in these fields.

> It is different from initializing multiple components field, these fields are considered as fields to mock. It won't inject all fields with same decorator and type (contrary to [InitEnumerableComponents](#initenumerablecomponents) and their variations)

You can see an example below.

```cs
public interface IOption
{
    public int OptionValue { get; }
}

public class Option : IOption
{
    public int OptionValue { get; private set; } = 0;

    public Option(int optionValue)
    {
        OptionValue = optionValue;
    }
}

public class GameOptionsMenu : MonoBehaviourInjectable
{
    [NixInjectComponent]
    public IOption Option;

    [NixInjectComponents]
    public List<IOption> Options;
}

public class GameOptionsMenuTests : InjectableTestTemplate<GameOptionsMenu>
{
    [Test]
    public void InjectAndRead_InterfaceEnumerable()
    {
        // Read field before inject should return null
        Assert.Null(MainInjector.ReadField<List<IOption>>());

        // Here I use an implementation
        // but this could be more pratical to use library like Mock/NSubstitute/etc.
        List<IOption> optionsInjected = new List<IOption>
        {
            new Option(123)
        };

        // Inject
        MainInjector.InjectField(optionsInjected);

        // Read should return the IOption list with Option implementation
        List<IOption> optionsFromRead = MainInjector.ReadField<List<IOption>>();

        // Should not be null anymore and implementation should be the same
        Assert.IsNotNull(optionsFromRead);
        Assert.AreEqual(1, optionsFromRead.Count);
        Assert.AreEqual(123, optionsFromRead[0].OptionValue);
    }
}
```
## Details of other features of InjectableTestTemplate

There are other features concerning InjectableTestTemplate if you want to use them, you can read the following parts:
  - [InstanceName property](#instancename-property)
  - [ResetTemplate method](#resettemplate-method)
  - [SetTemplateWithConstructor property](#settemplatewithconstructor-property)
  - [Using AbstractComponentMappingContainer](#using-abstractcomponentmappingcontainer)
  - [Template with NUnit framework](#template-with-nunit-framework)

### InstanceName property

[You can refer to this part](#nixinjectrootcomponent-special-case-where-you-want-your-maintested-instance-to-be-recognized-as-a-root-gameobject) to have informations about InstanceName usages.

### ResetTemplate method

This method resets the MainTested instance as well as the TestInjector instance.<br/>
It is called once in the constructor of InjectableTestTemplate if SetTemplateWithConstructor has its default value (true).

It is useful in cases where you need to reset these parameters for each test.<br/>
If you are using NUnit framework you can create a method decorated with [SetUp] and call ResetTemplate() in it.

You can see an example below :
```cs
public class Tests : InjectableTestTemplate<ClassToTest>
{
    // In NUnit framework [SetUp] decorator means :
    // call this method before each test
    [SetUp]
    public void InitTests()
    {
        // Will reset MainTested and MainInjector before each test
        ResetTemplate();
    }
}
```

The adaptated template for NUnit is shown [in this part](#template-with-nunit-framework).

### SetTemplateWithConstructor property

If true (default value), the constructor of InjectableTestTemplate will create once a MainTested instance and a MainInjector instance [with the ResetTemplate method](#resettemplate-method).

If false, InjectableTestTemplate constructor will do nothing special.

### Using AbstractComponentMappingContainer

If you instantiate a component that Unity considers abstract at compile time (such as Renderer) Unity will log an error.

Everytime TestInjector instantiate a component (into single or multiple component(s) field), if this type is abstract, Unity will log the same error.

> If you do new GameObject().AddComponent<Renderer>() Unity will log the following error :
> 
> *Cannot add component of type 'Renderer' because it is abstract. Add component of type that is derived from 'Renderer' instead).*

To solve this problem, there is a special container that allows to map abstract types with derived types (these mappings are called AbstractMapping in the code).
> This container is in the InjectableTestTemplate.

Each mapping added into this container force a type to be used by its derived form during tests using TestInjector.

You can see how to map an abstract mapping in the code example below.

> Abstract mappings can be done on non-abstract types but for the moment I didn't see any use, I didn't lock it in case someone finds a use for it (in which case the name is no longer suitable).

> To avoid complicated cases around handling transform declinaisons, it is not possible to map Transform type.

```cs
public class AbstractComponentExample : MonoBehaviourInjectable
{
    [NixInjectComponent]
    public Renderer Renderer;
}

public class AbstractComponentTests : InjectableTestTemplate<AbstractComponentExample>
{
    // Boolean in the scope of the class because the log is created before the test
    // It is the variable to check if an error log is returned
    private bool abstractErrorWasLog = false;

    // By default ResetTemplate is called once when InjectableTestTemplate constructor is called
    protected override void ResetTemplate()
    {
        // Force TestInjector to use MeshRenderer everytime a component with type Renderer has to be instantiated by it
        AddAbstractComponentMapping<Renderer, MeshRenderer>();

        // Subscribe to Unity console/logger
        Application.logMessageReceived += Application_logMessageReceived;

        // Reset MainTested and MainInjector
        base.ResetTemplate();
    }

    // Method subscribed to Unity console/logger
    // Change abstractErrorWasLog to true if an error concerning abstract component construction is logged by Unity
    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        abstractErrorWasLog = condition.StartsWith("Cannot add component of type 'Renderer' because it is abstract");
    }

    [Test]
    public void Check_AbstractError_WasNotLog()
    {
        Assert.False(abstractErrorWasLog);
    }
}
```
### Template with NUnit framework

I don't know if there are other ways to test your code from Unity with something other than the NUnit framework, but I made the choice to not depend on this library in case it might suit someone.

This is why there is no decorator like "SetUp" in InjectableTestTemplate and why I created a constructor that inits the class even though I know it won't be call on every test, for that you can [read this part](#resettemplate-method).

You can read a complete code template based on NUnit below.

```cs
/// <summary>
/// Test template for MonoBehaviourInjectable using NUnit,
/// it creates an instance of the MonoBehaviourInjectable and use TestInjector 
/// to specially handle dependency injection for testing
/// </summary>
public abstract class InjectableNUnitTemplate<T> : InjectableTestTemplate<T>
    where T : MonoBehaviourInjectable
{
    // False because InitTests will ResetTemplate before each test
    protected override bool SetTemplateWithConstructor => false;

    // Call ResetTemplate (resets the MainTested instance as well as the TestInjector instance) before each test
    [SetUp]
    public virtual void InitTests()
    {
        ResetTemplate();
    }
}

// I created this test class only to show how to use this new
// template and also to show that the instance of MainTested changes
// with each test called (MainInjector also changes with each test).
public class NUnitExampleTests : InjectableNUnitTemplate<Player>
{
    // Store the last MainTested instanceId
    private int lastInstanceId;

    // This test will be run first
    // It is not recommended to use the "Order" parameter
    // because each test must be independent and not wait
    // for another to be executed
    [Test, Order(0)]
    public void NotRecommended_OrderedTest_ToInitInstanceId()
    {
        lastInstanceId = MainTested.GetInstanceID();
    }

    // This test will be run second
    // The MainTested instanceId must have changed
    [Test, Order(1)]
    public void InstanceShouldChange_AfterFirstTest()
    {
        Assert.AreNotEqual(lastInstanceId, MainTested.GetInstanceID());
    }
}
```

## Special cases

### Multiple Transform components

If you are using TestInjector on a class with multiple Transform fields decorated with Nixi attributes (for single or multiple components injection):
- If they all have the same type the injection will work.
- If there are at least two different Transform types (Transform with RectTransform for example), TestInjector will raise an exception.

I blocked it because Unity only allows one type of Transform on each GameObject.

# Support

If you encounter any troubleshooting, you can : 
  - Create a ticket on the project : https://github.com/f-antoine/Nixi
  - Or contact me at this address : nixi.unity@gmail.com