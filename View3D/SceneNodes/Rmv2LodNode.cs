﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace View3D.SceneNodes
{
    public class Rmv2LodNode : GroupNode
    {
        public Rmv2LodNode(string name, int lodIndex) : base(name) { LodValue = lodIndex; }
        public int LodValue { get; set; }

        public List<Rmv2MeshNode> GetAllModels(bool onlyVisible)
        {
            var output = new List<Rmv2MeshNode>();
            foreach (var child in Children)
            {
                if (child is Rmv2MeshNode meshNode)
                {
                    if (!(onlyVisible && meshNode.IsVisible == false))
                        output.Add(meshNode);
                }
                else if (child is GroupNode groupNode)
                {
                    if ( !(onlyVisible && groupNode.IsVisible == false) )
                    {
                        foreach (var groupChild in child.Children)
                        {
                            if (groupChild is Rmv2MeshNode meshNode2)
                            {
                                if (!(onlyVisible && meshNode2.IsVisible == false))
                                    output.Add(meshNode2);
                            }
                        }
                    }
                }
            }

            return output;
        }

        public Dictionary<GroupNode, List<Rmv2MeshNode>> GetAllModelsGrouped(bool onlyVisible)
        {
            var output = new Dictionary<GroupNode, List<Rmv2MeshNode>>();

            foreach (var child in Children)
            {
                if (child is Rmv2MeshNode meshNode)
                {
                    if (!(onlyVisible && meshNode.IsVisible == false))
                    {
                        var parentAsGroup = meshNode.Parent as GroupNode;
                        if (output.ContainsKey(parentAsGroup) == false)
                            output.Add(parentAsGroup, new List<Rmv2MeshNode>());
                        output[parentAsGroup].Add(meshNode);
                    }
                }
                else if (child is GroupNode groupNode)
                {
                    if (!(onlyVisible && groupNode.IsVisible == false))
                    {
                        foreach (var groupChild in child.Children)
                        {
                            if (groupChild is Rmv2MeshNode meshNodeInGroup)
                            {
                                if (!(onlyVisible && meshNodeInGroup.IsVisible == false))
                                {
                                    if (output.ContainsKey(groupNode) == false)
                                        output.Add(groupNode, new List<Rmv2MeshNode>());
                                    output[groupNode].Add(meshNodeInGroup);

                                }
                            }
                        }
                    }
                }
            }

            return output;
        }
        
    }

 
}
