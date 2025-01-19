using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using UnityEngine.TestTools;
using UnityObject = UnityEngine.Object;

namespace AGGE.Input.Tests {
    [TestFixture]
    [RequiresPlayMode]
    sealed class InputLagTest {

        enum TestState {
            Initialized,
            SendingInput,
            AwaitingMovement,
            Finished,
        }

        GameObject camera;

        GameObject entity;

        IEnumerable<GameObject> objects => new[] { camera, entity };

        Keyboard keyboard;

        Vector3 startingPosition;

        int startingFrameCount;
        int deltaFrames;

        TestState state;

        readonly bool useFixture = true;

        void ActivateInput() {
            if (state != TestState.SendingInput) {
                return;
            }

            state = TestState.AwaitingMovement;

            startingFrameCount = Time.frameCount;
            startingPosition = entity.transform.position;

            InputSystem.Update();
            Assert.That(keyboard.spaceKey.isPressed, Is.False);

            if (useFixture) {
                input.Press(keyboard.spaceKey);
            } else {
                StateEvent.From(keyboard, out var eventPtr);
                keyboard.spaceKey.WriteValueIntoEvent(1f, eventPtr);
                InputSystem.QueueEvent(eventPtr);
            }

            InputSystem.Update();
            Assert.That(keyboard.spaceKey.isPressed, Is.True);
        }

        void RenderCamera() {
            if (state != TestState.AwaitingMovement) {
                return;
            }

            if (entity.transform.position != startingPosition) {
                deltaFrames = Time.frameCount - startingFrameCount;
                state = TestState.Finished;
            }
        }

        void OnCameraPreRender(ScriptableRenderContext context, Camera camera) {
            if (camera.gameObject == this.camera) {
                RenderCamera();
            }
        }

        void OnCameraPostRender(ScriptableRenderContext context, Camera camera) {
            if (camera.gameObject == this.camera) {
                ActivateInput();
            }
        }

        readonly InputTestFixture input = new();

        [UnitySetUp]
        public IEnumerator UnitySetUp() {
            yield return CollectGarbage();

            input.Setup();

            yield return null;

            RenderPipelineManager.beginCameraRendering += OnCameraPreRender;
            RenderPipelineManager.endCameraRendering += OnCameraPostRender;

            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsManually;

            keyboard = InputSystem.AddDevice<Keyboard>();
            InputSystem.Update();

            Assert.That(keyboard.enabled, Is.True);
            Assert.That(Keyboard.current, Is.EqualTo(keyboard));
        }

        [UnityTearDown]
        public IEnumerator UnityTearDown() {
            RenderPipelineManager.beginCameraRendering -= OnCameraPreRender;
            RenderPipelineManager.endCameraRendering -= OnCameraPostRender;

            InputSystem.RemoveDevice(keyboard);

            yield return CollectGarbage();

            input.TearDown();

            yield return null;
        }

        IEnumerator CollectGarbage() {
            foreach (var obj in objects) {
                if (obj) {
                    UnityObject.Destroy(obj);
                }
            }

            yield return null;
        }

        [TestCase(typeof(NullInput), EUpdateMode.None, ExpectedResult = null)]
        [TestCase(typeof(InstantiatedInputAssetEventInput), EUpdateMode.FixedUpdate, ExpectedResult = null)]
        [TestCase(typeof(InstantiatedInputAssetEventInput), EUpdateMode.Update, ExpectedResult = null)]
        [TestCase(typeof(InstantiatedInputAssetEventInput), EUpdateMode.LateUpdate, ExpectedResult = null)]
        [TestCase(typeof(InstantiatedInputAssetPhaseInput), EUpdateMode.FixedUpdate, ExpectedResult = null)]
        [TestCase(typeof(InstantiatedInputAssetPhaseInput), EUpdateMode.Update, ExpectedResult = null)]
        [TestCase(typeof(InstantiatedInputAssetPhaseInput), EUpdateMode.LateUpdate, ExpectedResult = null)]
        [UnityTest]
        [Performance]
        public IEnumerator MeasureInputLag(Type type, EUpdateMode mode) {
            camera = new GameObject(nameof(Camera));
            camera.AddComponent<Camera>();

            entity = new GameObject(type.Name);
            entity.AddComponent(type);
            entity.SendMessage(nameof(IUpdateModeMessages.OnUpdateMode), mode, SendMessageOptions.DontRequireReceiver);

            yield return null;

            state = TestState.SendingInput;

            int timeout = Time.frameCount + 10;

            while (state != TestState.Finished && Time.frameCount < timeout) {
                yield return null;
            }

            Assert.That(state, Is.EqualTo(TestState.Finished), $"{entity} never moved!");

            Measure.Custom("Input Lag", deltaFrames);
        }
    }
}