﻿using Common;
using CommonControls.MathViews;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Xna.Framework;
using MonoGame.Framework.WpfInterop;
using System.Linq;
using System.Windows.Input;
using View3D.Commands.Vertex;
using View3D.Components.Component;
using View3D.Components.Component.Selection;
using View3D.Components.Gizmo;
using View3D.Rendering.Geometry;

namespace KitbasherEditor.ViewModels.MenuBarViews
{
    public class TransformToolViewModel : NotifyPropertyChangedImpl
    {
        public enum TransformMode
        { 
            None,
            Rotate,
            Scale,
            Translate
        }

        TransformMode _activeMode = TransformMode.None;

        SelectionManager _selectionManager;
        CommandExecutor _commandExecutor;

        public ICommand ApplyCommand { get; set; }

        bool _buttonEnabled = false;
        public bool ButtonEnabled { get { return _buttonEnabled; } set { SetAndNotify(ref _buttonEnabled, value); } }


        bool _isVisible = false;
        public bool IsVisible { get { return _isVisible; } set { SetAndNotify(ref _isVisible, value); } }

        string _text;
        public string Text { get { return _text; } set { SetAndNotify(ref _text, value); } }

        Vector3ViewModel _vector3 = new Vector3ViewModel(0);
        public Vector3ViewModel Vector3 { get { return _vector3; } set { SetAndNotify(ref _vector3, value); } }

        public TransformToolViewModel(IComponentManager componentManager)
        {
            ApplyCommand = new RelayCommand(ApplyTransform);
            _selectionManager = componentManager.GetComponent<SelectionManager>();
            _commandExecutor = componentManager.GetComponent<CommandExecutor>();

            _selectionManager.SelectionChanged += SelectionChanged;
        }

        private void SelectionChanged(ISelectionState state)
        {
            if (state.Mode == GeometrySelectionMode.Face)
            {
                ButtonEnabled = false;
            }
            else
            {
                if (state is ObjectSelectionState objectSelectionState)
                    ButtonEnabled = objectSelectionState.SelectionCount() != 0;
                else if(state is VertexSelectionState vertexSelectionState)
                    ButtonEnabled = vertexSelectionState.SelectionCount() != 0;
            }
        }

        public void SetMode(TransformMode mode)
        {
            _activeMode = mode;
            IsVisible = _activeMode != TransformMode.None;

            if (_activeMode == TransformMode.Rotate)
                Text = "Rotate:";
            else if (_activeMode == TransformMode.Scale)
                Text = "Scale:";
            else if(_activeMode == TransformMode.Translate)
                Text = "Translate:";

            SetDefaultValue();
        }

        void ApplyTransform()
        {
            var transform = TransformGizmoWrapper.CreateFromSelectionState(_selectionManager.GetState());
            if (transform == null || _activeMode == TransformMode.None) 
                return;

            if (_activeMode == TransformMode.Rotate)
            {
                transform.Start(GizmoMode.Rotate);
                transform.GizmoRotateEvent(
                    Matrix.CreateRotationX(MathHelper.ToRadians((float)_vector3.X.Value)) *
                    Matrix.CreateRotationY(MathHelper.ToRadians((float)_vector3.Y.Value)) *
                    Matrix.CreateRotationZ(MathHelper.ToRadians((float)_vector3.Z.Value)), PivotType.ObjectCenter);
            }
            else if (_activeMode == TransformMode.Translate)
            {
                transform.Start(GizmoMode.Translate);
                transform.GizmoTranslateEvent(new Vector3((float)_vector3.X.Value, (float)_vector3.Y.Value, (float)_vector3.Z.Value), PivotType.ObjectCenter);
            }
            else if (_activeMode == TransformMode.Scale)
            {
                transform.Start(GizmoMode.NonUniformScale);
                transform.GizmoScaleEvent(new Vector3((float)_vector3.X.Value-1, (float)_vector3.Y.Value - 1, (float)_vector3.Z.Value - 1), PivotType.ObjectCenter);    // -1 due to weirdness inside the function
            }

            transform.Stop(_commandExecutor);
            SetDefaultValue();
        }

        void SetDefaultValue()
        {
            if (_activeMode == TransformMode.Scale)
                _vector3.Set(1);
            else
                _vector3.Set(0);
        }

        GizmoMode ConvertToGizmoMode(TransformMode mode)
        {
            switch (mode)
            {
                case TransformMode.Rotate:
                    return GizmoMode.Rotate;
                case TransformMode.Scale:
                    return GizmoMode.NonUniformScale;
                case TransformMode.Translate:
                    return GizmoMode.Translate;
                default:
                    throw new System.Exception();
            }
        }
    }
}
