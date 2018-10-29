﻿using System;
using osu.Framework.Input.Bindings;
using OpenTK;

namespace Symcol.NeuralNetworking
{
    public abstract class NeuralInputContainer<T> : KeyBindingContainer, IKeyBindingHandler<T>
        where T : struct, IConvertible
    {
        public abstract TensorFlowBrain<T> TensorFlowBrain { get; }

        /// <summary>
        /// All currently usable actions in T
        /// </summary>
        public abstract T[] GetActiveActions { get; }

        protected override void Update()
        {
            base.Update();

            if (TensorFlowBrain.NeuralNetworkState == NeuralNetworkState.Active)
                foreach (T t in GetActiveActions)
                {
                    int i = TensorFlowBrain.GetOutput(t);

                    if (i % 2 == 1)
                        Pressed(t);
                    else
                        Released(t);
                }
            
        }

        #region Input Handling
        public override bool ReceiveMouseInputAt(Vector2 screenSpacePos) => true;

        public bool OnPressed(T action)
        {
            if (TensorFlowBrain.NeuralNetworkState >= NeuralNetworkState.Active) return false;
            Pressed(action);
            return true;

        }

        public Action<T> Pressed;

        public bool OnReleased(T action)
        {
            if (TensorFlowBrain.NeuralNetworkState >= NeuralNetworkState.Active) return false;
            Released(action);
            return true;

        }

        public Action<T> Released;
        #endregion
    }
}
