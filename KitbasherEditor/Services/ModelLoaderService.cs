﻿using Common;
using CommonControls.Services;
using Filetypes.RigidModel;
using FileTypes.PackFiles.Models;
using KitbasherEditor.ViewModels;
using Serilog;
using View3D.Components.Component;
using View3D.Rendering.Geometry;
using View3D.SceneNodes;
using View3D.Services;
using View3D.Utility;

namespace KitbasherEditor.Services
{
    public class ModelLoaderService
    {
        ILogger _logger = Logging.Create<ModelLoaderService>();

        public MainEditableNode EditableMeshNode { get; private set; }
        public ISceneNode ReferenceMeshRoot { get; private set; }

        PackFileService _packFileService;
        ResourceLibary _resourceLibary;
        AnimationControllerViewModel _animationView;
        SceneManager _sceneManager;

        public ModelLoaderService(PackFileService packFileService, ResourceLibary resourceLibary, AnimationControllerViewModel animationView, SceneManager sceneManager, IPackFile mainFile)
        {
            _packFileService = packFileService;
            _resourceLibary = resourceLibary;
            _animationView = animationView;
            _sceneManager = sceneManager;

            var skeletonNode = _sceneManager.RootNode.AddObject(new SkeletonNode(resourceLibary.Content, animationView) { IsLockable = false }) as SkeletonNode;
            EditableMeshNode = (MainEditableNode)_sceneManager.RootNode.AddObject(new MainEditableNode("Editable Model", skeletonNode, mainFile));
            ReferenceMeshRoot = sceneManager.RootNode.AddObject(new GroupNode("Reference meshs") { IsEditable = false, IsLockable=false });
        }

        public void LoadEditableModel(PackFile file)
        {
            var rmv = new RmvRigidModel(file.DataSource.ReadData(), file.Name);
            EditableMeshNode.SetModel(rmv, _resourceLibary, _animationView.Player, GeometryGraphicsContextFactory.CreateInstance(_resourceLibary.GraphicsDevice));

            _animationView.SetActiveSkeleton(rmv.Header.SkeletonName);
        }

        public void LoadReference(string path)
        {
            _logger.Here().Information($"Loading reference model from path - {path}");

            var refereneceMesh = _packFileService.FindFile(path);
            if (refereneceMesh == null)
            {
                _logger.Here().Error("Unable to find file");
                return;
            }

            LoadReference(refereneceMesh as PackFile);
        }

        public void LoadReference(PackFile file, bool updateSkeleton = false)
        {
            _logger.Here().Information($"Loading reference model - {_packFileService.GetFullPath(file)}");

            SceneLoader loader = new SceneLoader(_packFileService, _resourceLibary);
            var outSkeletonName = "";
            var result = loader.Load(file, null, _animationView.Player, ref outSkeletonName);
            if (result == null)
            {
                _logger.Here().Error("Unable to load model");
                return;
            }

            result.ForeachNode((node) => 
            { 
                node.IsEditable = false;
                if (node is ISelectable selectable)
                    selectable.IsSelectable = false;
            });
            ReferenceMeshRoot.AddObject(result);

            //_animationView.SetActiveSkeleton(result.f.Header.SkeletonName);
        }
    }
}
