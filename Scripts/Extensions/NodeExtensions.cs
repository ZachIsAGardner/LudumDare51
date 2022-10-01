using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using Godot;
using System;

public static class NodeExtensions
{
    public static List<Node> GetHierarchy(this Node node)
    {
        List<Node> children = new List<Node>() { };

        children = node.GetChildren().ToList<Node>();

        Action<Node> recursive = null;
        recursive = (Node child) =>
        {
            List<Node> childsChildren = child.GetChildren().ToList<Node>();
            children.AddRange(childsChildren);
            foreach (Node childsChild in childsChildren) recursive(childsChild);
        };

        foreach (Node child in children.ToList()) recursive(child);

        return children;
    }

    public static T GetChild<T>(this Node node) where T : class
    {
        return node.GetChildren<T>().FirstOrDefault();
    }

    public static List<T> GetChildren<T>(this Node node) where T : class
    {
        return node.GetHierarchy().Where(x => x.GetType() == typeof(T)).Select(x => x as T).ToList<T>();
    }

    public static Node GetChildWithName(this Node node, string name)
    {
        return node.GetHierarchy().Find(x => x.Name == name);
    }

    public static void RemoveChildren(this Node node)
    {
        foreach (Node child in node.GetChildren()) child.QueueFree();;
    }
}