using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.TestTools;
using UnityObject = UnityEngine.Object;

namespace AGGE.Input.Tests {
    [TestFixture(typeof(NullInput))]
    [TestFixture(typeof(InstantiatedInputAssetEventInput))]
    [TestFixture(typeof(DirectDeviceInput))]
    [TestFixture(typeof(SerializedInputActionInput))]
    [TestFixture(typeof(ReferencedInputActionAssetInput))]
    [TestFixture(typeof(InstantiatedInputAssetPhaseInput))]
    [RequiresPlayMode]
    sealed class InputLagTests<T> where T : MonoBehaviour {

        GameObject camera;

        GameObject entity;

        IEnumerable<GameObject> objects => new[] { camera, entity };

        Keyboard keyboard;

        Vector3 startingPosition;

        int startingFrameCount;
        int deltaFrames;
        bool hasMoved;

        void ActivateInput() {
            startingFrameCount = Time.frameCount;
            startingPosition = entity.transform.position;
            hasMoved = false;

            InputSystem.Update();
            Assert.That(keyboard.spaceKey.isPressed, Is.False);

            input.Press(keyboard.spaceKey);

            InputSystem.Update();
            Assert.That(keyboard.spaceKey.isPressed, Is.True);
        }

        void RenderCamera() {
            if (entity.transform.position != startingPosition) {
                deltaFrames = Time.frameCount - startingFrameCount;
                hasMoved = true;
            }
        }

        // Built-In
        void OnCameraPreRender(Camera camera) {
            if (camera.gameObject == this.camera) {
                RenderCamera();
            }
        }

        // SRP
        void OnCameraPreRender(ScriptableRenderContext context, Camera camera) {
            if (camera.gameObject == this.camera) {
                RenderCamera();
            }
        }

        readonly InputTestFixture input = new();

        [UnitySetUp]
        public IEnumerator UnitySetUp() {
            yield return CollectGarbage();

            input.Setup();

            yield return null;

            Camera.onPreRender += OnCameraPreRender;
            RenderPipelineManager.beginCameraRendering += OnCameraPreRender;

            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsManually;

            keyboard = InputSystem.AddDevice<Keyboard>();

            Assert.That(keyboard.enabled, Is.True);
            Assert.That(Keyboard.current, Is.EqualTo(keyboard));
        }

        [UnityTearDown]
        public IEnumerator UnityTearDown() {
            Camera.onPreRender -= OnCameraPreRender;
            RenderPipelineManager.beginCameraRendering -= OnCameraPreRender;

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

        [Test]
        public void TestInputFixture() {
            InputSystem.Update();

            Assert.That(keyboard.spaceKey.isPressed, Is.False);

            input.Press(keyboard.spaceKey);
            InputSystem.Update();

            Assert.That(keyboard.spaceKey.isPressed, Is.True);
        }
        [UnityTest]
        [Performance]
        public IEnumerator MeasureInputLag() {
            camera = new GameObject(nameof(Camera));
            camera.AddComponent<Camera>();

            entity = new GameObject(typeof(T).Name);
            entity.AddComponent<T>();

            yield return null;

            ActivateInput();

            int timeout = Time.frameCount + 10;

            while (!hasMoved && Time.frameCount < timeout) {
                yield return null;
            }

            Assert.That(hasMoved, Is.True, $"{entity} never moved!");

            Measure.Custom("Input Lag", deltaFrames);
        }
    }
}