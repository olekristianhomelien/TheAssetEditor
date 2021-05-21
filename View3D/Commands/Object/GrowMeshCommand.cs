﻿using Microsoft.Xna.Framework;
using MonoGame.Framework.WpfInterop;
using System;
using System.Collections.Generic;
using System.Text;
using View3D.Animation;
using View3D.Rendering.Geometry;
using View3D.SceneNodes;

namespace View3D.Commands.Object
{
    public class GrowMeshCommand : CommandBase<GrowMeshCommand>
    {
        Rmv2MeshNode _node;
        float _factor;
        List<int> _bonesAffectedScale;
        GameSkeleton _gameSkeleton;

        IGeometry _backup;

        public GrowMeshCommand(GameSkeleton skeleton, Rmv2MeshNode node, float factor, List<int> bonesAffectedScale)
        {
            _node = node;
            _factor = factor;
            _gameSkeleton = skeleton;
            _bonesAffectedScale = bonesAffectedScale;
        }

        public override string GetHintText()
        {
            return "Grow mesh";
        }

        public override void Initialize(IComponentManager componentManager)
        {
            //_selectionManager = componentManager.GetComponent<SelectionManager>();
        }

        protected override void ExecuteCommand()
        {
            _backup = _node.Geometry.Clone();

            var mesh = _node.Geometry as Rmv2Geometry;
            var vertexCount = mesh.VertexCount();

            Vector3[] normals = new Vector3[vertexCount];
            float tolerance = 0.0001f;

            for (int i = 0; i < vertexCount; i++)
            {
                normals[i] = mesh.GetVertexExtented(i).Normal;

                // Find all vertexes with the same position
                for (int j = 0; j < vertexCount; j++)
                {
                    if (i == j)
                        continue;

                    var distance = (mesh.GetVertexById(i) - mesh.GetVertexById(j)).LengthSquared();
                    if (distance < tolerance)
                    {
                        normals[i] += mesh.GetVertexExtented(j).Normal;
                    }
                }

                normals[i] = Vector3.Normalize(normals[i]);
            }

            for (int i = 0; i < mesh.VertexCount(); i++)
            {
                var vertex = mesh.GetVertexExtented(i);
                var bones = vertex.GetBoneIndexs();
                var weights = vertex.GetBoneWeights();

                float boneWeight = 0;

                for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
                {
                    if (_bonesAffectedScale.Contains(bones[boneIndex]))
                        boneWeight += weights[boneIndex];
                }

                var offsetMatrix = Matrix.CreateTranslation(normals[i] * _factor * boneWeight);
                mesh.TransformVertex(i, offsetMatrix);
            }

            mesh.RebuildVertexBuffer();
        }

        protected override void UndoCommand()
        {
            _node.Geometry = _backup;
        }
    }
}