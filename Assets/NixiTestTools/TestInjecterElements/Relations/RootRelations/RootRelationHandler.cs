using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements.Relations.RootRelations
{
    /// <summary>
    /// Handle all root components with their children relation at one level below
    /// </summary>
    internal sealed class RootRelationHandler
    {
        /// <summary>
        /// All root components with their children relation at one level below
        /// </summary>
        private List<RootRelation> rootRelations = new List<RootRelation>();

        /// <summary>
        /// Get unique parent root relation with name parentName, null if not found
        /// </summary>
        /// <param name="parentName">Name of the parent relation</param>
        /// <returns>Unique parent with name parentName, null if not found</returns>
        internal RootRelation GetParentRelation(string parentName)
        {
            return rootRelations.SingleOrDefault(x => x.Parent != null && x.Parent.Name == parentName);
        }

        #region Add
        /// <summary>
        /// Create a parent root relation without childs (component is stored at parent level)
        /// </summary>
        /// <param name="parentName">Parent name of the root relation</param>
        /// <param name="parentComponent">Component associated to the parent level of the root relation</param>
        internal void AddParentWithoutChilds(string parentName, Component parentComponent)
        {
            ComponentsWithName parentWithComponentAndNoChild = new ComponentsWithName(parentName, parentComponent);
            rootRelations.Add(new RootRelation(parentWithComponentAndNoChild));
        }

        /// <summary>
        /// Create a parent root relation with his first child (component is stored at child level)
        /// </summary>
        /// <param name="parentName">Parent name of the root relation</param>
        /// <param name="childName">Name of the first child of the root relation</param>
        /// <param name="childComponent">Component associated to the first child of the root relation</param>
        internal void AddParentWithChilds(string parentName, string childName, Component childComponent)
        {
            ComponentsWithName parentWithoutComponent = new ComponentsWithName(parentName);
            List<ComponentsWithName> firstChildWithComponent = new List<ComponentsWithName> { new ComponentsWithName(childName, childComponent) };

            RootRelation parentRelation = new RootRelation(parentWithoutComponent, firstChildWithComponent);
            rootRelations.Add(parentRelation);
        }

        /// <summary>
        /// Add a component into parentRelation if it does not already exists
        /// </summary>
        /// <param name="parentRelation">Parent on which we add a component into the list</param>
        /// <param name="componentToAdd">Component to add to the component at the parent level</param>
        internal void AddComponentIntoParentRelation(RootRelation parentRelation, Component componentToAdd)
        {
            Type componentToAddType = componentToAdd.GetType();

            if (parentRelation.Parent.Components.Any(x => x.GetType() == componentToAddType))
                throw new NotImplementedException($"Cannot add component of type {componentToAddType}, with name {componentToAdd.name} on ParentRootRelation with parentName {parentRelation.Parent.Name}, because a component with this type already exists");

            parentRelation.Parent.AddComponent(componentToAdd);
            RefreshChildsTransformOfParentRelation(parentRelation);
        }

        /// <summary>
        /// Set transform.parent of every child handled by a parent in a root relation to transform from parent
        /// <para/>This makes a component to be 
        /// </summary>
        /// <param name="parentRelation">Parent root relation on which the operation is applied</param>
        private void RefreshChildsTransformOfParentRelation(RootRelation parentRelation)
        {
            foreach (ComponentsWithName child in parentRelation.Childs)
            {
                // Apply on first, apply on all because this on the same gameObject
                Component firstChildComponent = child.Components.FirstOrDefault();
                if (firstChildComponent != null)
                {
                    Component firstParentComponent = parentRelation.Parent.Components.FirstOrDefault();
                    if (firstParentComponent != null)
                    {
                        firstChildComponent.transform.parent = firstParentComponent.transform;
                    }
                }
            }
        }

        /// <summary>
        /// Build first child ComponentsWithName from componentToAdd and childName and link to first component of parent if he is not null
        /// <para/>then add this component to the parentRelation
        /// </summary>
        /// <param name="parentRelation">Parent root relation on which the operation is applied</param>
        /// <param name="childName">Name of the child contains in ComponentsWithName in the child list of the parentRelation</param>
        /// <param name="componentToAdd">Component to add to the list of childs</param>
        internal void AddAndLinkChildToParent(RootRelation parentRelation, string childName, Component componentToAdd)
        {
            ComponentsWithName newComponentsWithName = new ComponentsWithName(childName, componentToAdd);

            // Set to parent because RootObject is parent of ComponentNameToFindName
            Component parentComponent = parentRelation.Parent.Components.FirstOrDefault();
            if (parentComponent != null)
            {
                Component newComponent = newComponentsWithName.Components.Single();
                newComponent.transform.parent = parentComponent.transform;
            }

            parentRelation.AddChildComponent(newComponentsWithName);
        }

        /// <summary>
        /// From child, add component if not already presents in his list of component and link it to the same parent component if it exists
        /// </summary>
        /// <param name="parentRelation">Parent root relation on which the operation is applied</param>
        /// <param name="child">Child on which the operation is performed</param>
        /// <param name="componentToAdd">Component to add to the list of childs</param>
        internal void UpdateAndLinkChildToParent(RootRelation parentRelation, ComponentsWithName child, Component componentToAdd)
        {
            Type componentToAddType = componentToAdd.GetType();

            if (child.Components.Any(x => x.GetType() == componentToAddType))
                throw new NotImplementedException($"Cannot add component of type {componentToAddType}, with name {componentToAdd.name} on ChildRootRelation with parentName {parentRelation.Parent.Name} and childName {child.Name}, because a component with this type already exists");

            // Set to parent because RootObject is parent of ComponentNameToFindName
            Component parentComponent = parentRelation.Parent.Components.FirstOrDefault();
            if (parentComponent != null)
            {
                componentToAdd.transform.parent = parentComponent.transform;
            }

            child.AddComponent(componentToAdd);
        }
        #endregion Add

        #region Get
        /// <summary>
        /// Get ComponentsWithName object child with name childName in a parentRelation
        /// </summary>
        /// <param name="parentRelation">Parent root relation on which the operation is applied</param>
        /// <param name="childName">Name of the child contains in ComponentsWithName in the child list of the parentRelation</param>
        /// <returns>ComponentsWithName object child with name childName in a parentRelation</returns>
        internal ComponentsWithName GetChildComponentsWithName(RootRelation parentRelation, string childName)
        {
            return parentRelation.Childs.SingleOrDefault(x => x.Name == childName);
        }

        /// <summary>
        /// Get all the components of a parent root relation (at parent level) with parentName
        /// </summary>
        /// <param name="parentName">Parent root relation name</param>
        /// <returns>All the components of a parent root relation (at parent level) with parentName</returns>
        internal IEnumerable<Component> GetComponentsFromRelation(string parentName)
        {
            RootRelation parentRelation = GetParentRelation(parentName);

            if (parentRelation == null)
                return null;

            return parentRelation.Parent.Components;
        }

        /// <summary>
        /// Get all the components of a child in a parent root relation with parentName and with childName (at child level from parent rootRelation with parentName)
        /// </summary>
        /// <param name="parentName">Parent root relation name</param>
        /// <param name="childName">Name of the child in parent root relation name</param>
        /// <returns>All the components of a child in a parent root relation with parentName and with childName (at child level from parent rootRelation with parentName)</returns>
        internal IEnumerable<Component> GetComponentsFromRelation(string parentName, string childName)
        {
            if (string.IsNullOrEmpty(childName))
                return GetComponentsFromRelation(parentName);

            RootRelation parentRelation = rootRelations.SingleOrDefault(x => x.Parent != null && x.Parent.Name == parentName);
            if (parentRelation == null)
                return null;

            ComponentsWithName child = parentRelation.Childs.SingleOrDefault(x => x.Name == childName);
            if (child == null)
                return null;

            return child.Components;
        }

        /// <summary>
        /// Get unique component of a parent root relation (at parent level) with parentName, which has type componentType
        /// </summary>
        /// <param name="componentType">Targeted componentType</param>
        /// <param name="parentName">Parent root relation name</param>
        /// <returns>Unique component of a parent root relation (at parent level) with parentName, which has type componentType</returns>
        internal Component GetComponentFromRelation(Type componentType, string parentName)
        {
            RootRelation parentRelation = rootRelations.SingleOrDefault(x => x.Parent != null && x.Parent.Name == parentName);

            if (parentRelation == null)
                return null;

            return parentRelation.Parent.Components.SingleOrDefault(x => x.GetType() == componentType);
        }

        /// <summary>
        /// Get unique component of a child in a parent root relation with parentName and with childName (at child level from parent rootRelation with parentName), which has type componentType
        /// </summary>
        /// <param name="componentType">Targeted componentType</param>
        /// <param name="parentName">Parent root relation name</param>
        /// <param name="childName">Name of the child in parent root relation name</param>
        /// <returns>Unique component of a child in a parent root relation with parentName and with childName (at child level from parent rootRelation with parentName), which has type componentType</returns>
        internal Component GetComponentFromRelation(Type componentType, string parentName, string childName)
        {
            if (string.IsNullOrEmpty(childName))
                return GetComponentFromRelation(componentType, parentName);

            RootRelation parentRelation = rootRelations.SingleOrDefault(x => x.Parent != null && x.Parent.Name == parentName);
            if (parentRelation == null)
                return null;

            ComponentsWithName child = parentRelation.Childs.SingleOrDefault(x => x.Name == childName);
            if (child == null)
                return null;

            return child.Components.SingleOrDefault(x => x.GetType() == componentType);
        }
        #endregion Get
    }
}